using System.Net.Mime;
using System.Text;

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
        var createContainerUrl = $"{runtimeUrl}_apis/pipelines/workflows/{workflowRunId}/artifacts?api-version={apiVersion}";
        var body = new
        {
            Type = "actions_storage",
            Name = "my-artifact-name",
        };
        var jsonBody = System.Text.Json.JsonSerializer.Serialize(body);

        Console.WriteLine();
        Console.WriteLine($"create-container-request-url: {createContainerUrl}");
        Console.WriteLine($"create-container-request-auth: {authToken}");
        using var createContainerHttpRequest = new HttpRequestMessage(HttpMethod.Post, createContainerUrl);
        createContainerHttpRequest.Headers.TryAddWithoutValidation("Accept", $"application/json;api-version={apiVersion}");
        createContainerHttpRequest.Headers.Authorization = new AuthenticationHeaderValue("token", actionRuntimeToken);
        createContainerHttpRequest.Content = new StringContent(jsonBody, Encoding.UTF8, MediaTypeNames.Application.Json);
        var response = await httpClient.SendAsync(createContainerHttpRequest);
        var responseBody = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"create-container-response: {response.StatusCode}");
        Console.WriteLine($"create-container-response-body: {responseBody}");

        //using var uploadFileHttpRequest = new HttpRequestMessage(HttpMethod.Put, createContainerUrl);
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
