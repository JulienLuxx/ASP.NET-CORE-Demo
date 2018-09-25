﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Test.Core.IOC;
using Test.IdentityServer.Config;
using Test.IdentityServer.ValidatorAndProfile;
using Test.Service.IOC;

namespace Test.IdentityServer
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Autofac.IContainer ApplicationContainer { get; private set; }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            IdentityConfiguration.Configuration = Configuration;


            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddInMemoryIdentityResources(IdentityConfiguration.GetIdentityResources())
                .AddInMemoryApiResources(IdentityConfiguration.GetApiResources())
                .AddInMemoryClients(IdentityConfiguration.GetClients())
                .AddResourceOwnerValidator<ResourceOwnerPasswordValidator>()
                .AddProfileService<ProfileService>();

            var builder = new ContainerBuilder();
            builder.RegisterModule<UtilModule>();
            builder.RegisterModule<ServiceModule>();
            ApplicationContainer = builder.Build();
            return new AutofacServiceProvider(ApplicationContainer);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseIdentityServer();
            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();

            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync("Hello World!");
            //});
        }
    }
}
