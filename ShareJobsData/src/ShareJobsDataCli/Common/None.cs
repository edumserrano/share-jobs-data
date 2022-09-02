namespace ShareJobsDataCli.Common;

public static class NoneStaticUsingExtensions
{
    public static NoneResult None => NoneResult.Instance;
}

public record NoneResult
{
    private static readonly Lazy<NoneResult> _instance = new Lazy<NoneResult>(() => new NoneResult());

    private NoneResult()
    {
    }

    public static NoneResult Instance => _instance.Value;
}
