using static ShareJobsDataCli.GitHub.Artifacts.DifferentWorkflowRun.DownloadArtifactFile.Results.DownloadArtifactFileFromDifferentWorkflowResult;
using static ShareJobsDataCli.GitHub.Artifacts.DifferentWorkflowRun.DownloadArtifactFile.Results.DownloadArtifactZipResult;
using static ShareJobsDataCli.GitHub.JsonHttpResult<T>;

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


        var workflowRunArtifactsResult = await ListWorkflowRunArtifactsAsync(repoName);

        //(Ok okResult, NotOk errorResult) = await ListWorkflowRunArtifactsAsync(repoName);
        //if(errorResult is not null)
        //{
        //    //then it's an error
        //}
        // it's ok, use the okResult
        // how do I guarantee that okResult is not null here?

        if (workflowRunArtifactsResult is not JsonHttpResult<GitHubWorkflowRunArtifactsHttpResponse>.Ok okWorkflowRunArtifactResult)
        {
            var notOk = (JsonHttpResult<GitHubWorkflowRunArtifactsHttpResponse>.Error)workflowRunArtifactsResult;
            switch (notOk)
            {
                case JsonHttpResult<GitHubWorkflowRunArtifactsHttpResponse>.Ok:
                    const int a = 2;
                    break;
                case JsonHttpResult<GitHubWorkflowRunArtifactsHttpResponse>.JsonDeserializedToNull:
                    const int b = 3;
                    break;
                case JsonHttpResult<GitHubWorkflowRunArtifactsHttpResponse>.JsonModelValidationFailed:
                    const int C = 3;
                    break;
                case JsonHttpResult<GitHubWorkflowRunArtifactsHttpResponse>.FailedStatusCode:
                    const int d = 3;
                    break;
            }
            return new FailedToListWorkflowRunArtifacts(workflowRunArtifactsResult);
        }

        var workflowRunArtifacts = okWorkflowRunArtifactResult.Response;
        var artifact = workflowRunArtifacts.Artifacts.FirstOrDefault(x => x.Name == artifactContainerName);
        if (artifact is null)
        {
            return new ArtifactNotFound(repoName, runId, artifactContainerName);
        }

        var downloadArtifactResult = await DownloadArtifactAsync(artifact.ArchiveDownloadUrl);
        if (downloadArtifactResult is not DownloadArtifactZipResult.Ok okDownloadArtifactAzipResult)
        {
            return new FailedToDownloadArtifact(downloadArtifactResult);
        }

        using var artifactZip = okDownloadArtifactAzipResult.ZipArchive;
        var artifactFileAsZip = artifactZip.Entries.FirstOrDefault(e => e.FullName.Equals(artifactItemFilename, StringComparison.InvariantCultureIgnoreCase));
        if (artifactFileAsZip is null)
        {
            return new ArtifactFileNotFound(repoName, runId, artifactContainerName, artifactItemFilename);
        }

        using var artifactAsStream = artifactFileAsZip.Open();
        using var streamReader = new StreamReader(artifactAsStream, Encoding.UTF8);
        var artifactFileContent = await streamReader.ReadToEndAsync();
        return new GitHubArtifactItemContent(artifactFileContent);
    }

    private async Task<JsonHttpResult<GitHubWorkflowRunArtifactsHttpResponse>> ListWorkflowRunArtifactsAsync(string repoName)
    {
        using var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"repos/{repoName}/actions/artifacts");
        var httpResponse = await _httpClient.SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead);
        var jsonHttpResult = await httpResponse.ReadFromJsonAsync<GitHubWorkflowRunArtifactsHttpResponse, GitHubWorkflowRunArtifactsHttpResponseValidator>();
        return jsonHttpResult;
    }

    private async Task<DownloadArtifactZipResult> DownloadArtifactAsync(string archiveDownloadUrl)
    {
        using var httpRequest = new HttpRequestMessage(HttpMethod.Get, archiveDownloadUrl);
        var httpResponse = await _httpClient.SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead);
        var ensureSuccessStatusCodeResult = await httpResponse.EnsureSuccessStatusCodeAsync();
        if (ensureSuccessStatusCodeResult is not EnsureSuccessStatusCodeResult.Ok)
        {
            return new FailedToDownloadArtifactZip(ensureSuccessStatusCodeResult);
        }

        var responseStream = await httpResponse.Content.ReadAsStreamAsync();
        return new ZipArchive(responseStream, ZipArchiveMode.Read);
    }
}
