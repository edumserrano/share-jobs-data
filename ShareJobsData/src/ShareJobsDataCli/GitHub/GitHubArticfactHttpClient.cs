namespace ShareJobsDataCli.GitHub;

internal class GitHubArticfactHttpClient
{
    private readonly HttpClient _httpClient;

    public GitHubArticfactHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient.NotNull();
    }

    public static HttpClient CreateHttpClient(GitHubActionRuntimeToken actionRuntimeToken, GitHubRepository repository)
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

    // taken from exploring https://github.com/actions/toolkit/blob/90be12a59c20a6ecc43b234c1885fc2852d3212d/packages/artifact/src/internal/artifact-client.ts#L157
    public async Task<GitHubArtifactFileContainerResponse> DownloadArtifactsAsync(GitHubArtifactContainerUrl containerUrl, string artifactName)
    {
        containerUrl.NotNull();

        var listArtifactsResponse = await ListArtifactsAsync(containerUrl);
        for (var i = 0; i < listArtifactsResponse.ArtifactFileContainers.Count; i++)
        {
            var item = listArtifactsResponse.ArtifactFileContainers[i];
            Console.WriteLine($"listArtifactsResponse {i}: {item}");
        }

        var artifact = listArtifactsResponse.ArtifactFileContainers.FirstOrDefault(x => x.Name == artifactName);
        if (artifact is null)
        {
            //abort
            throw new InvalidOperationException();
        }

        var containerItemsResponse = await GetContainerItemsAsync(artifact.FileContainerResourceUrl, artifact.Name);
        for (var i = 0; i < containerItemsResponse.ContainerItems.Count; i++)
        {
            var item = containerItemsResponse.ContainerItems[i];
            Console.WriteLine($"containerItemsResponse {i}: {item}");
        }

        var file = containerItemsResponse.ContainerItems.FirstOrDefault(x => x.ItemType == "file"); //TODO could also check if Path contains artifact name or fully matches artifactName/itemName
        if (file is null)
        {
            //abort
            throw new InvalidOperationException();
        }

        var containerItemContent = await DownloadContainerItemAsync(file.ContentLocation);
        Console.WriteLine(containerItemContent);

        return null!;
    }



    private async Task<GitHubArtifactFileContainerResponse> CreateArtifactFileContainerAsync(GitHubArtifactContainerUrl containerUrl, string containerName)
    {
        using var httpRequest = new HttpRequestMessage(HttpMethod.Post, containerUrl);
        httpRequest.Headers.TryAddWithoutValidation("Accept", $"application/json;api-version={GitHubApiVersion.Latest}");
        var containerRequest = new GitHubCreateArtifactFileContainerRequest(containerName);
        httpRequest.Content = JsonContent.Create(containerRequest);
        var httpResponse = await _httpClient.SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead);
        var responseModel = await httpResponse.ReadFromJsonAsync<GitHubArtifactFileContainerResponse>();
        return responseModel;
    }

    private async Task<GitHubUpdateArtifactResponse> UploadArtifactFileAsync(
        string fileContainerResourceUrl,
        GitHubUploadArtifact artifact)
    {
        var contentBytes = Encoding.UTF8.GetBytes(artifact.FilePayload);
        using var stream = new MemoryStream(contentBytes);
        var uploadFileUrl = fileContainerResourceUrl.SetQueryParam("itemPath", artifact.FilePath);
        using var httpRequest = new HttpRequestMessage(HttpMethod.Put, uploadFileUrl);
        httpRequest.Headers.TryAddWithoutValidation("Accept", $"application/json;api-version={GitHubApiVersion.Latest}");
        httpRequest.Content = new StreamContent(stream);
        httpRequest.Content.Headers.ContentType = new MediaTypeHeaderValue(MediaTypeNames.Application.Octet);
        httpRequest.Content.Headers.ContentRange = new ContentRangeHeaderValue(from: 0, to: contentBytes.Length - 1, length: contentBytes.Length);
        var httpResponse = await _httpClient.SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead);
        var responseModel = await httpResponse.ReadFromJsonAsync<GitHubUpdateArtifactResponse>();
        return responseModel;
    }

    private async Task<GitHubArtifactFileContainerResponse> FinalizeArtifactContainerAsync(
        GitHubArtifactContainerUrl containerUrl,
        GitHubUploadArtifact artifact,
        int containerSize)
    {
        var finalizeArtifactContainerRequest = new GitHubFinalizeArtifactContainerRequest(containerSize);
        var setArtifactSizeUrl = $"{containerUrl}".SetQueryParam("artifactName", artifact.ContainerName);
        using var httpRequest = new HttpRequestMessage(HttpMethod.Patch, setArtifactSizeUrl);
        httpRequest.Headers.TryAddWithoutValidation("Accept", $"application/json;api-version={GitHubApiVersion.Latest}");
        httpRequest.Content = JsonContent.Create(finalizeArtifactContainerRequest);
        var httpResponse = await _httpClient.SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead);
        var responseModel = await httpResponse.ReadFromJsonAsync<GitHubArtifactFileContainerResponse>();
        return responseModel;
    }

    private async Task<GitHubListArtifactsResponse> ListArtifactsAsync(GitHubArtifactContainerUrl containerUrl)
    {
        using var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"{containerUrl}");
        httpRequest.Headers.TryAddWithoutValidation("Accept", $"application/json;api-version={GitHubApiVersion.Latest}");
        var httpResponse = await _httpClient.SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead);
        var responseModel = await httpResponse.ReadFromJsonAsync<GitHubListArtifactsResponse>();
        return responseModel;
    }

    private async Task<GitHubGetContainerItemsResponse> GetContainerItemsAsync(string fileContainerResourceUrl, string artifactName)
    {
        var getContainerItemsUrl = fileContainerResourceUrl.SetQueryParam("itemPath", artifactName);
        using var httpRequest = new HttpRequestMessage(HttpMethod.Get, getContainerItemsUrl);
        httpRequest.Headers.TryAddWithoutValidation("Accept", $"application/json;api-version={GitHubApiVersion.Latest}");
        var httpResponse = await _httpClient.SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead);
        var responseModel = await httpResponse.ReadFromJsonAsync<GitHubGetContainerItemsResponse>();
        return responseModel;
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
