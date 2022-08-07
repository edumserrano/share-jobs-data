namespace ShareJobsDataCli.GitHub;

internal static class HttpResponseMessageExtensions
{
    public static async Task<T> ReadFromJsonAndValidateAsync<T, TValidator>(this HttpResponseMessage httpResponse)
        where TValidator : AbstractValidator<T>, new()
    {
        await httpResponse.EnsureSuccessStatusCodeAsync();

        var responseModel = await httpResponse.Content.ReadFromJsonAsync<T>();
        if (responseModel is null)
        {
            throw HttpResponseValidationException.JsonDeserializedToNull<T>();
        }

        var validator = new TValidator();
        var validationResult = validator.Validate(responseModel);
        if (!validationResult.IsValid)
        {
            throw HttpResponseValidationException.ValidationFailed<T>(validationResult);
        }

        return responseModel;
    }
}
