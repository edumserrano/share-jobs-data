namespace ShareJobsDataCli.CliCommands.Errors;

public record CommandExceptionMessage(string ErrorMessage, string Cause, string Details)
{
    public CommandException ToCommandException()
    {
        var message = ToString();
        return new CommandException(message);
    }

    // sealed keyword is required to stop the compiler from auto-generating the ToString
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
