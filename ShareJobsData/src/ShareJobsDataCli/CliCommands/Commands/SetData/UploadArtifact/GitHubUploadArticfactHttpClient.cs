using static ShareJobsDataCli.CliCommands.Commands.SetData.UploadArtifact.Results.UploadArtifactFileResult;

namespace ShareJobsDataCli.CliCommands.Commands.SetData.UploadArtifact;

internal sealed class GitHubUploadArticfactHttpClient
{
    private readonly HttpClient _httpClient;

    public GitHubUploadArticfactHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient.NotNull();
    }

    // this process is explained here https://github.com/actions/upload-artifact/issues/180#issuecomment-1086306269
    // add something to dev readme about this
    // no need to gzip since content is always expected to be small json model
    // if I gzip on upload I would have to gzip on download
    public async Task<UploadArtifactFileResult> UploadArtifactFileAsync(
        GitHubArtifactContainerUrl containerUrl,
        GitHubArtifactContainerName containerName,
        GitHubArtifactFileUploadRequest fileUploadRequest)
    {
        containerUrl.NotNull();
        containerName.NotNull();
        fileUploadRequest.NotNull();

        var createArtifactContainer = await CreateArtifactContainerAsync(containerUrl, containerName);
        if (!createArtifactContainer.IsOk(out var artifactContainer, out var createArtifactContainerError))
        {
            return new FailedToCreateArtifactContainer(createArtifactContainerError);
        }

        var uploadArtifactItem = await UploadArtifactFileAsync(artifactContainer.FileContainerResourceUrl, fileUploadRequest);
        if (!uploadArtifactItem.IsOk(out var artifactItem, out var uploadError))
        {
            return new FailedToUploadArtifact(uploadError);
        }

        var finalizeArttifact = await FinalizeArtifactContainerAsync(containerUrl, containerName, containerSize: artifactItem.FileLength);
        if (!finalizeArttifact.IsOk(out var gitHubArtifactContainer, out var finalizeError))
        {
            return new FailedToFinalizeArtifactContainer(finalizeError);
        }

        return gitHubArtifactContainer;
    }

    private async Task<JsonHttpResult<GitHubCreateArtifactContainerHttpResponse>> CreateArtifactContainerAsync(
        GitHubArtifactContainerUrl containerUrl,
        GitHubArtifactContainerName containerName)
    {
        using var httpRequest = new HttpRequestMessage(HttpMethod.Post, containerUrl);
        httpRequest.Headers.TryAddWithoutValidation("Accept", $"application/json;api-version={GitHubApiVersion.Latest}");
        var containerRequest = new GitHubCreateArtifactContainerRequest(containerName);
        httpRequest.Content = JsonContent.Create(containerRequest);
        var httpResponse = await _httpClient.SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead);
        var jsonHttpResult = await httpResponse.ReadFromJsonAsync<GitHubCreateArtifactContainerHttpResponse, GitHubCreateArtifactContainerHttpResponseValidator>();
        return jsonHttpResult;
    }

    private async Task<JsonHttpResult<GitHubUploadArtifactFileHttpResponse>> UploadArtifactFileAsync(
        string fileContainerResourceUrl,
        GitHubArtifactFileUploadRequest fileUploadRequest)
    {
        var uploadFileUrl = fileContainerResourceUrl.SetQueryParam("itemPath", fileUploadRequest.FilePath);
        using var httpRequest = new HttpRequestMessage(HttpMethod.Put, uploadFileUrl);
        httpRequest.Headers.TryAddWithoutValidation("Accept", $"application/json;api-version={GitHubApiVersion.Latest}");
        var contentBytes = Encoding.UTF8.GetBytes(fileUploadRequest.FileUploadContent);
        httpRequest.Content = new ByteArrayContent(contentBytes);
        httpRequest.Content.Headers.ContentType = new MediaTypeHeaderValue(MediaTypeNames.Application.Octet);
        httpRequest.Content.Headers.ContentRange = new ContentRangeHeaderValue(from: 0, to: contentBytes.Length - 1, length: contentBytes.Length);
        var httpResponse = await _httpClient.SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead);
        var jsonHttpResult = await httpResponse.ReadFromJsonAsync<GitHubUploadArtifactFileHttpResponse, UploadArtifactFileHttpResponseValidator>();
        return jsonHttpResult;
    }

    private async Task<JsonHttpResult<GitHubFinalizeArtifactContainerHttpResponse>> FinalizeArtifactContainerAsync(
        GitHubArtifactContainerUrl containerUrl,
        GitHubArtifactContainerName containerName,
        long containerSize)
    {
        var finalizeArtifactContainerRequest = new GitHubFinalizeArtifactContainerRequest(containerSize);
        var setArtifactSizeUrl = $"{containerUrl}".SetQueryParam("artifactName", containerName);
        using var httpRequest = new HttpRequestMessage(HttpMethod.Patch, setArtifactSizeUrl);
        httpRequest.Headers.TryAddWithoutValidation("Accept", $"application/json;api-version={GitHubApiVersion.Latest}");
        httpRequest.Content = JsonContent.Create(finalizeArtifactContainerRequest);
        var httpResponse = await _httpClient.SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead);
        var jsonHttpResult = await httpResponse.ReadFromJsonAsync<GitHubFinalizeArtifactContainerHttpResponse, GitHubFinalizeArtifactContainerHttpResponseValidator>();
        return jsonHttpResult;
    }
}
