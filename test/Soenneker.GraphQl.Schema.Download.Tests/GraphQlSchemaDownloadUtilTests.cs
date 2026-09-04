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
                Content = new StringContent("{\"data\":{\"__schema\":{\"types\":[]}}}", Encoding.UTF8, "application/json")
            };
        });
        using var httpClient = new HttpClient(handler);

        await _util.Download(httpClient, "https://api.example.com/graphql", bearerToken: "authentication-token");

        await Assert.That(authorization).IsNotNull();
        await Assert.That(authorization!.Scheme).IsEqualTo("Bearer");
        await Assert.That(authorization.Parameter).IsEqualTo("authentication-token");
    }

    [Test]
    public async Task Download_should_reject_a_null_schema()
    {
        var handler = new StubHttpMessageHandler(_ => new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("{\"data\":{\"__schema\":null}}", Encoding.UTF8, "application/json")
        });
        using var httpClient = new HttpClient(handler);

        Func<Task> act = async () => await _util.Download(httpClient, "https://api.example.com/graphql");

        await Assert.That(act).Throws<InvalidOperationException>();
    }

    [Test]
    public async Task Download_should_retry_without_isOneOf_when_endpoint_does_not_support_it()
    {
        var requestCount = 0;
        var firstRequestIncludedIsOneOf = false;
        var secondRequestIncludedIsOneOf = true;
        var handler = new StubHttpMessageHandler(request =>
        {
            requestCount++;
            string requestJson = request.Content!.ReadAsStringAsync().GetAwaiter().GetResult();

            if (requestCount == 1)
                firstRequestIncludedIsOneOf = requestJson.Contains("isOneOf", StringComparison.Ordinal);
            else
                secondRequestIncludedIsOneOf = requestJson.Contains("isOneOf", StringComparison.Ordinal);

            string content = requestCount == 1
                ? "{\"errors\":[{\"message\":\"Cannot query field \\\"isOneOf\\\" on type \\\"__Type\\\".\"}]}"
                : "{\"data\":{\"__schema\":{\"types\":[]}}}";

            return new HttpResponseMessage(HttpStatusCode.OK) {Content = new StringContent(content, Encoding.UTF8, "application/json")};
        });
        using var httpClient = new HttpClient(handler);

        string result = await _util.Download(httpClient, "https://api.example.com/graphql");

        await Assert.That(requestCount).IsEqualTo(2);
        await Assert.That(firstRequestIncludedIsOneOf).IsTrue();
        await Assert.That(secondRequestIncludedIsOneOf).IsFalse();
        await Assert.That(result).Contains("__schema");
    }

    private sealed class StubHttpMessageHandler(Func<HttpRequestMessage, HttpResponseMessage> responseFactory) : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(responseFactory(request));
        }
    }
}
