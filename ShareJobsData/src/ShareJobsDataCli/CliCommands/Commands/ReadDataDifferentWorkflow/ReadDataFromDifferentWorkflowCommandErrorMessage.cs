namespace ShareJobsDataCli.CliCommands.Commands.ReadDataDifferentWorkflow;

internal sealed record ReadDataFromDifferentWorkflowCommandErrorMessage
    : CommandErrorMessage
{
    private const string _readDataFromDifferentWorkflowErrorMessage = "An error occurred trying to execute the command to read job data from a different workflow run.";
    private const string _failToDownloadArtifact = "Failed to download GitHub artifact.";

    public ReadDataFromDifferentWorkflowCommandErrorMessage(string details)
        : base(_readDataFromDifferentWorkflowErrorMessage, _failToDownloadArtifact, details)
    {
    }
}
