using static ShareJobsDataCli.GitHub.Artifacts.CurrentWorkflowRun.DownloadArtifactFile.Results.DownloadArtifactFileFromCurrentWorkflowResult;

namespace ShareJobsDataCli.CliCommands.Commands.ReadDataCurrentWorkflow;

internal static class CommandExceptions
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

        var error = $"Failed to download artifact. Couldn't find artifact '{artifactNotFound.ArtifactContainerName}' in current workflow.";
        var message = CreateReadJobDataErrorMessage(error);
        return new CommandException(message);
    }

    public static CommandException ToCommandException(this ArtifactFileNotFound artifactFileNotFound)
    {
        artifactFileNotFound.NotNull();

        var error = $"Failed to download artifact. Couldn't find artifact file '{artifactFileNotFound.ArtifactItemFilePath}' in current workflow.";
        var message = CreateReadJobDataErrorMessage(error);
        return new CommandException(message);
    }
}
