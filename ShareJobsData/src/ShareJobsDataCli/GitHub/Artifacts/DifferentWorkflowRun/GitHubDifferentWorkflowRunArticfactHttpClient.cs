using ShareJobsDataCli.ArgumentValidations;

namespace ShareJobsDataCli.GitHub.Artifacts.DifferentWorkflowRun;

internal class GitHubDifferentWorkflowRunArticfactHttpClient
{
    private readonly HttpClient _httpClient;

    public GitHubDifferentWorkflowRunArticfactHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient.NotNull();
    }

    public static HttpClient CreateHttpClient(GitHubAuthToken authToken, GitHubRepositoryName repository)
    {
        authToken.NotNull();
        repository.NotNull();

        var httpClient = new HttpClient
        {
            BaseAddress = new Uri("https://api.github.com"),
        };
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("token", authToken);
        httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "application/vnd.github+json");
        httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", $"edumserrano/share-jobs-data:{repository}");
        return httpClient;
    }

    public async Task<GitHubArtifactItemContent> DownloadArtifactFileAsync(
        GitHubRepositoryName repoName,
        GitHubRunId runId,
        GitHubArtifactContainerName artifactContainerName,
        GitHubArtifactItemFilename artifactItemFilename)
    {
        repoName.NotNull();
        runId.NotNull();
        artifactContainerName.NotNull();
        artifactItemFilename.NotNull();

        var workflowRunArtifacts = await ListWorkflowRunArtifactsAsync(repoName);
        var artifact = workflowRunArtifacts.Artifacts.FirstOrDefault(x => x.Name == artifactContainerName);
        if (artifact is null)
        {
            throw DownloadArtifactException.ArtifactNotFound(repoName, runId, artifactContainerName);
        }

        using var artifactZip = await DownloadArtifactAsync(artifact.ArchiveDownloadUrl);
        var artifactFileAsZip = artifactZip.Entries.FirstOrDefault(e => e.FullName.Equals(artifactItemFilename, StringComparison.InvariantCultureIgnoreCase));
        if (artifactFileAsZip is null)
        {
            throw DownloadArtifactException.ArtifactFileNotFound(repoName, runId, artifactItemFilename);
        }

        using var artifactAsStream = artifactFileAsZip.Open();
        using var streamReader = new StreamReader(artifactAsStream, Encoding.UTF8);
        var artifactFileContent = await streamReader.ReadToEndAsync();
        return new GitHubArtifactItemContent(artifactFileContent);
    }

    private async Task<GitHubWorkflowRunArtifacts> ListWorkflowRunArtifactsAsync(string repoName)
    {
        using var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"repos/{repoName}/actions/artifacts");
        var httpResponse = await _httpClient.SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead);
        var workflowRunArtifacts = await httpResponse.ReadFromJsonAsync<GitHubWorkflowRunArtifacts, WorkflowRunArtifactsValidator>();
        return workflowRunArtifacts;
    }

    private async Task<ZipArchive> DownloadArtifactAsync(string archiveDownloadUrl)
    {
        using var httpRequest = new HttpRequestMessage(HttpMethod.Get, archiveDownloadUrl);
        var httpResponse = await _httpClient.SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead);
        await httpResponse.EnsureSuccessStatusCodeAsync();
        var responseStream = await httpResponse.Content.ReadAsStreamAsync();
        return new ZipArchive(responseStream, ZipArchiveMode.Read);
    }
}
