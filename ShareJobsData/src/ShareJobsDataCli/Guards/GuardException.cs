namespace ShareJobsDataCli.Guards;

public sealed class GuardException : Exception
{
    internal GuardException(string message)
        : base(message)
    {
    }

    [DoesNotReturn]
    internal static void Throw(string message) => throw CreateGuardException(message);

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static Exception CreateGuardException(string message)
    {
        return new GuardException(message);
    }
}
