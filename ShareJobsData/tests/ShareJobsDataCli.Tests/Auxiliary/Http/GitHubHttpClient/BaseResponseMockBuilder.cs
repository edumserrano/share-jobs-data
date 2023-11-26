namespace ShareJobsDataCli.Tests.Auxiliary.Http.GitHubHttpClient;

// The signature `BaseResponseMockBuilder<T> where T : BaseResponseMockBuilder<T>`
// looks weird at first sight right?
//
// Here are a few links that help explain the benefits of implementing `A<T> : A<T>` and using the
// `return (T)this;` in the base class:
// - https://www.codeproject.com/Articles/240756/Hierarchically-Implementing-the-Bolchs-Builder-Pat
// - https://code-maze.com/fluent-builder-recursive-generics/
// - https://www.appsloveworld.com/csharp/100/192/chaining-methods-in-base-and-derived-class
// - https://github.com/dotnet/csharplang/issues/2495
//
// Apparently this pattern has a name, it's called the `Curiously recurring template pattern`:
// - http://www.codecutout.com/blog/curiously-repeating-template-pattern/
//
// In short:
// It looks weird at first but it enables a fluent like API with a generic
// but the `return (T)this;` allows the chaining to be specifically tied to
// the generic type T.
//
// See all the types that implement BaseResponseMockBuilder<T>. All of these types
// have access to the methods in the base class but when chaining you will also have
// access to the methods specific to the generic T type.
internal abstract class BaseResponseMockBuilder<T>
    where T : BaseResponseMockBuilder<T>
{
    protected HttpStatusCode ResponseHttpStatusCode { get; set; } = HttpStatusCode.OK;

    protected TestFilepath? ResponseContentFilepath { get; set; }

    protected string? ResponseContentAsString { get; set; }

    protected abstract string OperationName { get; }

    public T WithResponseStatusCode(HttpStatusCode responseHttpStatusCode)
    {
        ResponseHttpStatusCode = responseHttpStatusCode;
        return (T)this;
    }

    public T WithResponseContentFromFilepath(TestFilepath responseContentFilepath)
    {
        ResponseContentFilepath = responseContentFilepath;
        return (T)this;
    }

    public T WithResponseContent(string responseContent)
    {
        ResponseContentAsString = responseContent;
        return (T)this;
    }

    protected abstract string GetRequestUrl();

    protected virtual HttpContent? GetResponseContent()
    {
        if (ResponseContentFilepath is not null)
        {
            return ResponseContentFilepath.ReadFileAsStringContent();
        }

        if (ResponseContentAsString is not null)
        {
            return new StringContent(ResponseContentAsString);
        }

        return null;
    }

    public void Build(HttpResponseMessageMockBuilder httpResponseMessageMockBuilder)
    {
        var requestUrl = GetRequestUrl();
        if (string.IsNullOrEmpty(requestUrl))
        {
            throw new InvalidOperationException($"Invalid response mock configuration for {OperationName}");
        }

        var responseContent = GetResponseContent();
        httpResponseMessageMockBuilder
            .WhereRequestUriEquals(requestUrl)
            .RespondWith(httpRequestMessage => new HttpResponseMessage(ResponseHttpStatusCode)
            {
                // this is required when testing failure status codes because the app returns information from the http request made
                RequestMessage = httpRequestMessage,
                Content = responseContent,
            });
    }
}
