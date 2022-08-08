using Flurl.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;

namespace TechTest.Ryanair.Tycoon.IntegrationTests.Fixtures;

public class TestServerFixture
{
    internal readonly WebApplicationFactory<Program> WebAppFactory;
    internal readonly TestServer Server;
    internal readonly IFlurlClient Client;
    internal readonly IServiceProvider ServiceProvider;

    public TestServerFixture()
    {
        WebAppFactory = new WebApplicationFactory<Program>();
        Server = WebAppFactory.Server;
        Client = new FlurlClient(Server.CreateClient());
        ServiceProvider = WebAppFactory.Services;
    }
}
