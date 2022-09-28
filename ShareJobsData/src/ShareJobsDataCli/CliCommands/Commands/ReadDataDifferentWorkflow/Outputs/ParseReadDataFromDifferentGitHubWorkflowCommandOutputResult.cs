using static ShareJobsDataCli.CliCommands.Commands.ReadDataDifferentWorkflow.Outputs.ReadDataFromDifferentGitHubWorkflowCommandOutput;

namespace ShareJobsDataCli.CliCommands.Commands.ReadDataDifferentWorkflow.Outputs;

internal abstract record ParseReadDataFromDifferentGitHubWorkflowCommandOutputResult
{
    private ParseReadDataFromDifferentGitHubWorkflowCommandOutputResult()
    {
    }

    public abstract record Ok(ReadDataFromDifferentGitHubWorkflowCommandOutput CommandOutput)
        : ParseReadDataFromDifferentGitHubWorkflowCommandOutputResult;

    public sealed record StrictJson(StrictJsonOutput StrictJsonOutput)
        : Ok(StrictJsonOutput);

    public sealed record GitHubStepJson(GitHubStepJsonOutput GitHubStepJsonOutput)
        : Ok(GitHubStepJsonOutput);

    public sealed record UnknownOutput(string OutputOptionValue)
        : ParseReadDataFromDifferentGitHubWorkflowCommandOutputResult;

    public static implicit operator ParseReadDataFromDifferentGitHubWorkflowCommandOutputResult(StrictJsonOutput strictJsonOutput) => new StrictJson(strictJsonOutput);

    public static implicit operator ParseReadDataFromDifferentGitHubWorkflowCommandOutputResult(GitHubStepJsonOutput gitHubStepJson) => new GitHubStepJson(gitHubStepJson);

    public bool IsOk(
        [NotNullWhen(returnValue: true)] out ReadDataFromDifferentGitHubWorkflowCommandOutput? commandOutput,
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
