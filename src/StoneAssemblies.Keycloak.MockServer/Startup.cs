namespace StoneAssemblies.Keycloak.MockServer
{
    using MassTransit;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    using Serilog;

    using StoneAssemblies.Keycloak.Messages;
    using StoneAssemblies.Keycloak.MockServer.Services;
    using StoneAssemblies.Keycloak.Services.Interfaces;
    using StoneAssemblies.MassAuth.Services;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection serviceCollection)
        {
            Log.Information("Configuring services");

            serviceCollection.AddHealthChecks();

            var username = this.Configuration.GetSection("RabbitMQ")?["Username"] ?? "queuedemo";
            var password = this.Configuration.GetSection("RabbitMQ")?["Password"] ?? "queuedemo";
            var host = this.Configuration.GetSection("RabbitMQ")?["Host"] ?? "localhost";
            var port = this.Configuration.GetSection("RabbitMQ")?["Port"] ?? "5672";

            serviceCollection.AddMassTransit(
                serviceCollectionConfigurator =>
                    {
                        serviceCollectionConfigurator.AddConsumer<KeycloakMessagesConsumer>();

                        serviceCollectionConfigurator.AddBus(
                            context => Bus.Factory.CreateUsingRabbitMq(
                                cfg =>
                                    {
                                        cfg.Host(
                                            $"rabbitmq://{host}:{port}",
                                            configurator =>
                                                {
                                                    configurator.Username(username);
                                                    configurator.Password(password);
                                                });

                                        cfg.ReceiveEndpoint(
                                            nameof(UsersCountRequestMessage),
                                            e =>
                                                {
                                                    e.ConfigureConsumer<KeycloakMessagesConsumer>(context);
                                                });

                                        cfg.ReceiveEndpoint(
                                            nameof(FindUserByIdRequestMessage),
                                            e =>
                                                {
                                                    e.ConfigureConsumer<KeycloakMessagesConsumer>(context);
                                                });

                                        cfg.ReceiveEndpoint(
                                            nameof(FindUserByUsernameOrEmailRequestMessage),
                                            e =>
                                                {
                                                    e.ConfigureConsumer<KeycloakMessagesConsumer>(context);
                                                });

                                        cfg.ReceiveEndpoint(
                                            nameof(UsersRequestMessage),
                                            e =>
                                                {
                                                    e.ConfigureConsumer<KeycloakMessagesConsumer>(context);
                                                });

                                        cfg.ReceiveEndpoint(
                                            nameof(ValidateCredentialsRequestMessage),
                                            e =>
                                                {
                                                    e.ConfigureConsumer<KeycloakMessagesConsumer>(context);
                                                });

                                        cfg.ReceiveEndpoint(
                                            nameof(UpdateCredentialsRequestMessage),
                                            e =>
                                                {
                                                    e.ConfigureConsumer<KeycloakMessagesConsumer>(context);
                                                });
                                    }));
                    });

            serviceCollection.AddSingleton<IUserRepository, MockUserRepository>();
            serviceCollection.AddHostedService<BusHostedService>();
        }
    }
}