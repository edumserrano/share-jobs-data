using System.IO.Compression;

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
        httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", $"edumserrano/share-jobs-data:{repository}");
        return httpClient;
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

        Console.WriteLine($"download from {file.ContentLocation}");
        await DownloadContainerItemAsync(file.ContentLocation);

        return null!;
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

    private async Task DownloadContainerItemAsync(string contentLocation)
    {
        using var httpRequest = new HttpRequestMessage(HttpMethod.Get, contentLocation);
        httpRequest.Headers.TryAddWithoutValidation("Accept-Encoding", "gzip");
        httpRequest.Headers.TryAddWithoutValidation("Accept", $"application/octet-stream;api-version={GitHubApiVersion.Latest}");
        var httpResponse = await _httpClient.SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead);
        await httpResponse.EnsureSuccessStatusCodeAsync();

        //using var memStream = new MemoryStream();
        using var responseStream = await httpResponse.Content.ReadAsStreamAsync();
        //using (var decompressionStream = new GZipStream(responseStream, CompressionMode.Decompress))
        //{
        //    await decompressionStream.CopyToAsync(memStream);
        //    Console.WriteLine($"memStream position after gzip copy: {memStream.Position}");
        //    memStream.Position = 0;
        //}

        using var reader = new StreamReader(responseStream);
        var text = await reader.ReadToEndAsync();
        Console.WriteLine(text);
    }

    // see https://docs.github.com/en/rest/actions/artifacts#download-an-artifact
    //public async Task<ZipArchive> DownloadArtifactAsync(GitHubRepository repo, GitHubArtifactId artifactId)
    //{
    //    repo.NotNull();
    //    artifactId.NotNull();

    //    using var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"repos/{repo}/actions/artifacts/{artifactId}/zip");
    //    var httpResponse = await _httpClient.SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead);
    //    if (!httpResponse.IsSuccessStatusCode)
    //    {
    //        var errorResponseBody = await httpResponse.Content.ReadAsStringAsync();
    //        throw new HttpClientResponseException(httpRequest.Method, $"{httpRequest.RequestUri}", httpResponse.StatusCode, errorResponseBody);
    //    }

    //    var responseStream = await httpResponse.Content.ReadAsStreamAsync();
    //    return new ZipArchive(responseStream, ZipArchiveMode.Read);
    //}
}
