using System;
using System.Net;
using Xunit;
using Xunit.Abstractions;

namespace BookAppoinment.IntegrationTests.Endpoints.Api;

public class Info_IntegrationTests : Qwiik_IntegrationTests
{
    public Info_IntegrationTests(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public async Task Can_return_info()
    {
        var response = await _sut.GetAsync("/api/info");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
