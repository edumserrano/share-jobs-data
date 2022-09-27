namespace ShareJobsDataCli.Http;

// The purpose of this class is just for syntax usage of the JsonHttpResult<T> class.
// It allow the following syntax: JsonHttpResult.JsonDeserializedToNull<TModel>() instead of new JsonHttpResult<TModel>.JsonDeserializedToNull();
// It also allows for better usage of the JsonHttpResult<T> by declaring a static using on this class JsonHttpResult and then just doing
// something like JsonDeserializedToNull<TModel>() instead of JsonHttpResult.JsonDeserializedToNull<TModel>()
internal static class JsonHttpResult
{
    public static JsonHttpResult<T> JsonDeserializedToNull<T>()
        where T : class
    {
        return new JsonHttpResult<T>.JsonDeserializedToNull();
    }

    public static JsonHttpResult<T> JsonModelValidationFailed<T>(ValidationResult validationResult)
        where T : class
    {
        return new JsonHttpResult<T>.JsonModelValidationFailed(validationResult);
    }

    public static JsonHttpResult<T> FailedStatusCode<T>(FailedStatusCodeHttpResponse failedStatusCodeHttpResponse)
        where T : class
    {
        return new JsonHttpResult<T>.FailedStatusCode(failedStatusCodeHttpResponse);
    }
}

internal abstract record JsonHttpResult<T>
    where T : class
{
    private JsonHttpResult()
    {
    }

    public sealed record Ok(T Response)
        : JsonHttpResult<T>;

    public abstract record Error()
        : JsonHttpResult<T>;

    public sealed record JsonDeserializedToNull()
        : Error;

    public sealed record JsonModelValidationFailed(ValidationResult ValidationResult)
        : Error;

    public sealed record FailedStatusCode(FailedStatusCodeHttpResponse FailedStatusCodeHttpResponse)
        : Error;

    public static implicit operator JsonHttpResult<T>(T response) => new Ok(response);

    public bool IsOk(
        [NotNullWhen(returnValue: true)] out T? response,
        [NotNullWhen(returnValue: false)] out Error? error)
    {
        response = null;
        error = null;

        if (this is Ok ok)
        {
            response = ok.Response;
            return true;
        }

        error = (Error)this;
        return false;
    }
}
