namespace StoneAssemblies.Keycloak.MockServer
{
    using System;

    using GreenPipes;

    using MassTransit;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    using Serilog;

    using StoneAssemblies.Contrib.MassTransit.Extensions;
    using StoneAssemblies.Keycloak.Extensions;
    using StoneAssemblies.Keycloak.MockServer.Services;
    using StoneAssemblies.Keycloak.Services;
    using StoneAssemblies.Keycloak.Services.Interfaces;

    /// <summary>
    /// The startup.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">
        /// The configuration.
        /// </param>
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// The configure.
        /// </summary>
        /// <param name="app">
        /// The app.
        /// </param>
        /// <param name="env">
        /// The env.
        /// </param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHealthChecks("/health");

            app.UseRouting();

            app.UseEndpoints(
                endpoints => { endpoints.MapGet("/", async context => { await context.Response.WriteAsync("Hello World!"); }); });
        }

        /// <summary>
        /// The configure services.
        /// </summary>
        /// <param name="serviceCollection">
        /// The service collection.
        /// </param>
        public void ConfigureServices(IServiceCollection serviceCollection)
        {
            Log.Information("Configuring services");

            serviceCollection.AddHealthChecks();

            var username = this.Configuration.GetSection("RabbitMQ")?["Username"] ?? "queuedemo";
            var password = this.Configuration.GetSection("RabbitMQ")?["Password"] ?? "queuedemo";

            var host = this.Configuration.GetSection("RabbitMQ")?["Host"] ?? "localhost";
            var port = this.Configuration.GetSection("RabbitMQ")?["Port"] ?? "6002";

            serviceCollection.AddMassTransit(
                "PublicBus",
                serviceCollectionConfigurator =>
                    {
                        serviceCollectionConfigurator.AddKeycloakConsumers<JaneDoeUserRepository>();
                        serviceCollectionConfigurator.AddBus(
                            context => Bus.Factory.CreateUsingRabbitMq(
                                cfg =>
                                    {
                                        cfg.Host(
                                            new Uri(new Uri($"rabbitmq://{host}:{port}"), "public"),
                                            configurator =>
                                                {
                                                    configurator.Username(username);
                                                    configurator.Password(password);
                                                });
                                        cfg.KeycloakReceiveEndpoints<JaneDoeUserRepository>(
                                            context,
                                            e =>
                                                {
                                                    e.PrefetchCount = 16;
                                                    e.UseMessageRetry(x => x.Interval(2, 100));
                                                });
                                    }));
                    });

            // serviceCollection.AddSingleton<JohnDoeUserRepository>();

            serviceCollection.AddSingleton<JaneDoeUserRepository>();

            serviceCollection.AddSingleton<IEncryptionService>(provider => new AesEncryptionService("sOme*ShaREd*SecreT"));
            serviceCollection.AddMassTransitHostedService();
        }
    }
}