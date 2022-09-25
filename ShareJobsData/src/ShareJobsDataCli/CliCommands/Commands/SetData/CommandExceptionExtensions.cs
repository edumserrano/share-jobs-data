namespace ShareJobsDataCli.CliCommands.Commands.SetData;

internal static class CommandExceptionExtensions
{
    public static CommandException ToCommandException(this UploadArtifactFileResult.Error error)
    {
        error.NotNull();

        var details = error switch
        {
            UploadArtifactFileResult.FailedToCreateArtifactContainer failedToCreateArtifactContainer => failedToCreateArtifactContainer.JsonHttpError.ToErrorDetails("creating an artifact container"),
            UploadArtifactFileResult.FailedToUploadArtifact failedToUploadArtifact => failedToUploadArtifact.JsonHttpError.ToErrorDetails("uploading an artifact"),
            UploadArtifactFileResult.FailedToFinalizeArtifactContainer failedToFinalizeArtifactContainer => failedToFinalizeArtifactContainer.JsonHttpError.ToErrorDetails("finalizing artifact container"),
            _ => throw UnexpectedTypeException.Create(error),
        };
        var exceptionMessage = new SetDataCommandExceptionMessage(details);
        return exceptionMessage.ToCommandException();
    }

    public static CommandException ToCommandException(this CreateJobDataAsJsonResult.Error error)
    {
        error.NotNull();

        var details = error switch
        {
            CreateJobDataAsJsonResult.InvalidYml invalidYml => FormatYmlError(invalidYml),
            CreateJobDataAsJsonResult.CannotConvertYmlToJson cannotConvertYmlToJson => GetDataOptionErrorMessage($"Cannot convert YAML input to JSON: {cannotConvertYmlToJson.ErrorMessage}."),
            _ => throw UnexpectedTypeException.Create(error),
        };
        var exceptionMessage = new SetDataCommandExceptionMessage(details);
        return exceptionMessage.ToCommandException();

        string GetDataOptionErrorMessage(string error)
        {
            return @$"Option --data has been provided with an invalid value.
Error(s):
- {error}
";
        }

        string FormatYmlError(CreateJobDataAsJsonResult.InvalidYml invalidYml)
        {
            var sb = new StringBuilder();
            sb.Append("Failed to parse YAML: '").Append(invalidYml.ErrorMessage).Append("'.");
            if (!string.IsNullOrEmpty(invalidYml.Start))
            {
                sb.Append("Start: '").Append(invalidYml.Start).Append("'.");
            }

            if (!string.IsNullOrEmpty(invalidYml.End))
            {
                sb.Append("End: '").Append(invalidYml.End).Append("'.");
            }

            var ymlError = sb.ToString();
            return GetDataOptionErrorMessage(ymlError);
        }
    }
}
