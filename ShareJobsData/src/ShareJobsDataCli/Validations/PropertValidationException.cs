namespace ShareJobsDataCli.Validations;

public class PropertValidationException : Exception
{
    internal PropertValidationException(string modelName, string validationErrorMessage)
        : base($"Error validating {modelName}: {validationErrorMessage}")
    {
    }
}
