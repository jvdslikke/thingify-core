using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using ThingifyCore.BackgroundService;
using ThingifyCore.Models;
using System.Collections.Generic;

namespace ThingifyCore;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSwaggerGen(options => {
            // only add api controllers
            options.DocInclusionPredicate(delegate (string d, ApiDescription api) 
            {
                if (api.RelativePath == null)
                {
                    return true;
                }

                var result = api.RelativePath.StartsWith("api-rest");
                return result;
            });
        });

        builder.Services.AddSingleton<IThingsRepository>(ctx =>
        {
            return new ThingsRepository();
        });
        builder.Services.AddHostedService<BackgroundService.BackgroundService>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        app.UseSwagger(options => 
        {
            options.RouteTemplate = "api-rest/{documentName}/swagger.json";
        });
        app.UseSwaggerUI(options => 
        {
            options.SwaggerEndpoint("/api-rest/v1/swagger.json", "My API v1");
            options.RoutePrefix = "api-rest";
        });

        app.UseAuthorization();

        app.UseDefaultFiles();
        app.UseStaticFiles();

        app.MapControllers();

        // seed things
        using (var serviceScope = app.Services.CreateScope())
        {
            var services = serviceScope.ServiceProvider;

            var myDependency = services.GetRequiredService<IThingsRepository>();
            myDependency.Upsert(new [] {
                new Thing(
                    id: "test",
                    things: new List<Thing>
                    {
                        new Thing(
                            id: "subtest",
                            things: new List<Thing>
                            {
                                new Thing("subsubtest")
                            })
                    }),
                new Thing(
                    id: "test2",
                    things: new List<Thing>
                    {
                        new Thing("test2sub"),
                        new Thing("test2sub2")
                    }
                )
            });
        }

        app.Run();
    }
}
