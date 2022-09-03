namespace ShareJobsDataCli.CliCommands.Commands.SetData;

internal sealed record SetDataCommandExceptionMessage
    : CommandExceptionMessage
{
    private const string _readDataFromDifferentWorkflowErrorMessage = "An error occurred trying to execute the command to set job data.";
    private const string _failToDownloadArtifact = "Failed to upload GitHub artifact.";

    public SetDataCommandExceptionMessage(string details)
        : base(_readDataFromDifferentWorkflowErrorMessage, _failToDownloadArtifact, details)
    {
    }
}
