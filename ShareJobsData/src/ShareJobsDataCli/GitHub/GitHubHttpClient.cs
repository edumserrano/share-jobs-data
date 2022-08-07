namespace ShareJobsDataCli.GitHub;

internal class GitHubHttpClient
{
    private readonly HttpClient _httpClient;

    public GitHubHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient.NotNull();
    }

    public static HttpClient CreateHttpClient(GitHubAuthToken authToken, GitHubRepository repository)
    {
        authToken.NotNull();
        repository.NotNull();

        var httpClient = new HttpClient
        {
            BaseAddress = new Uri("https://api.github.com"),
        };
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("token", authToken);
        httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "application/vnd.github.v3+json");
        httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", $"edumserrano/share-jobs-data:{repository}");
        return httpClient;
    }

    // see https://docs.github.com/en/rest/actions/artifacts#list-workflow-run-artifacts
    public async Task ListArtifactsAsync(GitHubRepository repo, GitHubActionRunId runId)
    {
        repo.NotNull();
        runId.NotNull();

        using var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"repos/{repo}/actions/runs/{runId}/artifacts");
        var httpResponse = await _httpClient.SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead);
        Console.WriteLine($"ListArtifactsAsync-status-code: {httpResponse.StatusCode}");
        var raw = await httpResponse.Content.ReadAsStringAsync();
        Console.WriteLine($"ListArtifactsAsync-raw: {raw}");


        //if (!httpResponse.IsSuccessStatusCode)
        //{
        //    var errorResponseBody = await httpResponse.Content.ReadAsStringAsync();
        //    throw new HttpClientResponseException(httpRequest.Method, $"{httpRequest.RequestUri}", httpResponse.StatusCode, errorResponseBody);
        //}

        //var responseStream = await httpResponse.Content.ReadAsStreamAsync();
        //return new ZipArchive(responseStream, ZipArchiveMode.Read);
    }

    // see https://docs.github.com/en/rest/actions/artifacts#download-an-artifact
    public async Task<ZipArchive> DownloadArtifactAsync(GitHubRepository repo, GitHubArtifactId artifactId)
    {
        repo.NotNull();
        artifactId.NotNull();

        using var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"repos/{repo}/actions/artifacts/{artifactId}/zip");
        var httpResponse = await _httpClient.SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead);
        if (!httpResponse.IsSuccessStatusCode)
        {
            var errorResponseBody = await httpResponse.Content.ReadAsStringAsync();
            throw new HttpClientResponseException(httpRequest.Method, $"{httpRequest.RequestUri}", httpResponse.StatusCode, errorResponseBody);
        }

        var responseStream = await httpResponse.Content.ReadAsStreamAsync();
        return new ZipArchive(responseStream, ZipArchiveMode.Read);
    }
}
