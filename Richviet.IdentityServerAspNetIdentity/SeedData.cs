// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Linq;
using System.Security.Claims;
using IdentityModel;
using Richviet.IdentityServerAspNetIdentity.Data;
using Richviet.IdentityServerAspNetIdentity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Richviet.IdentityServerAspNetIdentity
{
    public class SeedData
    {
        public static void EnsureSeedData(string connectionString)
        {
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddDbContext<ApplicationDbContext>(options =>
                 options.UseSqlServer(connectionString));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = false;
            });

            using (var serviceProvider = services.BuildServiceProvider())
            {
                using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
                    context.Database.Migrate();

                    // Role
                    var roleMgr = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                    var adminManager = roleMgr.FindByNameAsync("adminManager").Result;
                    if (adminManager == null)
                    {
                        adminManager = new IdentityRole
                        {
                            Name = "adminManager"
                        };
                        var result = roleMgr.CreateAsync(adminManager).Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }
                    }
                    else
                    {
                        Log.Debug("Role:adminManager already exists");
                    }

                    var adminEmployee = roleMgr.FindByNameAsync("adminEmployee").Result;
                    if (adminEmployee == null)
                    {
                        adminEmployee = new IdentityRole
                        {
                            Name = "adminEmployee"
                        };
                        var result = roleMgr.CreateAsync(adminEmployee).Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }
                    }
                    else
                    {
                        Log.Debug("Role:adminEmployee already exists");
                    }

                    // User and UserRole
                    var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                    
                    var brightasia = userMgr.FindByNameAsync("brightasia").Result;
                    if (brightasia == null)
                    {
                        brightasia = new ApplicationUser
                        {
                            UserName = "brightasia",
                            Email = "rd@brightasia.net",
                            EmailConfirmed = true,
                           
                        };
                        var result = userMgr.CreateAsync(brightasia, "richviet").Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        result = userMgr.AddClaimsAsync(brightasia, new Claim[]{
                            new Claim(JwtClaimTypes.Name, "brightasia"),
                            new Claim(JwtClaimTypes.GivenName, "brightasia"),
                            new Claim(JwtClaimTypes.FamilyName, "brightasia"),
                            new Claim(JwtClaimTypes.WebSite, "https://brightasia.net"),
                        }).Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }
                        userMgr.AddToRoleAsync(brightasia, "adminManager").Wait();
                        userMgr.AddToRoleAsync(brightasia, "adminEmployee").Wait();
                        Log.Debug("brightasia created");
                    }
                    else
                    {
                        Log.Debug("brightasia already exists");
                    }

                    
                }
            }
        }
    }
}
