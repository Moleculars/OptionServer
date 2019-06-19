using Bb.Middlewares;
using Bb.OptionServer;
using Bb.Security.Jwt;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Data.Common;
using System.IO;

namespace Bb.OptionService
{
    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            _useSwagger = configuration.GetValue<bool>(ServiceConstants.UseSwagger);
        }

        public IConfiguration Configuration { get; }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            RegisterManager(services);
            RegisterJwtConfiguration(services);

            services
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

        }



        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app
                .UseMiddleware<LoggingCatchMiddleware>()
                .UseMiddleware<LoggingSupervisionMiddleware>()
                .UseMiddleware<ReadTokenMiddleware>()
                ;

            if (_useSwagger)
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    string path = @"/" + Path.Combine("swagger", ServiceConstants.VersionUmber, "swagger.json").Replace("\\", "/");
                    c.SwaggerEndpoint(path, ServiceConstants.SawggerName);
                    c.DefaultModelsExpandDepth(0);
                });

            }

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });




        }


        public class Connection
        {

            public string ConnectionName { get; set; }

            public string ProviderInvariantName { get; set; }

        }

        private void RegisterManager(IServiceCollection services)
        {

            var cnx = new Connection();
            Configuration.Bind("Connection", cnx);

            SqlManagerConfiguration sqlManagerConfiguration = RegisterConnexion(cnx);

            services.Add(ServiceDescriptor.Singleton(typeof(SqlManagerConfiguration), sqlManagerConfiguration));
            services.Add(ServiceDescriptor.Transient(typeof(SqlManager), typeof(SqlManager)));
            services.Add(ServiceDescriptor.Transient(typeof(DtoSqlManager), typeof(DtoSqlManager)));
            services.Add(ServiceDescriptor.Transient(typeof(OptionServices), typeof(OptionServices)));

            if (_useSwagger)
            {

                services.AddSwaggerGen(swagger =>
                {
                    swagger.DescribeAllEnumsAsStrings();
                    swagger.DescribeAllParametersInCamelCase();
                    swagger.IgnoreObsoleteActions();
                    swagger.AddSecurityDefinition(ServiceConstants.Key, new Swashbuckle.AspNetCore.Swagger.ApiKeyScheme { Name = ServiceConstants.ApiKey });

                    //swagger.TagActionsBy(a => a.ActionDescriptor is Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor b
                    //    ? b.ControllerTypeInfo.Assembly.FullName.Split('.')[2].Split(',')[0].Replace("Web", "")
                    //    : a.ActionDescriptor.DisplayName
                    //);

                    //swagger.DocInclusionPredicate((f, a) =>
                    //{
                    //    return a.ActionDescriptor is ControllerActionDescriptor b && b.MethodInfo.GetCustomAttributes<ExternalApiRouteAttribute>().Any();
                    //});

                    swagger.SwaggerDoc(ServiceConstants.VersionUmber, new Swashbuckle.AspNetCore.Swagger.Info
                    {
                        Title = ServiceConstants.Title,
                        License = new License() { Name = ServiceConstants.LicenceName },
                        Version = ServiceConstants.Version,
                    });

                    //var doc = DocumentationHelpers.ConcateDocumentations(ServiceConstants.AssemblyDocumentations);
                    //if (doc != null)
                    //    swagger.IncludeXmlComments(() => doc);

                });
            }

        }

        private SqlManagerConfiguration RegisterConnexion(Connection cnx)
        {

            DbProviderFactory factory = null;
            Func<SqlManager, IQueryGenerator> func = null;

            switch (cnx.ProviderInvariantName)
            {

                case "SqlClientFactory":
                    factory = System.Data.SqlClient.SqlClientFactory.Instance;
                    func = c => new SqlServerQueryGenerator(c);
                    break;

                default:
                    throw new Exception($"failed to intialize connection to '{cnx.ProviderInvariantName}'");

            }



            DbProviderFactories.RegisterFactory(cnx.ProviderInvariantName, factory);

            var sqlManagerConfiguration = new SqlManagerConfiguration()
            {
                ProviderInvariantName = cnx.ProviderInvariantName,
                ConnectionString = Configuration.GetConnectionString(cnx.ConnectionName),
                QueryManager = func,
            };

            return sqlManagerConfiguration;

        }

        private void RegisterJwtConfiguration(IServiceCollection services)
        {
            var jwtConfiguration = new JwtTokenConfiguration();
            Configuration.Bind("TokenConfiguration", jwtConfiguration);
            services.Add(ServiceDescriptor.Singleton(typeof(JwtTokenConfiguration), jwtConfiguration));
        }

        private readonly bool _useSwagger;

    }


    public static class ServiceConstants
    {

        public static string[] AssemblyDocumentations =
        {
             "Black.Beard*.xml"
        };

        public static string Version = "1.0.0";
        public static string VersionUmber = "v" + Version.Split('.')[0];

        public static string SawggerName = "My First Swagger Environment";
        public static string LicenceName = "Only usable with a valid partner contract.";
        public static string Title = "Black Beard Action APIs";

        public static string ApiKey = "ApiKey";
        public static string Key = "key";
        public static string UseSwagger = "useSwagger";
        public static string Namespace = "Namespace";


    }

}
