using static ShareJobsDataCli.CliCommands.Commands.ReadDataCurrentWorkflow.Outputs.ParseReadDataFromCurrentGitHubWorkflowCommandOutputResult;

namespace ShareJobsDataCli.CliCommands.Commands.ReadDataCurrentWorkflow.Outputs;

internal abstract class ReadDataFromCurrentGitHubWorkflowCommandOutput
{
    private ReadDataFromCurrentGitHubWorkflowCommandOutput()
    {
    }

    public static ParseReadDataFromCurrentGitHubWorkflowCommandOutputResult FromOption(IConsole console, string outputOptionValue)
    {
        return outputOptionValue switch
        {
            var _ when string.Equals(outputOptionValue, "strict-json", StringComparison.OrdinalIgnoreCase) => new StrictJsonOutput(console),
            var _ when string.Equals(outputOptionValue, "github-step-json", StringComparison.OrdinalIgnoreCase) => new GitHubStepJsonOutput(console),
            _ => new UnknownOutput(outputOptionValue),
        };
    }

    public abstract Task WriteToConsoleAsync(JobData jobData);

    public sealed class StrictJsonOutput : ReadDataFromCurrentGitHubWorkflowCommandOutput
    {
        private readonly GitHubActionStrictJsonStepOutput _strictJsonOutput;

        public StrictJsonOutput(IConsole console)
        {
            _strictJsonOutput = new GitHubActionStrictJsonStepOutput(console);
        }

        public override Task WriteToConsoleAsync(JobData jobData)
        {
            var json = jobData.AsJson();
            return _strictJsonOutput.WriteToConsoleAsync(json);
        }
    }

    public sealed class GitHubStepJsonOutput : ReadDataFromCurrentGitHubWorkflowCommandOutput
    {
        private readonly GitHubActionStepJsonStepOutput _stepJsonOutput;

        public GitHubStepJsonOutput(IConsole console)
        {
            _stepJsonOutput = new GitHubActionStepJsonStepOutput(console);
        }

        public override Task WriteToConsoleAsync(JobData jobData)
        {
            var keysAndValues = jobData.AsKeyValues();
            return _stepJsonOutput.WriteToConsoleAsync(keysAndValues);
        }
    }
}
