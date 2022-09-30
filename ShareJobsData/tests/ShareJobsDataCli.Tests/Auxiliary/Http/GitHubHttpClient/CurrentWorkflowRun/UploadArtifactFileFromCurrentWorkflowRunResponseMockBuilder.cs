namespace ShareJobsDataCli.Tests.Auxiliary.Http.GitHubHttpClient.CurrentWorkflowRun;

internal sealed class UploadArtifactFileFromCurrentWorkflowRunResponseMockBuilder : BaseResponseMockBuilder<UploadArtifactFileFromCurrentWorkflowRunResponseMockBuilder>
{
    private string? _fileContainerResourceUrl;
    private string? _artifactName;
    private string? _artifactFilename;

    public UploadArtifactFileFromCurrentWorkflowRunResponseMockBuilder FromFileContainerResourceUrl(
        string fileContainerResourceUrl,
        string artifactName,
        string artifactFilename)
    {
        _fileContainerResourceUrl = fileContainerResourceUrl;
        _artifactName = artifactName;
        _artifactFilename = artifactFilename;
        return this;
    }

    protected override string OperationName { get; } = "upload artifact file from current workflow run";

    protected override string GetRequestUrl() => $"{_fileContainerResourceUrl}?itemPath={_artifactName}%2F{_artifactFilename}"; // %2F is encoding for /
}
