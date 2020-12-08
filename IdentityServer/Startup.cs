using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using IdentityServer4.EntityFramework.DbContexts;
using System.Linq;
using IdentityServer4.EntityFramework.Mappers;
using System.Collections.Generic;
using IdentityServer.Models;
using IdentityServer.Entity;

namespace IdentityServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string connectionString = Configuration.GetConnectionString("DefaultConnection");
            

            //
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
            services.AddDbContext<UserContext>(options => options.UseSqlServer(connectionString));

            // configure identity server with in-memory stores, keys, clients and scopes
            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddTestUsers(Config.GetUsers())
                // this adds the config data from DB (clients, resources)
                .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = builder =>
                        builder.UseSqlServer(connectionString,
                            sql => sql.MigrationsAssembly(migrationsAssembly));
                })
                // this adds the operational data from DB (codes, tokens, consents)
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = builder =>
                        builder.UseSqlServer(connectionString,
                            sql => sql.MigrationsAssembly(migrationsAssembly));

                    // this enables automatic token cleanup. this is optional.
                    options.EnableTokenCleanup = false;
                    options.TokenCleanupInterval = 300;
                });
            //.AddInMemoryClients(Config.GetClients());
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            InitializeDatabase(app);

            app.UseIdentityServer();

        
        }

        private void InitializeDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                // User and Claims
                serviceScope.ServiceProvider.GetRequiredService<UserContext>().Database.Migrate();

                var context = serviceScope.ServiceProvider.GetRequiredService<UserContext>();
                context.Database.Migrate();
                if (!context.Users.Any())
                {
                    Models.User user = new Models.User()
                    {
                        UserId = "1",
                        UserName = "brightasia",
                        Password = "richviet",
                        IsActive = true,
                        Claims = new List<Models.Claims>
                        {
                            new Models.Claims("role","adminManager")
                        }
                    };
                    context.Users.Add(user.ToEntity());
                    context.SaveChanges();

                    // configuration
                    serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

                    var configurationContext = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                    configurationContext.Database.Migrate();
                    if (!configurationContext.Clients.Any())
                    {
                        foreach (var client in Config.GetClients())
                        {
                            configurationContext.Add(client.ToEntity());
                        }
                        configurationContext.SaveChanges();
                    }

                    if (!configurationContext.IdentityResources.Any())
                    {
                        foreach (var resource in Config.GetIdentityResources())
                        {
                            configurationContext.IdentityResources.Add(resource.ToEntity());
                        }
                        configurationContext.SaveChanges();
                    }

                    if (!configurationContext.ApiResources.Any())
                    {
                        foreach (var resource in Config.GetApiResources())
                        {
                            configurationContext.ApiResources.Add(resource.ToEntity());
                        }
                        configurationContext.SaveChanges();
                    }
                }
            }
        }
    }
}
