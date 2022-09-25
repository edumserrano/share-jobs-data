namespace ShareJobsDataCli.CliCommands.Errors;

public record CommandErrorMessage(string ErrorMessage, string Cause, string Details)
{
    public CommandException ToCommandException()
    {
        var message = ToString();
        return new CommandException(message);
    }

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
