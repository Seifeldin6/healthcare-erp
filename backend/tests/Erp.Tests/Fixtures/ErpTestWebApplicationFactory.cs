using Erp.Api;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace Erp.Tests.Fixtures;

public class ErpTestWebApplicationFactory : WebApplicationFactory<Program>
{
    public const string DefaultVideoServerUrl = "https://meet.jit.si";

    public string VideoServerUrl => DefaultVideoServerUrl;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Development");

        builder.ConfigureAppConfiguration((_, config) =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Video:ServerUrl"] = DefaultVideoServerUrl
            });
        });
    }
}
