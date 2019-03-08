using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using log4net;
using log4net.Config;
using log4net.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using NLog.Extensions.Logging;
using NLog.Web;
using Swashbuckle.AspNetCore.Swagger;
using Test.Core.Filter;
using Test.Core.IOC;
using Test.Domain;
using Test.Domain.Infrastructure;
using Test.Domain.IOC;
using Test.Service.Impl;
using Test.Service.Infrastructure;
using Test.Service.Interface;
using Test.Service.IOC;
using Test.Web.Filter;

namespace Test.Web
{
    public class Startup
    {
        public static ILoggerRepository loggerRepository { get; set; }
        public IConfiguration Configuration { get; }
        //public Startup(IConfiguration configuration)
        //{
        //    Configuration = configuration;
        //}
        public Startup(IHostingEnvironment env)
        {
            Configuration = new ConfigurationBuilder().SetBasePath(env.ContentRootPath).AddJsonFile("appsettings.json").Build();

            loggerRepository = LogManager.CreateRepository("NETCoreRepository");
            XmlConfigurator.Configure(loggerRepository, new FileInfo(Environment.CurrentDirectory + @"\Config\log4net.config"));
        }
        public Autofac.IContainer ApplicationContainer { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<TestDBContext>();

            //注册AutoMapper
            services.AddAutoMapper();

            services.AddMvcCore().AddAuthorization().AddJsonFormatters();

            services.AddAuthentication(Configuration["Identity:Scheme"]).AddIdentityServerAuthentication(option =>
            {
                option.RequireHttpsMetadata = false;
                option.Authority = Configuration["Identity:Url"];
                option.ApiName = Configuration["Service:Name"];
            });

            services.AddCors(option =>
            {
                option.AddPolicy("AllowAllOrigins",
                    policyBuilder =>
                    {
                        policyBuilder
                            .AllowAnyOrigin()
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            });

            //Add Swagger
            services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "Test"
                });
                var xmlFilePaths = new List<string>() {
                    "Test.Web.xml",
                    "Test.Service.xml"
                };
                foreach (var filePath in xmlFilePaths)
                {
                    var xmlFilePath = Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, filePath);
                    if (File.Exists(xmlFilePath))
                    {
                        option.IncludeXmlComments(xmlFilePath);
                    }
                }
                //TryAddAuthorization
                //option.AddSecurityDefinition(Configuration["Identity:Scheme"], new ApiKeyScheme()
                //{
                //    Description = "JWT Bearer 授权 \"Authorization:     Bearer+空格+token\"",
                //    Name = "Authorization",
                //    In = "header",
                //    Type = "apiKey"
                //});
            });

            services.AddMvc(option =>
            {
                //option.Filters.Add(typeof(GlobalExceptions));
            });

            //DI Injection
            //services.AddScoped<IArticleSvc, ArticleSvc>();
            //services.AddScoped<ICommentSvc, CommentSvc>();
            var builder = new ContainerBuilder();

            //var baseType = typeof(IDependency);
            //var assembly = Assembly.GetEntryAssembly();
            //builder.RegisterAssemblyTypes(assembly)
            //                .Where(t => baseType.IsAssignableFrom(t) && t != baseType)
            //                .AsImplementedInterfaces().InstancePerLifetimeScope();

            //SingleService Injection
            //builder.RegisterType<ArticleSvc>().As<IArticleSvc>().InstancePerLifetimeScope();
            //builder.RegisterType<CommentSvc>().As<ICommentSvc>().InstancePerLifetimeScope();

            //Attribute&Filter Injection
            builder.RegisterType<CustomerExceptionFilter>();/*NLogFilterAttribute*/
            //Module Injection
            builder.RegisterModule<UtilModule>();
            builder.RegisterModule<DomainServiceModule>();
            builder.RegisterModule<ServiceModule>();

            builder.Populate(services);
            ApplicationContainer = builder.Build();
            return new AutofacServiceProvider(ApplicationContainer);
        }

        //public void ConfigureServices(IServiceCollection services)
        //{
        //    //UseDbContext
        //    services.AddDbContext<TestDBContext>(option => option.UseLazyLoadingProxies().ConfigureWarnings(action => action.Ignore(CoreEventId.DetachedLazyLoadingWarning)).UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

        //    services.AddDbContext<TestDBContext>();

        //    //UseDbContextPool
        //    //services.AddDbContextPool<TestDBContext>(option => option.UseLazyLoadingProxies().ConfigureWarnings(action => action.Ignore(CoreEventId.DetachedLazyLoadingWarning)).UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

        //    //DI Injection
        //    services.AddScoped<IArticleSvc, ArticleSvc>();
        //    services.AddScoped<ICommentSvc, CommentSvc>();

        //    //注册AutoMapper
        //    services.AddAutoMapper();
            
        //    //Add Swagger
        //    services.AddSwaggerGen(c =>
        //    {
        //        c.SwaggerDoc("v1", new Info
        //        {
        //            Version = "v1",
        //            Title = "Test"
        //        });
        //        var xmlFilePaths = new List<string>() {
        //            "Test.Web.xml",
        //            "Test.Service.xml"
        //        };
        //        foreach (var filePath in xmlFilePaths)
        //        {
        //            var xmlFilePath = Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, filePath);
        //            if (File.Exists(xmlFilePath))
        //            {
        //                c.IncludeXmlComments(xmlFilePath);
        //            }
        //        }
        //    });

        //    services.AddMvc();
        //}

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"))
                .AddNLog()
                .AddDebug();
            env.ConfigureNLog(Path.Combine(env.ContentRootPath, "nlog.config"));
            //loggerFactory.AddNLog();
            //env.ConfigureNLog("nlog.config");
            //if (env.IsDevelopment())
            //{
            //    //app.UseBrowserLink();
            //    app.UseDeveloperExceptionPage();
            //}
            //else
            //{
            //    app.UseExceptionHandler("/Home/Error");
            //}

            app.UseStaticFiles();

            //Init AutoMapper,Add Profile
            Mapper.Initialize(x => x.AddProfile<CustomizeProfile>());

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Test");
            });

            app.UseAuthentication();
            app.UseCors("AllowAllOrigins");

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
