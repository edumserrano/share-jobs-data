using static ShareJobsDataCli.Common.Http.JsonHttpResult;

namespace ShareJobsDataCli.Common.Http;

internal static class HttpResponseMessageExtensions
{
    public static async ValueTask<EnsureSuccessStatusCodeResult> EnsureSuccessStatusCodeAsync(this HttpResponseMessage httpResponse, CancellationToken cancellationToken = default)
    {
        httpResponse.NotNull();

        if (httpResponse.IsSuccessStatusCode)
        {
            return new EnsureSuccessStatusCodeResult.Ok();
        }

        var errorResponseBody = await httpResponse.Content.ReadAsStringAsync(cancellationToken);
        return httpResponse.ToFailedStatusCodeHttpResponse(errorResponseBody);
    }

    public static async Task<JsonHttpResult<TModel>> ReadFromJsonAsync<TModel, TValidator>(this HttpResponseMessage httpResponse, CancellationToken cancellationToken = default)
         where TModel : class
         where TValidator : AbstractValidator<TModel>, new()
    {
        httpResponse.NotNull();

        var ensureSuccessStatusCodeResult = await httpResponse.EnsureSuccessStatusCodeAsync(cancellationToken);
        if (!ensureSuccessStatusCodeResult.IsOk(out var failedStatusCodeHttpResponse))
        {
            return FailedStatusCode<TModel>(failedStatusCodeHttpResponse);
        }

        var responseModel = await httpResponse.Content.ReadFromJsonAsync<TModel>(cancellationToken);
        if (responseModel is null)
        {
            return JsonDeserializedToNull<TModel>();
        }

        var validator = new TValidator();
        var validationResult = await validator.ValidateAsync(responseModel, cancellationToken);
        if (!validationResult.IsValid)
        {
            return JsonModelValidationFailed<TModel>(validationResult);
        }

        return responseModel;
    }
}
