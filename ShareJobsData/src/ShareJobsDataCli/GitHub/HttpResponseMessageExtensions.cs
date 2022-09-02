using static ShareJobsDataCli.GitHub.JsonHttpResult;

namespace ShareJobsDataCli.GitHub;

internal static class HttpResponseMessageExtensions
{
    public static async ValueTask<EnsureSuccessStatusCodeResult> EnsureSuccessStatusCodeAsync(this HttpResponseMessage httpResponse)
    {
        if (httpResponse.IsSuccessStatusCode)
        {
            return None;
        }

        var method = httpResponse.RequestMessage?.Method?.ToString() ?? "Unknown";
        var url = httpResponse.RequestMessage?.RequestUri?.ToString() ?? "Unknown";
        var statusCode = httpResponse.StatusCode.ToString();
        var errorResponseBody = await httpResponse.Content.ReadAsStringAsync();
        return new FailedStatusCodeHttpResponse(method, url, statusCode, errorResponseBody);
    }

    public static async Task<JsonHttpResult<TModel>> ReadFromJsonAsync<TModel, TValidator>(this HttpResponseMessage httpResponse)
         where TValidator : AbstractValidator<TModel>, new()
    {
        var ensureSuccessStatusCodeResult = await httpResponse.EnsureSuccessStatusCodeAsync();
        if (ensureSuccessStatusCodeResult is EnsureSuccessStatusCodeResult.NonSuccessStatusCode failed)
        {
            return FailedStatusCode<TModel>(failed.FailedStatusCodeHttpResponse);
        }

        if (ensureSuccessStatusCodeResult is EnsureSuccessStatusCodeResult.Ok)
        {
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

        throw UnexpectedTypeException.UnexpectedType(ensureSuccessStatusCodeResult);
    }
}
