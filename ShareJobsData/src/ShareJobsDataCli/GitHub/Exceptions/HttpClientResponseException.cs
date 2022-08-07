using System.Net;

namespace ShareJobsDataCli.GitHub.Exceptions;

public class HttpClientResponseException : Exception
{
    internal HttpClientResponseException(
        HttpMethod method,
        string requestUrl,
        HttpStatusCode statusCode,
        string responseBody)
        : base($"An error ocurrered making a request to GitHub. Got {(int)statusCode} {statusCode} from {method} {requestUrl}. The response body was: '{responseBody}'.")
    {
    }
}
