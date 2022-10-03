using static ShareJobsDataCli.Features.ReadDataCurrentWorkflow.Outputs.ParseReadDataFromCurrentGitHubWorkflowCommandOutputResult;

namespace ShareJobsDataCli.Features.ReadDataCurrentWorkflow.Errors;

internal static class ParseCommandOutputErrorExtensions
{
    [DoesNotReturn]
    public static void Throw(this UnknownOutput unknownOutput, string command)
    {
        unknownOutput.NotNull();
        var error = $"Option --output has been provided with an invalid value: '{unknownOutput.OutputOptionValue}'. It must be one of: strict-json, github-step-json.";
        CommandExceptionThrowHelper.Throw(command, error);
    }
}
