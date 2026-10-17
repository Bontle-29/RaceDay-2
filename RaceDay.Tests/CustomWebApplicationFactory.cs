using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using RaceDay.Api.Data;

namespace RaceDay.Tests;

public class CustomWebApplicationFactory
    : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(
        IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureServices(services =>
        {
            services.RemoveAll<
                DbContextOptions<RaceDayDbContext>>();

            services.RemoveAll<
                DbContextOptions>();

            services.RemoveAll<RaceDayDbContext>();

            services.AddDbContext<RaceDayDbContext>(options =>
            {
                options.UseInMemoryDatabase(
                    "RaceDayTestDatabase");
            });
        });
    }
}

