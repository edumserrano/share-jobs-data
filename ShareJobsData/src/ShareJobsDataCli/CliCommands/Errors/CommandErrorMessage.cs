namespace ShareJobsDataCli.CliCommands.Errors;

public record CommandErrorMessage(string ErrorMessage, string Cause, string Details)
{
    // sealed keyword is required to enforce this ToString to be used by the derived classes
    // otherwise the compiler auto-generates a ToString for the derived types which bypasses this ToString implementation.
    // see https://stackoverflow.com/questions/64094373/c-sharp-9-0-records-tostring-not-inherited
    public sealed override string ToString()
    {
        return @$"{ErrorMessage}
Error:
{Cause}
Details:
{Details}";
    }
}

internal abstract record CommandErrorMessage2(string Command, string Error, string HttpError = "")
{
    // sealed keyword is required to enforce this ToString to be used by the derived classes
    // otherwise the compiler auto-generates a ToString for the derived types which bypasses this ToString implementation.
    // see https://stackoverflow.com/questions/64094373/c-sharp-9-0-records-tostring-not-inherited
    public sealed override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append("An error occurred trying to execute the ").Append(Command).AppendLine(" command.");
        sb.AppendLine("Error(s):");
        sb.Append("- ");
        sb.AppendLine(Error);
        if (!string.IsNullOrEmpty(HttpError))
        {
            sb.AppendLine();
            sb.AppendLine("HTTP Error(s):");
            sb.AppendLine("====================================");
            sb.AppendLine(HttpError);
            sb.AppendLine("====================================");
        }

        return sb.ToString();
    }
}
