namespace ShareJobsDataCli.Guards;

public sealed class GuardException : Exception
{
    internal GuardException(string message)
        : base(message)
    {
    }

    [DoesNotReturn]
    internal static void Throw(string message) => throw new GuardException(message);
}
