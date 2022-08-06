using Shouldly;

namespace ShareJobsDataCli.Tests;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        const int a = 2;
        a.ShouldBe(2);
    }
}
