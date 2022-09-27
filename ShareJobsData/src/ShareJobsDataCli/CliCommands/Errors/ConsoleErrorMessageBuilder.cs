namespace ShareJobsDataCli.CliCommands.Errors;

internal sealed class ConsoleErrorMessageBuilder
{
    private string? _command;
    private string? _error;

    public ConsoleErrorMessageBuilder UseCommand(string command)
    {
        _command = command;
        return this;
    }

    public ConsoleErrorMessageBuilder UseError(string error)
    {
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
