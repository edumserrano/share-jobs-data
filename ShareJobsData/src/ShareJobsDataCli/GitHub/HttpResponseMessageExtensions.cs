namespace ShareJobsDataCli.GitHub;

internal static class HttpResponseMessageExtensions
{
    public static async Task<TModel> ReadFromJsonAsync<TModel, TValidator>(this HttpResponseMessage httpResponse)
         where TValidator : AbstractValidator<TModel>, new()
    {
        await httpResponse.EnsureSuccessStatusCodeAsync();
        var responseModel = await httpResponse.Content.ReadFromJsonAsync<TModel>();
        if (responseModel is null)
        {
            throw HttpResponseValidationException.JsonDeserializedToNull<TModel>();
        }

        var validator = new TValidator();
        var validationResult = validator.Validate(responseModel);
        if (!validationResult.IsValid)
        {
            throw HttpResponseValidationException.ValidationFailed<TModel>(validationResult);
        }

        return responseModel;
    }
}
