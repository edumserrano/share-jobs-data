namespace ShareJobsDataCli.Common;

public record CommandExceptionMessage(string ErrorMessage, string Cause, string Details)
{
    public CommandException ToCommandException()
    {
        var message = ToString();
        return new CommandException(message);
    }

    public override string ToString()
    {
        return @$"{ErrorMessage}
Error:
{Cause}
Details:
{Details}";
    }
}
