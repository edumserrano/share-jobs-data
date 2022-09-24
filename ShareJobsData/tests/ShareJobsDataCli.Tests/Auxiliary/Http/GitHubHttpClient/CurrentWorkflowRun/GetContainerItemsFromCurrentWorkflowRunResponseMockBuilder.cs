namespace ShareJobsDataCli.Tests.Auxiliary.Http.GitHubHttpClient.CurrentWorkflowRun;

internal class GetContainerItemsFromCurrentWorkflowRunResponseMockBuilder : BaseResponseMockBuilder<GetContainerItemsFromCurrentWorkflowRunResponseMockBuilder>
{
    private string? _fileContainerResourceUrl;
    private string? _artifactName;

    public GetContainerItemsFromCurrentWorkflowRunResponseMockBuilder FromFileContainerResourceUrl(string fileContainerResourceUrl, string artifactName)
    {
        _fileContainerResourceUrl = fileContainerResourceUrl;
        _artifactName = artifactName;
        return this;
    }

    protected override string OperationName { get; } = "get container items from current workflow run";

    protected override string? GetRequestUrl() => $"{_fileContainerResourceUrl}?itemPath={_artifactName}";
}
