using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MovieManager.Core.Contracts;
using MovieManager.Persistence;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;

namespace MovieManager.Web
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
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.ContractResolver =
                        new CamelCasePropertyNamesContractResolver();
                    options.SerializerSettings.PreserveReferencesHandling =
                        PreserveReferencesHandling.Objects;
                });

            services.AddScoped<IUnitOfWork, UnitOfWork>(
                serviceProvider => new UnitOfWork());

            services.AddSwaggerGen(configuration =>
                configuration.SwaggerDoc(
                    "v1", new Info()
                    {
                        Title = "Movie Manager API",
                        Version = "v1",
                        Contact = new Contact()
                        {
                            Name = "Josef Fürlinger",
                            Email = "j.fuerlinger@htl-leonding.ac.at",
                            Url = "https://github.com/jfuerlinger"
                        }
                    }));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();

            app.UseSwagger();
            app.UseSwaggerUI(configuration =>
            {
                configuration.SwaggerEndpoint("/swagger/v1/swagger.json", "MovieManager API V1");
                configuration.RoutePrefix = string.Empty;
            });

        }
    }
}
