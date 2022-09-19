using static ShareJobsDataCli.GitHub.JsonHttpResult;

namespace ShareJobsDataCli.GitHub;

internal static class HttpResponseMessageExtensions
{
    public static async ValueTask<EnsureSuccessStatusCodeResult> EnsureSuccessStatusCodeAsync(this HttpResponseMessage httpResponse)
    {
        if (httpResponse.IsSuccessStatusCode)
        {
            return new EnsureSuccessStatusCodeResult.Ok();
        }

        var errorResponseBody = await httpResponse.Content.ReadAsStringAsync();
        return httpResponse.ToFailedStatusCodeHttpResponse(errorResponseBody);
    }

    public static async Task<JsonHttpResult<TModel>> ReadFromJsonAsync<TModel, TValidator>(this HttpResponseMessage httpResponse)
         where TModel : class
         where TValidator : AbstractValidator<TModel>, new()
    {
        var ensureSuccessStatusCodeResult = await httpResponse.EnsureSuccessStatusCodeAsync();
        if (!ensureSuccessStatusCodeResult.IsOk(out var failedStatusCodeHttpResponse))
        {
            return FailedStatusCode<TModel>(failedStatusCodeHttpResponse);
        }

        var responseDebug = await httpResponse.Content.ReadAsStringAsync();
        Console.WriteLine(responseDebug);

        var responseModel = await httpResponse.Content.ReadFromJsonAsync<TModel>();
        if (responseModel is null)
        {
            return JsonDeserializedToNull<TModel>();
        }

        var validator = new TValidator();
        var validationResult = validator.Validate(responseModel);
        if (!validationResult.IsValid)
        {
            return JsonModelValidationFailed<TModel>(validationResult);
        }

        return responseModel;
    }
}
