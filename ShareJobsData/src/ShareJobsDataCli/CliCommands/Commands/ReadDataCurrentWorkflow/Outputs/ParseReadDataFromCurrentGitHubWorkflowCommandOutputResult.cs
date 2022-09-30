using static ShareJobsDataCli.CliCommands.Commands.ReadDataCurrentWorkflow.Outputs.ReadDataFromCurrentGitHubWorkflowCommandOutput;

namespace ShareJobsDataCli.CliCommands.Commands.ReadDataCurrentWorkflow.Outputs;

internal abstract record ParseReadDataFromCurrentGitHubWorkflowCommandOutputResult
{
    private ParseReadDataFromCurrentGitHubWorkflowCommandOutputResult()
    {
    }

    public abstract record Ok(ReadDataFromCurrentGitHubWorkflowCommandOutput CommandOutput)
        : ParseReadDataFromCurrentGitHubWorkflowCommandOutputResult;

    public sealed record StrictJson(StrictJsonOutput StrictJsonOutput)
        : Ok(StrictJsonOutput);

    public sealed record GitHubStepJson(GitHubStepJsonOutput GitHubStepJsonOutput)
        : Ok(GitHubStepJsonOutput);

    public sealed record UnknownOutput(string OutputOptionValue)
        : ParseReadDataFromCurrentGitHubWorkflowCommandOutputResult;

    public static implicit operator ParseReadDataFromCurrentGitHubWorkflowCommandOutputResult(StrictJsonOutput strictJsonOutput) => new StrictJson(strictJsonOutput);

    public static implicit operator ParseReadDataFromCurrentGitHubWorkflowCommandOutputResult(GitHubStepJsonOutput gitHubStepJson) => new GitHubStepJson(gitHubStepJson);

    public bool IsOk(
        [NotNullWhen(returnValue: true)] out ReadDataFromCurrentGitHubWorkflowCommandOutput? commandOutput,
        [NotNullWhen(returnValue: false)] out UnknownOutput? parseCommandOutputError)
    {
        commandOutput = null;
        parseCommandOutputError = null;

        if (this is Ok ok)
        {
            commandOutput = ok.CommandOutput;
            return true;
        }

        parseCommandOutputError = (UnknownOutput)this;
        return false;
    }
}
