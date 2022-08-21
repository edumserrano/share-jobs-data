using ShareJobsDataCli.ArgumentValidations;

namespace ShareJobsDataCli.GitHub.Artifacts.CurrentWorkflowRun;

internal class GitHubCurrentWorkflowRunArticfactHttpClient
{
    private readonly HttpClient _httpClient;

    public GitHubCurrentWorkflowRunArticfactHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient.NotNull();
    }

    public static HttpClient CreateHttpClient(GitHubActionRuntimeToken actionRuntimeToken, GitHubRepositoryName repository)
    {
        actionRuntimeToken.NotNull();
        repository.NotNull();

        var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", actionRuntimeToken);
        httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", $"edumserrano/share-jobs-data:{repository}");
        return httpClient;
    }

    // this process is explained here https://github.com/actions/upload-artifact/issues/180#issuecomment-1086306269
    // add something to dev readme about this
    // no need to gzip since content is always expected to be small json model
    // if I gzip on upload I would have to gzip on download
    public async Task UploadArtifactFileAsync(
        GitHubArtifactContainerUrl containerUrl,
        GitHubArtifactContainerName containerName,
        GitHubArtifactFileUploadRequest fileUploadRequest)
    {
        containerUrl.NotNull();
        containerName.NotNull();
        fileUploadRequest.NotNull();

        var artifactContainer = await CreateArtifactContainerAsync(containerUrl, containerName);
        var artifactItem = await UploadArtifactFileAsync(artifactContainer.FileContainerResourceUrl, fileUploadRequest);
        _ = await FinalizeArtifactContainerAsync(containerUrl, containerName, containerSize: artifactItem.FileLength);
    }

    // taken from exploring https://github.com/actions/toolkit/blob/90be12a59c20a6ecc43b234c1885fc2852d3212d/packages/artifact/src/internal/artifact-client.ts#L157
    public async Task<GitHubArtifactItemContent> DownloadArtifactFileAsync(
        GitHubArtifactContainerUrl containerUrl,
        GitHubArtifactContainerName containerName,
        GitHubArtifactItemFilePath itemFilePath)
    {
        containerUrl.NotNull();
        containerName.NotNull();

        var artifactContainers = await ListWorkflowRunArtifactsAsync(containerUrl);
        var artifactContainer = artifactContainers.Containers.FirstOrDefault(x => x.Name == containerName);
        if (artifactContainer is null)
        {
            throw DownloadArtifactException.ArtifactNotFound(containerName);
        }

        var artifactContainerItems = await GetContainerItemsAsync(artifactContainer.FileContainerResourceUrl, artifactContainer.Name);
        var artifactContainerFileItem = artifactContainerItems.ContainerItems.FirstOrDefault(x => x.ItemType == "file" && x.Path == itemFilePath);
        if (artifactContainerFileItem is null)
        {
            throw DownloadArtifactException.ArtifactFileNotFound(itemFilePath);
        }

        var containerItemContent = await DownloadContainerItemAsync(artifactContainerFileItem.ContentLocation);
        return new GitHubArtifactItemContent(containerItemContent);
    }

    private async Task<GitHubArtifactContainer> CreateArtifactContainerAsync(GitHubArtifactContainerUrl containerUrl, GitHubArtifactContainerName containerName)
    {
        using var httpRequest = new HttpRequestMessage(HttpMethod.Post, containerUrl);
        httpRequest.Headers.TryAddWithoutValidation("Accept", $"application/json;api-version={GitHubApiVersion.Latest}");
        var containerRequest = new GitHubCreateArtifactFileContainerRequest(containerName);
        httpRequest.Content = JsonContent.Create(containerRequest);
        var httpResponse = await _httpClient.SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead);
        var artifactContainer = await httpResponse.ReadFromJsonAsync<GitHubArtifactContainer, GitHubArtifactContainerValidator>();
        return artifactContainer;
    }

    private async Task<GitHubArtifactItem> UploadArtifactFileAsync(
        string fileContainerResourceUrl,
        GitHubArtifactFileUploadRequest fileUploadRequest)
    {
        var contentBytes = Encoding.UTF8.GetBytes(fileUploadRequest.FilePayload);
        using var stream = new MemoryStream(contentBytes);
        var uploadFileUrl = fileContainerResourceUrl.SetQueryParam("itemPath", fileUploadRequest.FilePath);
        using var httpRequest = new HttpRequestMessage(HttpMethod.Put, uploadFileUrl);
        httpRequest.Headers.TryAddWithoutValidation("Accept", $"application/json;api-version={GitHubApiVersion.Latest}");
        httpRequest.Content = new StreamContent(stream);
        httpRequest.Content.Headers.ContentType = new MediaTypeHeaderValue(MediaTypeNames.Application.Octet);
        httpRequest.Content.Headers.ContentRange = new ContentRangeHeaderValue(from: 0, to: contentBytes.Length - 1, length: contentBytes.Length);
        var httpResponse = await _httpClient.SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead);
        var artifactItem = await httpResponse.ReadFromJsonAsync<GitHubArtifactItem, GitHubArtifactItemValidator>();
        return artifactItem;
    }

    private async Task<GitHubArtifactContainer> FinalizeArtifactContainerAsync(
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
        var artifactContainer = await httpResponse.ReadFromJsonAsync<GitHubArtifactContainer, GitHubArtifactContainerValidator>();
        return artifactContainer;
    }

    private async Task<GitHubArtifactContainers> ListWorkflowRunArtifactsAsync(GitHubArtifactContainerUrl containerUrl)
    {
        using var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"{containerUrl}");
        httpRequest.Headers.TryAddWithoutValidation("Accept", $"application/json;api-version={GitHubApiVersion.Latest}");
        var httpResponse = await _httpClient.SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead);
        var artifactContainers = await httpResponse.ReadFromJsonAsync<GitHubArtifactContainers, GitHubArtifactContainersValidator>();
        return artifactContainers;
    }

    private async Task<GitHubArtifactContainerItems> GetContainerItemsAsync(string fileContainerResourceUrl, string artifactName)
    {
        var getContainerItemsUrl = fileContainerResourceUrl.SetQueryParam("itemPath", artifactName);
        using var httpRequest = new HttpRequestMessage(HttpMethod.Get, getContainerItemsUrl);
        httpRequest.Headers.TryAddWithoutValidation("Accept", $"application/json;api-version={GitHubApiVersion.Latest}");
        var httpResponse = await _httpClient.SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead);
        var artifactContainerItems = await httpResponse.ReadFromJsonAsync<GitHubArtifactContainerItems, GitHubArtifactContainerItemsValidator>();
        return artifactContainerItems;
    }

    private async Task<string> DownloadContainerItemAsync(string contentLocation)
    {
        using var httpRequest = new HttpRequestMessage(HttpMethod.Get, contentLocation);
        httpRequest.Headers.TryAddWithoutValidation("Accept-Encoding", "gzip"); // not really needed since I'm not uploading gzip compressed stream ?
        httpRequest.Headers.TryAddWithoutValidation("Accept", $"application/octet-stream;api-version={GitHubApiVersion.Latest}");
        var httpResponse = await _httpClient.SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead);
        await httpResponse.EnsureSuccessStatusCodeAsync();
        var containerItemContent = await httpResponse.Content.ReadAsStringAsync();
        return containerItemContent;
    }
}
