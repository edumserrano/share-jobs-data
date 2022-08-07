namespace ShareJobsDataCli.GitHub;

internal class GitHubHttpClient
{
    private readonly HttpClient _httpClient;

    public GitHubHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient.NotNull();
    }

    public static HttpClient CreateHttpClient(GitHubActionRuntimeToken actionRuntimeToken, GitHubRepository repository)
    {
        actionRuntimeToken.NotNull();
        repository.NotNull();

        var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", actionRuntimeToken);
        httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept", $"application/json;api-version={GitHubApiVersion.Latest}");
        httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", $"edumserrano/share-jobs-data:{repository}");
        return httpClient;
    }

    // taken from exploring https://github.com/actions/toolkit/blob/90be12a59c20a6ecc43b234c1885fc2852d3212d/packages/artifact/src/internal/artifact-client.ts#L157
    public async Task ListArtifactsAsync(GitHubArtifactContainerUrl containerUrl)
    {
        containerUrl.NotNull();

        using var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"{containerUrl}");
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
