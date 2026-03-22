using Soenneker.GraphQl.Schema.Download.Abstract;
using Soenneker.Tests.FixturedUnit;
using Xunit;

namespace Soenneker.GraphQl.Schema.Download.Tests;

[Collection("Collection")]
public sealed class GraphQlSchemaDownloadUtilTests : FixturedUnitTest
{
    private readonly IGraphQlSchemaDownloadUtil _util;

    public GraphQlSchemaDownloadUtilTests(Fixture fixture, ITestOutputHelper output) : base(fixture, output)
    {
        _util = Resolve<IGraphQlSchemaDownloadUtil>(true);
    }

    [Fact]
    public void Default()
    {

    }
}
