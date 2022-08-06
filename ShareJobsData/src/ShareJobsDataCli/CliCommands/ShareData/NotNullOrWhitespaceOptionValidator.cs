namespace ShareJobsDataCli.CliCommands.ShareData;

#pragma warning disable CA1812 // Avoid uninstantiated internal classes. Referenced via typeof(NotNullOrWhitespaceOptionValidator) usage
internal class NotNullOrWhitespaceOptionValidator : BindingValidator<string>
{
    public override BindingValidationError? Validate(string? value)
    {
        return string.IsNullOrWhiteSpace(value)
            ? new BindingValidationError("Cannot be null or whitespace.")
            : null;
    }
}
#pragma warning restore CA1812 // Avoid uninstantiated internal classes. Referenced via typeof(NotNullOrWhitespaceOptionValidator) usage
