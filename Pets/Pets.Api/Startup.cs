using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Pets.Core.Repositories;
using Pets.Infrastructure.Repositories;
using Pets.Infrastructure.Services;
using Pets.Infrastructure.EF;
using Pets.Infrastructure.Settings;
using Microsoft.AspNetCore.Http;
using Pets.Api.Framework;
using Swashbuckle.AspNetCore.Swagger;

namespace Pets.Api
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddEntityFrameworkSqlServer()
                .AddEntityFrameworkInMemoryDatabase()
                .AddDbContext<PetsContext>();

            // Register the IConfiguration instance which MyOptions binds against.
            services.Configure<MyOptions>(Configuration);

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();

            services.AddScoped<IAnimalRepository, AnimalRepository>();
            services.AddScoped<IAnimalService, AnimalService>();

            services.AddScoped<IAnimalToCareRepository, AnimalToCareRepository>();
            services.AddScoped<IAnimalToCareService, AnimalToCareService>();

            services.AddScoped<IEncrypter, Encrypter>();

            services.AddMvc();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", 
                    new Info
                    {
                        Version = "v1",
                        Title = "Pets API",
                        Description = "The API allows you to take care of someone's pet in some period of time when the owner can't.",
                        Contact = new Contact { Name = "Paweł Kowalewicz", Email = "kowalewicz.pawel@gmail.com", Url = "http://www.pawelkowalewicz.pl"}
                    });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseCookieAuthentication(new CookieAuthenticationOptions()
            {
                AuthenticationScheme = "CookieAuthentication",
                LoginPath = new PathString("/users/login"),
                AutomaticAuthenticate = true,
                AutomaticChallenge = true
            });

            app.UseExceptionHandlerCustom();

            app.UseMvc();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Pets API");
            });
        }
    }
}
