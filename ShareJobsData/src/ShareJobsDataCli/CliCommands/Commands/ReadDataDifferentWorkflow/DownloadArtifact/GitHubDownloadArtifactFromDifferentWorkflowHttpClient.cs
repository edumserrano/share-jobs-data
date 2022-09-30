using static ShareJobsDataCli.CliCommands.Commands.ReadDataDifferentWorkflow.DownloadArtifact.Results.DownloadArtifactFileFromDifferentWorkflowResult;
using static ShareJobsDataCli.CliCommands.Commands.ReadDataDifferentWorkflow.DownloadArtifact.Results.DownloadArtifactZipResult;

namespace ShareJobsDataCli.CliCommands.Commands.ReadDataDifferentWorkflow.DownloadArtifact;

internal sealed class GitHubDownloadArtifactFromDifferentWorkflowHttpClient
{
    private readonly HttpClient _httpClient;

    public GitHubDownloadArtifactFromDifferentWorkflowHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient.NotNull();
    }

    public async Task<DownloadArtifactFileFromDifferentWorkflowResult> DownloadArtifactFileAsync(
        GitHubRepositoryName repoName,
        GitHubRunId runId,
        GitHubArtifactContainerName artifactContainerName,
        GitHubArtifactItemFilename artifactItemFilename)
    {
        repoName.NotNull();
        runId.NotNull();
        artifactContainerName.NotNull();
        artifactItemFilename.NotNull();

        var workflowRunArtifactsResult = await ListWorkflowRunArtifactsAsync(repoName, runId);
        if (!workflowRunArtifactsResult.IsOk(out var workflowRunArtifacts, out var errorJsonHttpResult))
        {
            return new FailedToListWorkflowRunArtifacts(errorJsonHttpResult);
        }

        var artifact = workflowRunArtifacts.Artifacts.FirstOrDefault(x => string.Equals(x.Name, artifactContainerName, StringComparison.Ordinal));
        if (artifact is null)
        {
            return new ArtifactNotFound(repoName, runId, artifactContainerName);
        }

        var downloadArtifactResult = await DownloadArtifactAsync(artifact.ArchiveDownloadUrl);
        if (!downloadArtifactResult.IsOk(out var artifactZip, out var failedStatusCodeHttpResponse))
        {
            return new FailedToDownloadArtifact(failedStatusCodeHttpResponse);
        }

        using (artifactZip)
        {
            var artifactFileAsZip = artifactZip.Entries.FirstOrDefault(e => e.FullName.Equals(artifactItemFilename, StringComparison.OrdinalIgnoreCase));
            if (artifactFileAsZip is null)
            {
                return new ArtifactFileNotFound(repoName, runId, artifactContainerName, artifactItemFilename);
            }

            await using var artifactAsStream = artifactFileAsZip.Open();
            using var streamReader = new StreamReader(artifactAsStream, Encoding.UTF8);
            var artifactFileContent = await streamReader.ReadToEndAsync();
            var createArtifactItemJsonContentResult = GitHubArtifactItemJsonContent.Create(artifactFileContent);
            if (!createArtifactItemJsonContentResult.IsOk(out var gitHubArtifactItemJsonContent, out var notJsonContent))
            {
                return new ArtifactItemContentNotJson(notJsonContent);
            }

            return gitHubArtifactItemJsonContent;
        }
    }

    private async Task<JsonHttpResult<GitHubListWorkflowRunArtifactsHttpResponse>> ListWorkflowRunArtifactsAsync(GitHubRepositoryName repoName, GitHubRunId runId)
    {
        using var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"repos/{repoName}/actions/runs/{runId}/artifacts");
        var httpResponse = await _httpClient.SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead);
        var jsonHttpResult = await httpResponse.ReadFromJsonAsync<GitHubListWorkflowRunArtifactsHttpResponse, GitHubListWorkflowRunArtifactsHttpResponseValidator>();
        return jsonHttpResult;
    }

    private async Task<DownloadArtifactZipResult> DownloadArtifactAsync(string archiveDownloadUrl)
    {
        using var httpRequest = new HttpRequestMessage(HttpMethod.Get, archiveDownloadUrl);
        var httpResponse = await _httpClient.SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead);
        var ensureSuccessStatusCodeResult = await httpResponse.EnsureSuccessStatusCodeAsync();
        if (!ensureSuccessStatusCodeResult.IsOk(out var failedStatusCodeHttpResponse))
        {
            return new FailedToDownloadArtifactZip(failedStatusCodeHttpResponse);
        }

        var responseStream = await httpResponse.Content.ReadAsStreamAsync();
        return new ZipArchive(responseStream, ZipArchiveMode.Read);
    }
}
