using static ShareJobsDataCli.Features.SetData.UploadArtifact.Results.UploadArtifactFileResult;

namespace ShareJobsDataCli.Features.SetData.UploadArtifact;

internal sealed class GitHubUploadArtifactHttpClient
{
    private readonly HttpClient _httpClient;

    public GitHubUploadArtifactHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient.NotNull();
    }

    // The GitHub's APIs used to upload an artifact are internal APIs which are subject to
    // breaking changes.
    //
    // See: https://github.com/actions/upload-artifact/issues/180#issuecomment-1086306269
    // "They're effectively "internal" APIs that don't hit api.github.com but some of our
    // backend services. Anyone can hit them but we're deliberately not advertising this
    // and these APIs are not documented on https://docs.github.com/en/rest/reference/actions#artifacts"
    //
    // The usage of these internal APIs was reverse engineered by exploring the code flow from:
    // https://github.com/actions/toolkit/blob/03eca1b0c77c26d3eaa0a4e9c1583d6e32b87f6f/packages/artifact/src/internal/upload-http-client.ts#L101
    // https://github.com/actions/toolkit/blob/03eca1b0c77c26d3eaa0a4e9c1583d6e32b87f6f/packages/artifact/src/internal/upload-http-client.ts#L421
    // https://github.com/actions/toolkit/blob/03eca1b0c77c26d3eaa0a4e9c1583d6e32b87f6f/packages/artifact/src/internal/upload-http-client.ts#L542
    public async Task<UploadArtifactFileResult> UploadArtifactFileAsync(
        GitHubArtifactContainerUrl containerUrl,
        GitHubArtifactContainerName containerName,
        GitHubArtifactFileUploadRequest fileUploadRequest,
        CancellationToken cancellation = default)
    {
        containerUrl.NotNull();
        containerName.NotNull();
        fileUploadRequest.NotNull();

        var createArtifactContainer = await CreateArtifactContainerAsync(containerUrl, containerName, cancellation);
        if (!createArtifactContainer.IsOk(out var artifactContainer, out var createArtifactContainerError))
        {
            return new FailedToCreateArtifactContainer(createArtifactContainerError);
        }

        var uploadArtifactItem = await UploadArtifactFileAsync(artifactContainer.FileContainerResourceUrl, fileUploadRequest, cancellation);
        if (!uploadArtifactItem.IsOk(out var artifactItem, out var uploadError))
        {
            return new FailedToUploadArtifact(uploadError);
        }

        var finalizeArtifact = await FinalizeArtifactContainerAsync(
            containerUrl,
            containerName,
            containerSize: artifactItem.FileLength,
            cancellation);
        if (!finalizeArtifact.IsOk(out var gitHubArtifactContainer, out var finalizeError))
        {
            return new FailedToFinalizeArtifactContainer(finalizeError);
        }

        return gitHubArtifactContainer;
    }

    private async Task<JsonHttpResult<GitHubCreateArtifactContainerHttpResponse>> CreateArtifactContainerAsync(
        GitHubArtifactContainerUrl containerUrl,
        GitHubArtifactContainerName containerName,
        CancellationToken cancellationToken)
    {
        using var httpRequest = new HttpRequestMessage(HttpMethod.Post, containerUrl);
        httpRequest.Headers.TryAddWithoutValidation("Accept", $"application/json;api-version={GitHubApiVersion.Latest}");
        var containerRequest = new GitHubCreateArtifactContainerRequest(containerName);
        httpRequest.Content = JsonContent.Create(containerRequest);
        var httpResponse = await _httpClient.SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
        var jsonHttpResult = await httpResponse.ReadFromJsonAsync<GitHubCreateArtifactContainerHttpResponse, GitHubCreateArtifactContainerHttpResponseValidator>(cancellationToken);
        return jsonHttpResult;
    }

    private async Task<JsonHttpResult<GitHubUploadArtifactFileHttpResponse>> UploadArtifactFileAsync(
        string fileContainerResourceUrl,
        GitHubArtifactFileUploadRequest fileUploadRequest,
        CancellationToken cancellationToken)
    {
        var uploadFileUrl = fileContainerResourceUrl.SetQueryParam("itemPath", fileUploadRequest.FilePath);
        using var httpRequest = new HttpRequestMessage(HttpMethod.Put, uploadFileUrl);
        httpRequest.Headers.TryAddWithoutValidation("Accept", $"application/json;api-version={GitHubApiVersion.Latest}");
        var contentBytes = Encoding.UTF8.GetBytes(fileUploadRequest.FileUploadContent);
        httpRequest.Content = new ByteArrayContent(contentBytes);
        httpRequest.Content.Headers.ContentType = new MediaTypeHeaderValue(MediaTypeNames.Application.Octet);
        httpRequest.Content.Headers.ContentRange = new ContentRangeHeaderValue(from: 0, to: contentBytes.Length - 1, length: contentBytes.Length);
        var httpResponse = await _httpClient.SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
        var jsonHttpResult = await httpResponse.ReadFromJsonAsync<GitHubUploadArtifactFileHttpResponse, UploadArtifactFileHttpResponseValidator>(cancellationToken);
        return jsonHttpResult;
    }

    private async Task<JsonHttpResult<GitHubFinalizeArtifactContainerHttpResponse>> FinalizeArtifactContainerAsync(
        GitHubArtifactContainerUrl containerUrl,
        GitHubArtifactContainerName containerName,
        long containerSize,
        CancellationToken cancellationToken)
    {
        var finalizeArtifactContainerRequest = new GitHubFinalizeArtifactContainerRequest(containerSize);
        var setArtifactSizeUrl = $"{containerUrl}".SetQueryParam("artifactName", containerName);
        using var httpRequest = new HttpRequestMessage(HttpMethod.Patch, setArtifactSizeUrl);
        httpRequest.Headers.TryAddWithoutValidation("Accept", $"application/json;api-version={GitHubApiVersion.Latest}");
        httpRequest.Content = JsonContent.Create(finalizeArtifactContainerRequest);
        var httpResponse = await _httpClient.SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
        var jsonHttpResult = await httpResponse.ReadFromJsonAsync<GitHubFinalizeArtifactContainerHttpResponse, GitHubFinalizeArtifactContainerHttpResponseValidator>(cancellationToken);
        return jsonHttpResult;
    }
}
