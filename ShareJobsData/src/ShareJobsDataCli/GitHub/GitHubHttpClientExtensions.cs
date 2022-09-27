namespace ShareJobsDataCli.GitHub;

internal static class GitHubHttpClientExtensions
{
    // use this to configure a GitHub HTTP Client that works with a standard GitHub
    // authentication token
    public static HttpClient ConfigureGitHubHttpClient(
        this HttpClient httpClient,
        GitHubAuthToken authToken,
        GitHubRepositoryName repository)
    {
        httpClient.NotNull();
        authToken.NotNull();
        repository.NotNull();

        httpClient.BaseAddress = new Uri("https://api.github.com");
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("token", authToken);
        httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "application/vnd.github+json");
        httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", $"edumserrano/share-jobs-data:{repository}");
        return httpClient;
    }

    // use this to configure a GitHub HTTP Client that requires a privileged token
    // the action runtime token is a specific type of token that is only available
    // to the agents running the GitHub workflow. These are not the same type as
    // PAT tokens or for instance the GITHUB_TOKEN. 
    public static HttpClient ConfigureGitHubHttpClient(
        this HttpClient httpClient,
        GitHubActionRuntimeToken actionRuntimeToken,
        GitHubRepositoryName repository)
    {
        httpClient.NotNull();
        actionRuntimeToken.NotNull();
        repository.NotNull();

        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", actionRuntimeToken);
        httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", $"edumserrano/share-jobs-data:{repository}");
        return httpClient;
    }
}
