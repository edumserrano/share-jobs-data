using System.Net.Http.Json;
using System.Net.Mime;
using System.Text;
using Flurl;

namespace ShareJobsDataCli.GitHub;

internal class GitHubHttpClient
{
    //private readonly HttpClient _httpClient;

    //public GitHubHttpClient()
    //{
    //    _httpClient = new HttpClient();
    //}

    //public GitHubHttpClient(HttpClient httpClient)
    //{
    //    _httpClient = httpClient.NotNull();
    //}

    //public static HttpClient Create(GitHubAuthToken authToken)
    //{
    //    authToken.NotNull();
    //    var httpClient = new HttpClient
    //    {
    //        //BaseAddress = new Uri("https://api.github.com"),
    //    };
    //    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("token", authToken);
    //    httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "application/vnd.github.v3+json");
    //    httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "share-jobs-data-cli");
    //    return httpClient;
    //}

    public async Task UploadArtifactAsync(string json)
    {
        var httpClient = new HttpClient();


        // `${getRuntimeUrl()}_apis/pipelines/workflows/${getWorkFlowRunId()}/artifacts?api-version=${getApiVersion()}`
        var runtimeUrl = Environment.GetEnvironmentVariable("ACTIONS_RUNTIME_URL");
        var workflowRunId = Environment.GetEnvironmentVariable("GITHUB_RUN_ID");
        var actionRuntimeToken = Environment.GetEnvironmentVariable("ACTIONS_RUNTIME_TOKEN");
        var apiVersion = "6.0-preview";
        var artifactName = "my-dotnet-artifact-name";
        var createContainerUrl = $"{runtimeUrl}_apis/pipelines/workflows/{workflowRunId}/artifacts?api-version={apiVersion}";
        var createArtifactFileContainerRequest = new
        {
            Type = "actions_storage",
            Name = artifactName
        };

        Console.WriteLine($"createArtifactFileContainerUrl: {createContainerUrl}");
        using var createArtifactFileContainerHttpRequest = new HttpRequestMessage(HttpMethod.Post, createContainerUrl);
        createArtifactFileContainerHttpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", actionRuntimeToken);
        createArtifactFileContainerHttpRequest.Headers.TryAddWithoutValidation("Accept", $"application/json;api-version={apiVersion}");
        createArtifactFileContainerHttpRequest.Content = JsonContent.Create(createArtifactFileContainerRequest);
        var createArtifactFileContainerHttpResponse = await httpClient.SendAsync(createArtifactFileContainerHttpRequest);
        var createArtifactFileContainerResponse = await createArtifactFileContainerHttpResponse.Content.ReadFromJsonAsync<CreateArtifactFileContainerResponse>();

        Console.WriteLine($"create-container-response: {createArtifactFileContainerHttpResponse.StatusCode}");
        Console.WriteLine($"Name: {createArtifactFileContainerResponse!.Name}");
        Console.WriteLine($"Url: {createArtifactFileContainerResponse.Url}");
        Console.WriteLine($"FileContainerResourceUrl: {createArtifactFileContainerResponse.FileContainerResourceUrl}");

        var content = "Hello!";
        var contentBytes = Encoding.UTF8.GetBytes(content);
        /*using*/
        var stream = new MemoryStream(contentBytes);
        var itemPath = $"{artifactName}/name-of-file.txt";
        var uploadFileUrl = createArtifactFileContainerResponse.FileContainerResourceUrl.SetQueryParam("itemPath", itemPath);
        Console.WriteLine($"uploadFileUrl: {uploadFileUrl}");
        using var uploadFileHttpRequest = new HttpRequestMessage(HttpMethod.Put, uploadFileUrl);
        uploadFileHttpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", actionRuntimeToken);
        uploadFileHttpRequest.Headers.TryAddWithoutValidation("Accept", $"application/json;api-version={apiVersion}");
        uploadFileHttpRequest.Content = new StreamContent(stream);
        uploadFileHttpRequest.Content.Headers.ContentType = new MediaTypeHeaderValue(MediaTypeNames.Application.Octet);
        uploadFileHttpRequest.Content.Headers.ContentRange = new ContentRangeHeaderValue(from: 0, to: contentBytes.Length - 1, length: contentBytes.Length);

        var uploadFileHttpResponse = await httpClient.SendAsync(uploadFileHttpRequest);
        var uploadFileResponse = await uploadFileHttpResponse.Content.ReadAsStringAsync();
        Console.WriteLine($"uploadFileResponse-status-code: {uploadFileHttpResponse.StatusCode}");
        Console.WriteLine($"uploadFileResponse: {uploadFileResponse}");

        var setArtifactSizeRequest = new
        {
            Size = contentBytes.Length,
        };
        var setArtifactSizeUrl = createContainerUrl.SetQueryParam("artifactName", artifactName);
        Console.WriteLine($"setArtifactSizeUrl: {setArtifactSizeUrl}");
        using var setArtifactSizeHttpRequest = new HttpRequestMessage(HttpMethod.Patch, setArtifactSizeUrl);
        setArtifactSizeHttpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", actionRuntimeToken);
        setArtifactSizeHttpRequest.Headers.TryAddWithoutValidation("Accept", $"application/json;api-version={apiVersion}");
        setArtifactSizeHttpRequest.Content = JsonContent.Create(setArtifactSizeRequest);
        var setArtifactSizeHttpResponse = await httpClient.SendAsync(setArtifactSizeHttpRequest);
        var setArtifactSizeResponse = await setArtifactSizeHttpResponse.Content.ReadAsStringAsync();
        Console.WriteLine($"setArtifactSizeHttpResponse-status-code: {setArtifactSizeHttpResponse.StatusCode}");
        Console.WriteLine($"setArtifactSizeResponse: {uploadFileResponse}");



        //createContainerHttpRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue($"application/json;api-version=${apiVersion}"));
        //createContainerHttpRequest.Content = new StringContent(jsonBody, Encoding.UTF8, MediaTypeNames.Application.Json);
        //var response = await httpClient.SendAsync(createContainerHttpRequest);
        //var responseBody = await response.Content.ReadAsStringAsync();
        //Console.WriteLine($"create-container-response: {response.StatusCode}");
        //Console.WriteLine($"create-container-response-body: {responseBody}");

        //        const headers = getUploadHeaders(
        //  'application/octet-stream',
        //  true,
        //  isGzip,
        //  totalFileSize,
        //  end - start + 1,
        //  getContentRange(start, end, uploadFileSize)
        //)





    }

    // see https://docs.github.com/en/rest/actions/workflow-runs#download-workflow-run-logs
    //public async Task<ZipArchive> DownloadWorkflowRunLogsAsync(GitHubRepository repo, GitHubRunId runId)
    //{
    //    repo.NotNull();
    //    runId.NotNull();

    //    using var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"repos/{repo}/actions/runs/{runId}/logs");
    //    var httpResponse = await _httpClient.SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead);
    //    if (!httpResponse.IsSuccessStatusCode)
    //    {
    //        throw new GitHubHttpClientException(httpResponse.StatusCode, httpRequest.Method, httpRequest.RequestUri);
    //    }

    //    var responseStream = await httpResponse.Content.ReadAsStreamAsync();
    //    return new ZipArchive(responseStream, ZipArchiveMode.Read);
    //}
}
