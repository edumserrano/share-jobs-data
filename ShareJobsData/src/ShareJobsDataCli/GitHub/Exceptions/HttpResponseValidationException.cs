namespace ShareJobsDataCli.GitHub.Exceptions;

/// <summary>
/// The purpose of this class is mainly for syntax usage of the HttpResponseValidationException<T> class.
/// It allows moving the generic declaration to the method instead of the HttpResponseValidationException type.
/// For instance, instead of HttpResponseValidationException<MyType>.SomeMethod we can do HttpResponseValidationException.SomeMethod<MyType>
/// The fact that this type doesn't declare the T type in the class declaration enables this.
///
/// The other reason is that it allows the HttpResponseValidationException<T> type to remain simple and
/// moves some custom logic on how to build these types of extensions to this class.
/// </summary>
internal class HttpResponseValidationException
{
    public static HttpResponseValidationException<T> JsonDeserializedToNull<T>()
    {
        return new HttpResponseValidationException<T>($"{typeof(T)} deserialized to null.");
    }

    public static HttpResponseValidationException<T> ValidationFailed<T>(ValidationResult validationResult)
    {
        var errorMessages = validationResult.Errors.Select(x => x.ErrorMessage);
        var formattedErrorMessages = string.Join($"{Environment.NewLine}  - ", errorMessages);
        var message = $"Failed to validate {typeof(T)}:{Environment.NewLine}  - {formattedErrorMessages}";
        return new HttpResponseValidationException<T>(message);
    }
}

public class HttpResponseValidationException<T> : Exception
{
    public HttpResponseValidationException(string message)
        : base(message)
    {
    }
}
