namespace ShareJobsDataCli.Tests.Auxiliary.Http;

internal sealed class OutboundHttpRequests : IReadOnlyCollection<LoggedSend>
{
    private readonly ConcurrentQueue<LoggedSend> _value;

    public OutboundHttpRequests(ConcurrentQueue<LoggedSend> outboundHttpRequests)
    {
        _value = outboundHttpRequests;
    }

    public int Count => _value.Count;

    public IEnumerator<LoggedSend> GetEnumerator() => _value.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => _value.GetEnumerator();
}
