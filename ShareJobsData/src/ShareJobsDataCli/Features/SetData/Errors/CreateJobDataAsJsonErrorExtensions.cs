using static ShareJobsDataCli.Common.JobsData.CreateJobDataResult;

namespace ShareJobsDataCli.Features.SetData.Errors;

internal static class CreateJobDataErrorExtensions
{
    private const string _errorMessagePrefix = "Option --data has been provided with an invalid value.";

    public static async Task WriteToConsoleAsync(this OneOf<InvalidYml, CannotConvertYmlToJson> createJobDataError, IConsole console, string command)
    {
        console.NotNull();
        command.NotNullOrWhiteSpace();

        var error = createJobDataError.Match(
            GetInvalidYmlErrorMessage,
            GetCannotConvertYmlToJsonErrorMessage);
        await console.WriteErrorAsync(command, error);
    }

    private static string GetInvalidYmlErrorMessage(InvalidYml invalidYml)
    {
        var sb = new StringBuilder();
        sb.Append("Failed to parse YAML because '").Append(invalidYml.ErrorMessage);
        if (!string.IsNullOrEmpty(invalidYml.Start))
        {
            sb.Append(" Start: ").Append(invalidYml.Start).Append('.');
        }

        if (!string.IsNullOrEmpty(invalidYml.End))
        {
            sb.Append(" End: ").Append(invalidYml.End).Append('.');
        }

        sb.Append('\'');
        var ymlError = sb.ToString();
        return $"{_errorMessagePrefix} {ymlError}";
    }

    private static string GetCannotConvertYmlToJsonErrorMessage(CannotConvertYmlToJson cannotConvertYmlToJson)
    {
        return $"{_errorMessagePrefix} Cannot convert YAML input to JSON because '{cannotConvertYmlToJson.ErrorMessage}'.";
    }
}
