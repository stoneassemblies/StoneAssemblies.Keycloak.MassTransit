namespace StoneAssemblies.Keycloak.MockServer
{
    using System;

    using MassTransit;
    using MassTransit.MultiBus;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    using Serilog;

    using StoneAssemblies.Contrib.MassTransit.Extensions;
    using StoneAssemblies.Keycloak.Consumers;
    using StoneAssemblies.Keycloak.Messages;
    using StoneAssemblies.Keycloak.MockServer.Services;
    using StoneAssemblies.Keycloak.Services;
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
            var port = this.Configuration.GetSection("RabbitMQ")?["Port"] ?? "6002";

            
            serviceCollection.AddMassTransit("PublicBus",
                serviceCollectionConfigurator =>
                    {
                        serviceCollectionConfigurator.AddConsumer<UsersCountRequestMessageConsumer<JaneDoeUserRepository>>();
                        serviceCollectionConfigurator.AddConsumer<FindUserByIdRequestMessageConsumer<JaneDoeUserRepository>>();
                        serviceCollectionConfigurator.AddConsumer<FindUserByUsernameOrEmailRequestMessageConsumer<JaneDoeUserRepository>>();
                        serviceCollectionConfigurator.AddConsumer<UsersRequestMessageConsumer<JaneDoeUserRepository>>();
                        serviceCollectionConfigurator.AddConsumer<ValidateCredentialsRequestMessageConsumer<JaneDoeUserRepository>>();
                        serviceCollectionConfigurator.AddConsumer<UpdateCredentialsRequestMessageConsumer<JaneDoeUserRepository>>();

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

                                        cfg.ReceiveEndpoint(
                                            nameof(UsersCountRequestMessage),
                                            e =>
                                                {
                                                    e.ConfigureConsumer<UsersCountRequestMessageConsumer<JaneDoeUserRepository>>(context);
                                                });

                                        cfg.ReceiveEndpoint(
                                            nameof(FindUserByIdRequestMessage),
                                            e =>
                                                {
                                                    e.ConfigureConsumer<FindUserByIdRequestMessageConsumer<JaneDoeUserRepository>>(context);
                                                });

                                        cfg.ReceiveEndpoint(
                                            nameof(FindUserByUsernameOrEmailRequestMessage),
                                            e =>
                                                {
                                                    e.ConfigureConsumer<FindUserByUsernameOrEmailRequestMessageConsumer<JaneDoeUserRepository>>(context);
                                                });

                                        cfg.ReceiveEndpoint(
                                            nameof(UsersRequestMessage),
                                            e =>
                                                {
                                                    e.ConfigureConsumer<UsersRequestMessageConsumer<JaneDoeUserRepository>>(context);
                                                });

                                        cfg.ReceiveEndpoint(
                                            nameof(ValidateCredentialsRequestMessage),
                                            e =>
                                                {
                                                    e.ConfigureConsumer<ValidateCredentialsRequestMessageConsumer<JaneDoeUserRepository>>(context);
                                                });

                                        cfg.ReceiveEndpoint(
                                            nameof(UpdateCredentialsRequestMessage),
                                            e =>
                                                {
                                                    e.ConfigureConsumer<UpdateCredentialsRequestMessageConsumer<JaneDoeUserRepository>>(context);
                                                });
                                    }));
                    });
            
            //serviceCollection.AddMassTransit("PrivateBus",
            //    serviceCollectionConfigurator =>
            //        {
            //            serviceCollectionConfigurator.AddConsumer<UsersCountRequestMessageConsumer<JohnDoeUserRepository>>();
            //            serviceCollectionConfigurator.AddConsumer<FindUserByIdRequestMessageConsumer<JohnDoeUserRepository>>();
            //            serviceCollectionConfigurator.AddConsumer<FindUserByUsernameOrEmailRequestMessageConsumer<JohnDoeUserRepository>>();
            //            serviceCollectionConfigurator.AddConsumer<UsersRequestMessageConsumer<JohnDoeUserRepository>>();
            //            serviceCollectionConfigurator.AddConsumer<ValidateCredentialsRequestMessageConsumer<JohnDoeUserRepository>>();
            //            serviceCollectionConfigurator.AddConsumer<UpdateCredentialsRequestMessageConsumer<JohnDoeUserRepository>>();

            //            serviceCollectionConfigurator.AddBus(
            //                context => Bus.Factory.CreateUsingRabbitMq(
            //                    cfg =>
            //                        {
            //                            cfg.Host(
            //                                new Uri(new Uri($"rabbitmq://{host}:{port}"), "private"),
            //                                configurator =>
            //                                    {
            //                                        configurator.Username(username);
            //                                        configurator.Password(password);
            //                                    });
            //                            cfg.ReceiveEndpoint(
            //                                nameof(UsersCountRequestMessage),
            //                                e =>
            //                                {
            //                                    e.ConfigureConsumer<UsersCountRequestMessageConsumer<JohnDoeUserRepository>>(context);
            //                                });

            //                            cfg.ReceiveEndpoint(
            //                                nameof(FindUserByIdRequestMessage),
            //                                e =>
            //                                {
            //                                    e.ConfigureConsumer<FindUserByIdRequestMessageConsumer<JohnDoeUserRepository>>(context);
            //                                });

            //                            cfg.ReceiveEndpoint(
            //                                nameof(FindUserByUsernameOrEmailRequestMessage),
            //                                e =>
            //                                {
            //                                    e.ConfigureConsumer<FindUserByUsernameOrEmailRequestMessageConsumer<JohnDoeUserRepository>>(context);
            //                                });

            //                            cfg.ReceiveEndpoint(
            //                                nameof(UsersRequestMessage),
            //                                e =>
            //                                {
            //                                    e.ConfigureConsumer<UsersRequestMessageConsumer<JohnDoeUserRepository>>(context);
            //                                });

            //                            cfg.ReceiveEndpoint(
            //                                nameof(ValidateCredentialsRequestMessage),
            //                                e =>
            //                                {
            //                                    e.ConfigureConsumer<ValidateCredentialsRequestMessageConsumer<JohnDoeUserRepository>>(context);
            //                                });

            //                            cfg.ReceiveEndpoint(
            //                                nameof(UpdateCredentialsRequestMessage),
            //                                e =>
            //                                {
            //                                    e.ConfigureConsumer<UpdateCredentialsRequestMessageConsumer<JohnDoeUserRepository>>(context);
            //                                });
            //                        }));
            //        });

            serviceCollection.AddSingleton<JohnDoeUserRepository>();
            serviceCollection.AddSingleton<JaneDoeUserRepository>();

            serviceCollection.AddSingleton<IEncryptionService>(provider => new AesEncryptionService("sOme*ShaREd*SecreT"));
            serviceCollection.AddMassTransitHostedService();
        }
    }
}