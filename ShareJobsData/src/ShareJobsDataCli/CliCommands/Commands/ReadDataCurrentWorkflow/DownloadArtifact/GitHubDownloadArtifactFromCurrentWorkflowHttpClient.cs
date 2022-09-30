using static ShareJobsDataCli.CliCommands.Commands.ReadDataCurrentWorkflow.DownloadArtifact.Results.DownloadArtifactFileFromCurrentWorkflowResult;
using static ShareJobsDataCli.CliCommands.Commands.ReadDataCurrentWorkflow.DownloadArtifact.Results.DownloadContainerItemResult;

namespace ShareJobsDataCli.CliCommands.Commands.ReadDataCurrentWorkflow.DownloadArtifact;

internal sealed class GitHubDownloadArtifactFromCurrentWorkflowHttpClient
{
    private readonly HttpClient _httpClient;

    public GitHubDownloadArtifactFromCurrentWorkflowHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient.NotNull();
    }

    // taken from exploring https://github.com/actions/toolkit/blob/90be12a59c20a6ecc43b234c1885fc2852d3212d/packages/artifact/src/internal/artifact-client.ts#L157
    public async Task<DownloadArtifactFileFromCurrentWorkflowResult> DownloadArtifactFileAsync(
        GitHubArtifactContainerUrl containerUrl,
        GitHubArtifactContainerName containerName,
        GitHubArtifactItemFilePath itemFilePath)
    {
        containerUrl.NotNull();
        containerName.NotNull();
        itemFilePath.NotNull();

        var workflowRunArtifactsResult = await ListWorkflowRunArtifactsAsync(containerUrl);
        if (!workflowRunArtifactsResult.IsOk(out var artifactContainers, out var listError))
        {
            return new FailedToListWorkflowRunArtifacts(listError);
        }

        var artifactContainer = artifactContainers.Containers.FirstOrDefault(x => string.Equals(x.Name, containerName, StringComparison.Ordinal));
        if (artifactContainer is null)
        {
            return new ArtifactNotFound(containerName);
        }

        var containerItemsResult = await GetContainerItemsAsync(artifactContainer.FileContainerResourceUrl, artifactContainer.Name);
        if (!containerItemsResult.IsOk(out var artifactContainerItems, out var getContainerItemsError))
        {
            return new FailedToGetContainerItems(getContainerItemsError);
        }

        var artifactContainerFileItem = artifactContainerItems.ContainerItems.FirstOrDefault(x => string.Equals(x.ItemType, "file", StringComparison.Ordinal) && string.Equals(x.Path, itemFilePath, StringComparison.Ordinal));
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

    private async Task<JsonHttpResult<GitHubListArtifactsHttpResponse>> ListWorkflowRunArtifactsAsync(GitHubArtifactContainerUrl containerUrl)
    {
        using var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"{containerUrl}");
        httpRequest.Headers.TryAddWithoutValidation("Accept", $"application/json;api-version={GitHubApiVersion.Latest}");
        var httpResponse = await _httpClient.SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead);
        var jsonHttpResult = await httpResponse.ReadFromJsonAsync<GitHubListArtifactsHttpResponse, GitHubListArtifactsHttpResponseValidator>();
        return jsonHttpResult;
    }

    private async Task<JsonHttpResult<GitHubGetContainerItemsHttpResponse>> GetContainerItemsAsync(string fileContainerResourceUrl, string artifactName)
    {
        var getContainerItemsUrl = fileContainerResourceUrl.SetQueryParam("itemPath", artifactName);
        using var httpRequest = new HttpRequestMessage(HttpMethod.Get, getContainerItemsUrl);
        httpRequest.Headers.TryAddWithoutValidation("Accept", $"application/json;api-version={GitHubApiVersion.Latest}");
        var httpResponse = await _httpClient.SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead);
        var jsonHttpResult = await httpResponse.ReadFromJsonAsync<GitHubGetContainerItemsHttpResponse, GitHubGetContainerItemsHttpResponseValidator>();
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
            return new ContainerItemContentNotJson(notJsonContent);
        }

        return gitHubArtifactItemJsonContent;
    }
}
