namespace ShareJobsDataCli.CliCommands.Commands.SetData;

internal sealed record SetDataCommandErrorMessage
    : CommandErrorMessage
{
    private const string _readDataFromDifferentWorkflowErrorMessage = "An error occurred trying to execute the command to set job data.";
    private const string _failToDownloadArtifact = "Failed to upload GitHub artifact.";

    public SetDataCommandErrorMessage(string details)
        : base(_readDataFromDifferentWorkflowErrorMessage, _failToDownloadArtifact, details)
    {
    }
}
