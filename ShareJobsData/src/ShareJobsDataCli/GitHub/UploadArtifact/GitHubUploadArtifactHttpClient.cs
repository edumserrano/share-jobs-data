namespace ShareJobsDataCli.GitHub.UploadArtifact;

internal class GitHubUploadArtifactHttpClient
{
    private readonly HttpClient _httpClient;

    public GitHubUploadArtifactHttpClient(HttpClient httpClient)
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

    // this process is explained here https://github.com/actions/upload-artifact/issues/180#issuecomment-1086306269
    // add something to dev readme about this
    // no need to gzip since content is always expected to be small json model
    public async Task UploadArtifactAsync(
        GitHubArtifactContainerUrl containerUrl,
        GitHubUploadArtifact artifact)
    {
        containerUrl.NotNull();
        artifact.NotNull();

        var createArtifactFileContainerResponse = await CreateArtifactFileContainerAsync(containerUrl, artifact.ContainerName);
        Console.WriteLine($"createArtifactFileContainerResponse: {createArtifactFileContainerResponse}");

        var uploadFileResponse = await UploadArtifactFileAsync(createArtifactFileContainerResponse.FileContainerResourceUrl, artifact);
        Console.WriteLine($"uploadFileResponse: {uploadFileResponse}");

        var finalizeArtifactResponse = await FinalizeArtifactContainerAsync(containerUrl, artifact, uploadFileResponse.FileLength);
        Console.WriteLine($"finalizeArtifactResponse: {finalizeArtifactResponse}");
    }

    private async Task<GitHubUpdateArtifactFileContainerResponse> CreateArtifactFileContainerAsync(GitHubArtifactContainerUrl containerUrl, string containerName)
    {
        using var httpRequest = new HttpRequestMessage(HttpMethod.Post, containerUrl);
        var containerRequest = new GitHubCreateArtifactFileContainerRequest(containerName);
        httpRequest.Content = JsonContent.Create(containerRequest);
        var httpResponse = await _httpClient.SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead);
        if (!httpResponse.IsSuccessStatusCode)
        {
            var errorResponseBody = await httpResponse.Content.ReadAsStringAsync();
            throw new HttpClientResponseException(httpRequest.Method, $"{httpRequest.RequestUri}", httpResponse.StatusCode, errorResponseBody);
        }

        var createArtifactFileContainerResponse = await httpResponse.Content.ReadFromJsonAsync<GitHubUpdateArtifactFileContainerResponse>();
        if (createArtifactFileContainerResponse is null)
        {
            throw HttpResponseValidationException.JsonDeserializedToNull<GitHubUpdateArtifactFileContainerResponse>();
        }

        var validator = new GitHubUpdateArtifactFileContainerResponseValidator();
        var validationResult = validator.Validate(createArtifactFileContainerResponse);
        if (!validationResult.IsValid)
        {
            throw HttpResponseValidationException.ValidationFailed<GitHubUpdateArtifactFileContainerResponse>(validationResult);
        }

        return createArtifactFileContainerResponse;
    }

    private async Task<GitHubUpdateArtifactResponse> UploadArtifactFileAsync(
        string fileContainerResourceUrl,
        GitHubUploadArtifact artifact)
    {
        var contentBytes = Encoding.UTF8.GetBytes(artifact.FilePayload);
        using var stream = new MemoryStream(contentBytes);
        var uploadFileUrl = fileContainerResourceUrl.SetQueryParam("itemPath", artifact.FilePath);
        using var httpRequest = new HttpRequestMessage(HttpMethod.Put, uploadFileUrl);
        httpRequest.Content = new StreamContent(stream);
        httpRequest.Content.Headers.ContentType = new MediaTypeHeaderValue(MediaTypeNames.Application.Octet);
        httpRequest.Content.Headers.ContentRange = new ContentRangeHeaderValue(from: 0, to: contentBytes.Length - 1, length: contentBytes.Length);
        var httpResponse = await _httpClient.SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead);
        if (!httpResponse.IsSuccessStatusCode)
        {
            var errorResponseBody = await httpResponse.Content.ReadAsStringAsync();
            throw new HttpClientResponseException(httpRequest.Method, $"{httpRequest.RequestUri}", httpResponse.StatusCode, errorResponseBody);
        }

        var updateArtifactResponse = await httpResponse.Content.ReadFromJsonAsync<GitHubUpdateArtifactResponse>();
        if (updateArtifactResponse is null)
        {
            throw HttpResponseValidationException.JsonDeserializedToNull<GitHubUpdateArtifactResponse>();
        }

        var validator = new GitHubUpdateArtifactResponseValidator();
        var validationResult = validator.Validate(updateArtifactResponse);
        if (!validationResult.IsValid)
        {
            throw HttpResponseValidationException.ValidationFailed<GitHubUpdateArtifactResponse>(validationResult);
        }

        return updateArtifactResponse;
    }

    private async Task<GitHubUpdateArtifactFileContainerResponse> FinalizeArtifactContainerAsync(
        GitHubArtifactContainerUrl containerUrl,
        GitHubUploadArtifact artifact,
        int containerSize)
    {
        var finalizeArtifactContainerRequest = new GitHubFinalizeArtifactContainerRequest(containerSize);
        var setArtifactSizeUrl = $"{containerUrl}".SetQueryParam("artifactName", artifact.ContainerName);
        using var httpRequest = new HttpRequestMessage(HttpMethod.Patch, setArtifactSizeUrl);
        httpRequest.Content = JsonContent.Create(finalizeArtifactContainerRequest);
        var httpResponse = await _httpClient.SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead);
        if (!httpResponse.IsSuccessStatusCode)
        {
            var errorResponseBody = await httpResponse.Content.ReadAsStringAsync();
            throw new HttpClientResponseException(httpRequest.Method, $"{httpRequest.RequestUri}", httpResponse.StatusCode, errorResponseBody);
        }

        var finalizeArtifactContainerResponse = await httpResponse.Content.ReadFromJsonAsync<GitHubUpdateArtifactFileContainerResponse>();
        if (finalizeArtifactContainerResponse is null)
        {
            throw HttpResponseValidationException.JsonDeserializedToNull<GitHubUpdateArtifactFileContainerResponse>();
        }

        var validator = new GitHubUpdateArtifactFileContainerResponseValidator();
        var validationResult = validator.Validate(finalizeArtifactContainerResponse);
        if (!validationResult.IsValid)
        {
            throw HttpResponseValidationException.ValidationFailed<GitHubUpdateArtifactFileContainerResponse>(validationResult);
        }

        return finalizeArtifactContainerResponse;
    }
}
