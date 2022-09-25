using static ShareJobsDataCli.GitHub.Artifacts.CurrentWorkflowRun.DownloadArtifactFile.Results.DownloadArtifactFileFromCurrentWorkflowResult;
using static ShareJobsDataCli.GitHub.Artifacts.CurrentWorkflowRun.DownloadArtifactFile.Results.DownloadContainerItemResult;
using static ShareJobsDataCli.GitHub.Artifacts.CurrentWorkflowRun.UploadArtifactFile.Results.UploadArtifactFileResult;

namespace ShareJobsDataCli.GitHub.Artifacts.CurrentWorkflowRun;

internal class GitHubCurrentWorkflowRunArticfactHttpClient
{
    private readonly HttpClient _httpClient;

    public GitHubCurrentWorkflowRunArticfactHttpClient(HttpClient httpClient)
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

    // taken from exploring https://github.com/actions/toolkit/blob/90be12a59c20a6ecc43b234c1885fc2852d3212d/packages/artifact/src/internal/artifact-client.ts#L157
    public async Task<DownloadArtifactFileFromCurrentWorkflowResult> DownloadArtifactFileAsync(
        GitHubArtifactContainerUrl containerUrl,
        GitHubArtifactContainerName containerName,
        GitHubArtifactItemFilePath itemFilePath)
    {
        containerUrl.NotNull();
        containerName.NotNull();

        var workflowRunArtifactsResult = await ListWorkflowRunArtifactsAsync(containerUrl);
        if (!workflowRunArtifactsResult.IsOk(out var artifactContainers, out var listError))
        {
            return new FailedToListWorkflowRunArtifacts(listError);
        }

        var artifactContainer = artifactContainers.Containers.FirstOrDefault(x => x.Name == containerName);
        if (artifactContainer is null)
        {
            return new ArtifactNotFound(containerName);
        }

        var containerItemsResult = await GetContainerItemsAsync(artifactContainer.FileContainerResourceUrl, artifactContainer.Name);
        if (!containerItemsResult.IsOk(out var artifactContainerItems, out var getContainerItemsError))
        {
            return new FailedToGetContainerItems(getContainerItemsError);
        }

        var artifactContainerFileItem = artifactContainerItems.ContainerItems.FirstOrDefault(x => x.ItemType == "file" && x.Path == itemFilePath);
        if (artifactContainerFileItem is null)
        {
            return new ArtifactContainerItemNotFound(itemFilePath);
        }

        var downloadContainerItemResult = await DownloadContainerItemAsync(artifactContainerFileItem.ContentLocation);
        if (!downloadContainerItemResult.IsOk(out var artifactItemContent, out var downloadContainerItemError))
        {
            return new FailedToDownloadArtifact(downloadContainerItemError);
        }

        return artifactItemContent;
    }

    private async Task<JsonHttpResult<GitHubArtifactContainer>> CreateArtifactContainerAsync(GitHubArtifactContainerUrl containerUrl, GitHubArtifactContainerName containerName)
    {
        using var httpRequest = new HttpRequestMessage(HttpMethod.Post, containerUrl);
        httpRequest.Headers.TryAddWithoutValidation("Accept", $"application/json;api-version={GitHubApiVersion.Latest}");
        var containerRequest = new GitHubCreateArtifactFileContainerRequest(containerName);
        httpRequest.Content = JsonContent.Create(containerRequest);
        var httpResponse = await _httpClient.SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead);
        var jsonHttpResult = await httpResponse.ReadFromJsonAsync<GitHubArtifactContainer, GitHubArtifactContainerValidator>();
        return jsonHttpResult;
    }

    private async Task<JsonHttpResult<GitHubArtifactItem>> UploadArtifactFileAsync(
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
        var jsonHttpResult = await httpResponse.ReadFromJsonAsync<GitHubArtifactItem, GitHubArtifactItemValidator>();
        return jsonHttpResult;
    }

    private async Task<JsonHttpResult<GitHubArtifactContainer>> FinalizeArtifactContainerAsync(
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
        var jsonHttpResult = await httpResponse.ReadFromJsonAsync<GitHubArtifactContainer, GitHubArtifactContainerValidator>();
        return jsonHttpResult;
    }

    private async Task<JsonHttpResult<GitHubArtifactContainers>> ListWorkflowRunArtifactsAsync(GitHubArtifactContainerUrl containerUrl)
    {
        using var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"{containerUrl}");
        httpRequest.Headers.TryAddWithoutValidation("Accept", $"application/json;api-version={GitHubApiVersion.Latest}");
        var httpResponse = await _httpClient.SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead);
        var jsonHttpResult = await httpResponse.ReadFromJsonAsync<GitHubArtifactContainers, GitHubArtifactContainersValidator>();
        return jsonHttpResult;
    }

    private async Task<JsonHttpResult<GitHubArtifactContainerItems>> GetContainerItemsAsync(string fileContainerResourceUrl, string artifactName)
    {
        var getContainerItemsUrl = fileContainerResourceUrl.SetQueryParam("itemPath", artifactName);
        using var httpRequest = new HttpRequestMessage(HttpMethod.Get, getContainerItemsUrl);
        httpRequest.Headers.TryAddWithoutValidation("Accept", $"application/json;api-version={GitHubApiVersion.Latest}");
        var httpResponse = await _httpClient.SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead);
        var jsonHttpResult = await httpResponse.ReadFromJsonAsync<GitHubArtifactContainerItems, GitHubArtifactContainerItemsValidator>();
        return jsonHttpResult;
    }

    private async Task<DownloadContainerItemResult> DownloadContainerItemAsync(string contentLocation)
    {
        using var httpRequest = new HttpRequestMessage(HttpMethod.Get, contentLocation);
        httpRequest.Headers.TryAddWithoutValidation("Accept-Encoding", "gzip"); // TODO: not really needed since I'm not uploading gzip compressed stream on the set-data command ?
        httpRequest.Headers.TryAddWithoutValidation("Accept", $"application/octet-stream;api-version={GitHubApiVersion.Latest}");
        var httpResponse = await _httpClient.SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead);
        var ensureSuccessStatusCodeResult = await httpResponse.EnsureSuccessStatusCodeAsync();
        if (!ensureSuccessStatusCodeResult.IsOk(out var failedStatusCodeHttpResponse))
        {
            return new FailedToDownloadContainerItem(failedStatusCodeHttpResponse);
        }

        var containerItemContent = await httpResponse.Content.ReadAsStringAsync();
        var createArtifactItemJsonContentResult = GitHubArtifactItemJsonContent.Create(containerItemContent);
        if (!createArtifactItemJsonContentResult.IsOk(out var gitHubArtifactItemJsonContent, out var notJsonContent))
        {
            return notJsonContent;
        }

        return gitHubArtifactItemJsonContent;
    }
}
