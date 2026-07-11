using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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

    [Test]
    public async Task Download_should_send_bearer_token()
    {
        AuthenticationHeaderValue? authorization = null;
        var handler = new StubHttpMessageHandler(request =>
        {
            authorization = request.Headers.Authorization;

            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("{\"data\":{\"__schema\":{}}}", Encoding.UTF8, "application/json")
            };
        });
        using var httpClient = new HttpClient(handler);

        await _util.Download(httpClient, "https://api.example.com/graphql", bearerToken: "authentication-token");

        await Assert.That(authorization).IsNotNull();
        await Assert.That(authorization!.Scheme).IsEqualTo("Bearer");
        await Assert.That(authorization.Parameter).IsEqualTo("authentication-token");
    }

    private sealed class StubHttpMessageHandler(Func<HttpRequestMessage, HttpResponseMessage> responseFactory) : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(responseFactory(request));
        }
    }
}
