using static ShareJobsDataCli.Features.ReadDataDifferentWorkflow.DownloadArtifact.Results.DownloadArtifactFileFromDifferentWorkflowResult;
using static ShareJobsDataCli.Features.ReadDataDifferentWorkflow.DownloadArtifact.Results.DownloadArtifactZipResult;

namespace ShareJobsDataCli.Features.ReadDataDifferentWorkflow.DownloadArtifact;

internal sealed class GitHubDownloadArtifactFromDifferentWorkflowHttpClient
{
    private readonly HttpClient _httpClient;

    public GitHubDownloadArtifactFromDifferentWorkflowHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient.NotNull();
    }

    // based on the public APIs documented at https://docs.github.com/en/rest/actions/artifacts
    public async Task<DownloadArtifactFileFromDifferentWorkflowResult> DownloadArtifactFileAsync(
        GitHubRepositoryName repoName,
        GitHubRunId runId,
        GitHubArtifactContainerName artifactContainerName,
        GitHubArtifactItemFilename artifactItemFilename,
        CancellationToken cancellationToken = default)
    {
        repoName.NotNull();
        runId.NotNull();
        artifactContainerName.NotNull();
        artifactItemFilename.NotNull();

        var workflowRunArtifactsResult = await ListWorkflowRunArtifactsAsync(repoName, runId, cancellationToken);
        if (!workflowRunArtifactsResult.IsOk(out var workflowRunArtifacts, out var errorJsonHttpResult))
        {
            return new FailedToListWorkflowRunArtifacts(errorJsonHttpResult);
        }

        var artifact = workflowRunArtifacts.Artifacts.FirstOrDefault(x => string.Equals(x.Name, artifactContainerName, StringComparison.Ordinal));
        if (artifact is null)
        {
            return new ArtifactNotFound(repoName, runId, artifactContainerName);
        }

        var downloadArtifactResult = await DownloadArtifactAsync(artifact.ArchiveDownloadUrl, cancellationToken);
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
            var artifactFileContent = await streamReader.ReadToEndAsync(cancellationToken);
            var createArtifactItemJsonContentResult = GitHubArtifactItemJsonContent.Create(artifactFileContent);
            if (!createArtifactItemJsonContentResult.IsOk(out var gitHubArtifactItemJsonContent, out var notJsonContent))
            {
                return new ArtifactItemContentNotJson(notJsonContent);
            }

            return gitHubArtifactItemJsonContent;
        }
    }

    private async Task<JsonHttpResult<GitHubListWorkflowRunArtifactsHttpResponse>> ListWorkflowRunArtifactsAsync(
        GitHubRepositoryName repoName,
        GitHubRunId runId,
        CancellationToken cancellationToken)
    {
        using var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"repos/{repoName}/actions/runs/{runId}/artifacts");
        var httpResponse = await _httpClient.SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
        var jsonHttpResult = await httpResponse.ReadFromJsonAsync<GitHubListWorkflowRunArtifactsHttpResponse, GitHubListWorkflowRunArtifactsHttpResponseValidator>(cancellationToken);
        return jsonHttpResult;
    }

    private async Task<DownloadArtifactZipResult> DownloadArtifactAsync(string archiveDownloadUrl, CancellationToken cancellationToken)
    {
        using var httpRequest = new HttpRequestMessage(HttpMethod.Get, archiveDownloadUrl);
        var httpResponse = await _httpClient.SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
        var ensureSuccessStatusCodeResult = await httpResponse.EnsureSuccessStatusCodeAsync(cancellationToken);
        if (!ensureSuccessStatusCodeResult.IsOk(out var failedStatusCodeHttpResponse))
        {
            return new FailedToDownloadArtifactZip(failedStatusCodeHttpResponse);
        }

        var responseStream = await httpResponse.Content.ReadAsStreamAsync(cancellationToken);
        return new ZipArchive(responseStream, ZipArchiveMode.Read);
    }
}
