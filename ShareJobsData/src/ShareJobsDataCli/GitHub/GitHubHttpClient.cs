using ShareJobsDataCli.GitHub.UploadArtifact.HttpModels.Responses;

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
    public async Task<GitHubArtifactFileContainerResponse> DownloadArtifactsAsync(GitHubArtifactContainerUrl containerUrl, string artifactName)
    {
        containerUrl.NotNull();

        var listArtifactsResponse = await ListArtifactsAsync(containerUrl);
        Console.WriteLine($"listArtifactsResponse: {listArtifactsResponse}");

        var artifact = listArtifactsResponse.Artifacts.FirstOrDefault(x => x.Name == artifactName);
        if (artifact is null)
        {
            //abort
            throw new InvalidOperationException();
        }

        await GetContainerItemsAsync(artifact.FileContainerResourceUrl, artifact.Name);

        return null!;
    }

    private async Task<GitHubListArtifactsResponse> ListArtifactsAsync(GitHubArtifactContainerUrl containerUrl)
    {
        using var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"{containerUrl}");
        var httpResponse = await _httpClient.SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead);
        if (!httpResponse.IsSuccessStatusCode)
        {
            var errorResponseBody = await httpResponse.Content.ReadAsStringAsync();
            throw new HttpClientResponseException(httpRequest.Method, $"{httpRequest.RequestUri}", httpResponse.StatusCode, errorResponseBody);
        }

        var listArtifactsResponse = await httpResponse.Content.ReadFromJsonAsync<GitHubListArtifactsResponse>();
        if (listArtifactsResponse is null)
        {
            throw HttpResponseValidationException.JsonDeserializedToNull<GitHubListArtifactsResponse>();
        }

        var validator = new GitHubListArtifactsResponseValidator();
        var validationResult = validator.Validate(listArtifactsResponse);
        if (!validationResult.IsValid)
        {
            throw HttpResponseValidationException.ValidationFailed<GitHubListArtifactsResponse>(validationResult);
        }

        return listArtifactsResponse;
    }

    private async Task GetContainerItemsAsync(string fileContainerResourceUrl, string artifactName)
    {
        var getContainerItemsUrl = fileContainerResourceUrl.SetQueryParam("itemPath", artifactName);
        using var httpRequest = new HttpRequestMessage(HttpMethod.Get, getContainerItemsUrl);
        var httpResponse = await _httpClient.SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead);
        if (!httpResponse.IsSuccessStatusCode)
        {
            var errorResponseBody = await httpResponse.Content.ReadAsStringAsync();
            throw new HttpClientResponseException(httpRequest.Method, $"{httpRequest.RequestUri}", httpResponse.StatusCode, errorResponseBody);
        }

        var raw = await httpResponse.Content.ReadAsStringAsync();
        Console.WriteLine($"GetContainerItemsAsync-raw: {raw}");

        //var listArtifactsResponse = await httpResponse.Content.ReadFromJsonAsync<GitHubListArtifactsResponse>();
        //if (listArtifactsResponse is null)
        //{
        //    throw HttpResponseValidationException.JsonDeserializedToNull<GitHubListArtifactsResponse>();
        //}

        //var validator = new GitHubListArtifactsResponseValidator();
        //var validationResult = validator.Validate(listArtifactsResponse);
        //if (!validationResult.IsValid)
        //{
        //    throw HttpResponseValidationException.ValidationFailed<GitHubListArtifactsResponse>(validationResult);
        //}

        //return listArtifactsResponse;
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
