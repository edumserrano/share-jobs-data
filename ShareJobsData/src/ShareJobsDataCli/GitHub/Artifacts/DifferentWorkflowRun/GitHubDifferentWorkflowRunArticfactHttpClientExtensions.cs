namespace ShareJobsDataCli.GitHub.Artifacts.DifferentWorkflowRun;

internal static class GitHubDifferentWorkflowRunArticfactHttpClientExtensions
{
    public static HttpClient ConfigureGitHubDifferentWorkflowRunArticfactHttpClient(
        this HttpClient httpClient,
        GitHubAuthToken authToken,
        GitHubRepositoryName repository)
    {
        authToken.NotNull();
        repository.NotNull();

        httpClient.BaseAddress = new Uri("https://api.github.com");
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("token", authToken);
        httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "application/vnd.github+json");
        httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", $"edumserrano/share-jobs-data:{repository}");
        return httpClient;
    }
}
