namespace ShareJobsDataCli.CliCommands.Commands.ReadDataCurrentWorkflow;

internal sealed record ReadDataFromCurrentWorkflowCommandErrorMessage
    : CommandErrorMessage
{
    private const string _readDataFromCurrentWorkflowErrorMessage = "An error occurred trying to execute the command to read job data from the current workflow run.";
    private const string _failToDownloadArtifact = "Failed to download GitHub artifact.";

    public ReadDataFromCurrentWorkflowCommandErrorMessage(string details)
        : base(_readDataFromCurrentWorkflowErrorMessage, _failToDownloadArtifact, details)
    {
    }
}
