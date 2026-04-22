using Soenneker.GraphQl.Schema.Download.Abstract;
using Soenneker.Tests.HostedUnit;

namespace Soenneker.GraphQl.Schema.Download.Tests;

[ClassDataSource<Host>(Shared = SharedType.PerTestSession)]
public sealed class GraphQlSchemaDownloadUtilTests : HostedUnitTest
{
    private readonly IGraphQlSchemaDownloadUtil _util;

    public GraphQlSchemaDownloadUtilTests(Host host) : base(host)
    {
        _util = Resolve<IGraphQlSchemaDownloadUtil>(true);
    }

    [Test]
    public void Default()
    {

    }
}
