using static ShareJobsDataCli.GitHub.Artifacts.DifferentWorkflowRun.DownloadArtifactFile.Results.DownloadArtifactFileFromDifferentWorkflowResult;

namespace ShareJobsDataCli.CliCommands.Commands.ReadDataDifferentWorkflow;

internal static class CommandExceptionExtensions
{
    private static string CreateReadJobDataErrorMessage(string error)
    {
        return @$"An error occurred trying to execute the command to read job data from a different workflow run.
Error:
- {error}";
    }

    public static CommandException ToCommandException(this ArtifactNotFound artifactNotFound)
    {
        artifactNotFound.NotNull();

        var error = $"Failed to download artifact. Couldn't find artifact '{artifactNotFound.ArtifactContainerName}' in workflow run id {artifactNotFound.WorkflowRunId} at repo {artifactNotFound.RepoName}.";
        var message = CreateReadJobDataErrorMessage(error);
        return new CommandException(message);
    }

    public static CommandException ToCommandException(this ArtifactFileNotFound artifactFileNotFound)
    {
        artifactFileNotFound.NotNull();

        var error = $"Failed to download artifact. Couldn't find artifact file '{artifactFileNotFound.ArtifactContainerName}/{artifactFileNotFound.ArtifactItemFilename}' in workflow run id {artifactFileNotFound.WorkflowRunId} at repo {artifactFileNotFound.RepoName}.";
        var message = CreateReadJobDataErrorMessage(error);
        return new CommandException(message);
    }
}
