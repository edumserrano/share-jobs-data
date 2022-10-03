namespace ShareJobsDataCli.Common.Cli.Errors;

internal sealed class ErrorMessageBuilder
{
    private string? _command;
    private string? _error;

    public ErrorMessageBuilder UseCommand(string command)
    {
        command.NotNullOrWhiteSpace();
        _command = command;
        return this;
    }

    public ErrorMessageBuilder UseError(string error)
    {
        error.NotNullOrWhiteSpace();
        _error = error;
        return this;
    }

    public string Build()
    {
        var sb = new StringBuilder();
        sb.Append("An error occurred trying to execute the ").Append(_command).AppendLine(" command.");
        sb.AppendLine("Error(s):");
        sb.Append("- ");
        sb.Append(_error);
        return sb.ToString();
    }
}
