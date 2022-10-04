using static ShareJobsDataCli.Features.ReadDataCurrentWorkflow.DownloadArtifact.Results.DownloadArtifactFileFromCurrentWorkflowResult;
using static ShareJobsDataCli.Features.ReadDataCurrentWorkflow.DownloadArtifact.Results.DownloadContainerItemResult;

namespace ShareJobsDataCli.Features.ReadDataCurrentWorkflow.DownloadArtifact;

internal sealed class GitHubDownloadArtifactFromCurrentWorkflowHttpClient
{
    private readonly HttpClient _httpClient;

    public GitHubDownloadArtifactFromCurrentWorkflowHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient.NotNull();
    }

    // The GitHub's APIs used to download an artifact on the current workflow are internal APIs
    // which are subject to breaking changes.
    //
    // See: https://github.com/actions/upload-artifact/issues/180#issuecomment-1086306269
    // "They're effectively "internal" APIs that don't hit api.github.com but some of our
    // backend services. Anyone can hit them but we're deliberately not advertising this
    // and these APIs are not documented on https://docs.github.com/en/rest/reference/actions#artifacts"
    //
    // The usage of these internal APIs was reverse engineered by exploring the code flow from:
    // https://github.com/actions/toolkit/blob/90be12a59c20a6ecc43b234c1885fc2852d3212d/packages/artifact/src/internal/artifact-client.ts#L157
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
        // the this Accept-Encoding header below was set on the GitHub toolkit repo were I reverse engineered
        // the API calls but I probably don't need it since I'm not uploading gzip compressed content on
        // the set-data command
        httpRequest.Headers.TryAddWithoutValidation("Accept-Encoding", "gzip");
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
