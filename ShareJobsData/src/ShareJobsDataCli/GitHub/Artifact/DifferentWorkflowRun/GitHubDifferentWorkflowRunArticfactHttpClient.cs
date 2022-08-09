namespace ShareJobsDataCli.GitHub.Artifact.DifferentWorkflowRun;

internal class GitHubDifferentWorkflowRunArticfactHttpClient
{
    private readonly HttpClient _httpClient;

    public GitHubDifferentWorkflowRunArticfactHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient.NotNull();
    }

    public static HttpClient CreateHttpClient(GitHubActionRuntimeToken actionRuntimeToken, GitHubRepositoryName repository)
    {
        actionRuntimeToken.NotNull();
        repository.NotNull();

        var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", actionRuntimeToken);
        httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", $"edumserrano/share-jobs-data:{repository}");
        return httpClient;
    }
}
