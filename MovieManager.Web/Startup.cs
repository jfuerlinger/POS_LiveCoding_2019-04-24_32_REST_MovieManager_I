using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MovieManager.Core.Contracts;
using MovieManager.Persistence;
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddScoped<IUnitOfWork, UnitOfWork>(
                serviceProvider => new UnitOfWork());

            services.AddSwaggerGen(configuration =>
                {
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
                        });
                }
            );
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();


            app.UseSwagger();
            app.UseSwaggerUI(configuration =>
            {
                configuration.SwaggerEndpoint("/swagger/v1/swagger.json", "Movie Manager API V1");
                //configuration.RoutePrefix = string.Empty;
            });

            app.UseMvc();
        }
    }
}
