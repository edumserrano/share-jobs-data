namespace ShareJobsDataCli.CliCommands.Errors;

internal static class FailedStatusCodeHttpResponseErrorExtensions
{
    public static string AsHttpErrorMEssage(this FailedStatusCodeHttpResponse httpResponse)
    {
        var sb = new StringBuilder();
        sb.Append(httpResponse.Method).Append(' ')
            .Append(httpResponse.RequestUrl).Append(" returned ")
            .Append(httpResponse.StatusCode);
        if (string.IsNullOrEmpty(httpResponse.ResponseBody))
        {
            sb.AppendLine(" without response body.");
        }
        else
        {
            sb.AppendLine(" with the following response body:");
            sb.AppendLine("---START---");
            sb.AppendLine(httpResponse.ResponseBody);
            sb.AppendLine("---END---");
        }

        return sb.ToString();
    }
}
