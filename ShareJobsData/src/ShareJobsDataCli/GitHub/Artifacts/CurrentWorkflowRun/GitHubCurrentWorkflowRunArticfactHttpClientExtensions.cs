namespace ShareJobsDataCli.GitHub.Artifacts.CurrentWorkflowRun;

internal static class GitHubCurrentWorkflowRunArticfactHttpClientExtensions
{
    public static HttpClient ConfigureGitHubCurrentWorkflowRunArticfactHttpClient(
        this HttpClient httpClient,
        GitHubActionRuntimeToken actionRuntimeToken,
        GitHubRepositoryName repository)
    {
        actionRuntimeToken.NotNull();
        repository.NotNull();

        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", actionRuntimeToken);
        httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", $"edumserrano/share-jobs-data:{repository}");
        return httpClient;
    }
}
