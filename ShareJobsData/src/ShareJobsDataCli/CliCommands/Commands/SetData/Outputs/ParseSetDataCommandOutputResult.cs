using static ShareJobsDataCli.CliCommands.Commands.SetData.Outputs.SetDataCommandOutput;

namespace ShareJobsDataCli.CliCommands.Commands.SetData.Outputs;

internal abstract record ParseSetDataCommandOutputResult
{
    private ParseSetDataCommandOutputResult()
    {
    }

    public abstract record Ok(SetDataCommandOutput SetDataCommandOutput)
        : ParseSetDataCommandOutputResult;

    public sealed record StrictJson(StrictJsonOutput StrictJsonOutput)
        : Ok(StrictJsonOutput);

    public sealed record GitHubStepJson(GitHubStepJsonOutput GitHubStepJsonOutput)
        : Ok(GitHubStepJsonOutput);

    public sealed record None(NoOutput NoOutput)
        : Ok(NoOutput);

    public sealed record UnknownOutput(string OutputOptionValue)
        : ParseSetDataCommandOutputResult;

    public static implicit operator ParseSetDataCommandOutputResult(StrictJsonOutput strictJsonOutput) => new StrictJson(strictJsonOutput);

    public static implicit operator ParseSetDataCommandOutputResult(GitHubStepJsonOutput gitHubStepJson) => new GitHubStepJson(gitHubStepJson);

    public static implicit operator ParseSetDataCommandOutputResult(NoOutput noOutput) => new None(noOutput);

    public bool IsOk(
        [NotNullWhen(returnValue: true)] out SetDataCommandOutput? setDataCommandOutput,
        [NotNullWhen(returnValue: false)] out UnknownOutput? parseCommandOutputError)
    {
        setDataCommandOutput = null;
        parseCommandOutputError = null;

        if (this is Ok ok)
        {
            setDataCommandOutput = ok.SetDataCommandOutput;
            return true;
        }

        parseCommandOutputError = (UnknownOutput)this;
        return false;
    }
}
