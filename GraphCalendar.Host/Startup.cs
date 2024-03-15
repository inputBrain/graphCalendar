using System.Net.Http.Headers;
using Azure.Core;
using Azure.Identity;
using GraphCalendar.Host.Configs;
using Microsoft.Graph;
using Microsoft.OpenApi.Models;

namespace GraphCalendar.Host;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        var azureConfig = Configuration.GetSection("Azure").Get<AzureConfig>();
        
        if (azureConfig == null)
        {
            throw new Exception("\n\n -----ERROR ATTENTION! ----- \n Config in appsettings.json 'Azure' does not exist. \n\n");
        }

        var options = new ClientSecretCredentialOptions
        {
            AuthorityHost = AzureAuthorityHosts.AzurePublicCloud,
        };
        
        var clientSecretCredential = new ClientSecretCredential(
            azureConfig.TenantId,
            azureConfig.ClientId,
            azureConfig.ClientSecret,
            options
        );

        var scopes = new[] { "https://graph.microsoft.com/.default" };
        var graphClient = new GraphServiceClient(clientSecretCredential, scopes);

        services.AddSingleton(graphClient);
        
        
        
        services.AddControllers();
        services.AddSwaggerGen(
            c => { c.SwaggerDoc("v1", new OpenApiInfo {Title = "GraphCalendar.Host", Version = "v1"}); }
        );
    }


    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "GraphCalendar.Host v1"));
        }

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
}