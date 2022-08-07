namespace ShareJobsDataCli.GitHub;

internal static class HttpResponseMessageExtensions
{
    public static async Task<T> ReadFromJsonAsync<T>(this HttpResponseMessage httpResponse)
    {
        await httpResponse.EnsureSuccessStatusCodeAsync();
        var responseModel = await httpResponse.Content.ReadFromJsonAsync<T>();
        if (responseModel is null)
        {
            throw HttpResponseValidationException.JsonDeserializedToNull<T>();
        }

        return responseModel;
    }
}
