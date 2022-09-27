namespace ShareJobsDataCli.CliCommands.Commands.ReadDataCurrentWorkflow.DownloadArtifact;

internal static class GitHubDownloadArticfactHttpClientExtensions
{
    public static HttpClient ConfigureGitHubDownloadArticfactHttpClient(
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
