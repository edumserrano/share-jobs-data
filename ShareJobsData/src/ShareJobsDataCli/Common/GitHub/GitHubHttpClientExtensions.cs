namespace ShareJobsDataCli.Common.GitHub;

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
    //
    // See: https://github.com/actions/upload-artifact/issues/180#issuecomment-1086306269
    // "They're effectively "internal" APIs that don't hit api.github.com but some of our
    // backend services. Anyone can hit them but we're deliberately not advertising this
    // and these APIs are not documented on https://docs.github.com/en/rest/reference/actions#artifacts
    // because it works with a special token the runner has as opposed to GITHUB_TOKEN or a PAT.
    // Outside of the context of a run you can't upload artifacts."
    //
    // Note: "Outside of the context of a run you can't upload artifacts" OR download artifacts
    // that were uploaded during the workflow BEFORE the workflow ends. You can do both of these
    // only with the access to the action runtime time and the internal APIs for upload/download
    // of artifacts. This is represented in this code base by the set-data and read-data-current-workflow
    // commands.
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
