namespace ShareJobsDataCli.Common.Guards;

// See /docs/dev-notes/code-details/throw-helper.md to understand more about this implementation.
public sealed class GuardException : Exception
{
    internal GuardException(string message)
        : base(message)
    {
    }

    [DoesNotReturn]
    internal static void Throw(string message) => throw new GuardException(message);
}
