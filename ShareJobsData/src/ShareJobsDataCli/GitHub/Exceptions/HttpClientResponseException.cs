namespace ShareJobsDataCli.GitHub.Exceptions;

internal static class HttpClientResponseExceptionExtensions
{
    public static async ValueTask EnsureSuccessStatusCodeAsync(this HttpResponseMessage httpResponse)
    {
        if (httpResponse.IsSuccessStatusCode)
        {
            return;
        }

        var method = httpResponse.RequestMessage?.Method?.ToString() ?? "Unknown";
        var url = httpResponse.RequestMessage?.RequestUri?.ToString() ?? "Unknown";
        var statusCode = httpResponse.StatusCode.ToString();
        var errorResponseBody = await httpResponse.Content.ReadAsStringAsync();
        throw new HttpClientResponseException(method, url, statusCode, errorResponseBody);
    }
}

public sealed class HttpClientResponseException : Exception
{
    internal HttpClientResponseException(
        string method,
        string requestUrl,
        string statusCode,
        string responseBody)
        : base($"An error ocurrered making a request to GitHub. Got {statusCode} from {method} {requestUrl}. The response body was: '{responseBody}'.")
    {
    }
}
