using static ShareJobsDataCli.JobsData.CreateJobDataResult;

namespace ShareJobsDataCli.CliCommands.Commands.SetData.Errors;

internal static class CreateJobDataErrorExtensions
{
    private const string _errorMessagePrefix = "Option --data has been provided with an invalid value.";

    public static async Task WriteToConsoleAsync(this Error createJobDataError, IConsole console, string command)
    {
        createJobDataError.NotNull();
        console.NotNull();
        command.NotNullOrWhiteSpace();

        var error = createJobDataError switch
        {
            InvalidYml invalidYml => GetErrorMessage(invalidYml),
            CannotConvertYmlToJson cannotConvertYmlToJson => GetErrorMessage(cannotConvertYmlToJson),
            _ => throw UnexpectedTypeException.Create(createJobDataError),
        };
        await console.WriteErrorAsync(command, error);
    }

    private static string GetErrorMessage(InvalidYml invalidYml)
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

    private static string GetErrorMessage(CannotConvertYmlToJson cannotConvertYmlToJson)
    {
        return $"{_errorMessagePrefix} Cannot convert YAML input to JSON because '{cannotConvertYmlToJson.ErrorMessage}'.";
    }
}
