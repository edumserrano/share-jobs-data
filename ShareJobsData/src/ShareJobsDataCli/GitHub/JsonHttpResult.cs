namespace ShareJobsDataCli.GitHub;

// The purpose of this class is just for syntax usage of the JsonHttpResult<T> class.
// It allow the following syntax: JsonHttpResult.JsonDeserializedToNull<TModel>() instead of new JsonHttpResult<TModel>.JsonDeserializedToNull();
// It also allows for better usage of the JsonHttpResult<T> by declaring a static using on this class JsonHttpResult and then just doing
// something like JsonDeserializedToNull<TModel>() instead of JsonHttpResult.JsonDeserializedToNull<TModel>() 
internal static class JsonHttpResult
{
    public static JsonHttpResult<T> JsonDeserializedToNull<T>() => new JsonHttpResult<T>.JsonDeserializedToNull();

    public static JsonHttpResult<T> JsonModelValidationFailed<T>(ValidationResult validationResult) => new JsonHttpResult<T>.JsonModelValidationFailed(validationResult);

    public static JsonHttpResult<T> FailedStatusCode<T>(FailedStatusCodeHttpResponse failedStatusCodeHttpResponse) => new JsonHttpResult<T>.FailedStatusCode(failedStatusCodeHttpResponse);
}

internal abstract record JsonHttpResult<T>
{
    private JsonHttpResult()
    {
    }

    public interface INotOk
    {
    }

    public record Ok(T Response)
        : JsonHttpResult<T>;

    public record Error()
        : JsonHttpResult<T>;

    public record JsonDeserializedToNull()
        : Error;

    public record JsonModelValidationFailed(ValidationResult ValidationResult)
        : Error;

    public record FailedStatusCode(FailedStatusCodeHttpResponse FailedStatusCodeHttpResponse)
        : Error;

    public static implicit operator JsonHttpResult<T>(T response) => new Ok(response);
}
