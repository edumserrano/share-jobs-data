namespace ShareJobsDataCli.CliCommands.Commands.SetData.UploadArtifact;

internal static class GitHubUploadArticfactHttpClientExtensions
{
    public static HttpClient ConfigureGitHubUploadArticfactHttpClient(
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
