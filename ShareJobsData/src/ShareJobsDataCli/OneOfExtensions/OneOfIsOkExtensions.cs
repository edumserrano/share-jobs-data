namespace ShareJobsDataCli.OneOfExtensions;

internal static class OneOfIsOkExtensions
{
    public static bool IsOk<T0, T1, T2>(
        this OneOfBase<T0, T1, T2> oneOf,
        [NotNullWhen(returnValue: true)] out T0? ok,
        out OneOf<T1, T2> error)
    {
        return oneOf.TryPickT0(out ok, out error);
    }

    public static bool IsOk<T0, T1, T2, T3>(
        this OneOfBase<T0, T1, T2, T3> oneOf,
        [NotNullWhen(returnValue: true)] out T0? ok,
        out OneOf<T1, T2, T3> error)
    {
        return oneOf.TryPickT0(out ok, out error);
    }
}
