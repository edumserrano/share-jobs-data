namespace ShareJobsDataCli.Validations;

public sealed class PropertValidationException : Exception
{
    internal PropertValidationException(string modelName, string validationErrorMessage)
        : base($"Error validating {modelName}: {validationErrorMessage}")
    {
    }
}
