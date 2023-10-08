using ABP.AppCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using ABP.Application.Profiles;
using ABP.Repository.ContractImplementation.EFImplementations;
using ABP.Repository.Repository.EFRepositories;
using ABP.Infrastructure.Services.EF;
using ABP.Infrastructure.Services.EF.Implementation;
using ABP.Repository.ContractImplementation.DirectImplementations;
using ABP.Repository.Repository.DirectRepositories;
using ABP.Infrastructure.Services.Direct;
using ABP.Infrastructure.Services.Direct.Implementation;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Http;

namespace ABP_Server
{
    public class Startup
    {
        IWebHostEnvironment _env;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            _env = env; 
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            string connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<ABPContext>(options => options.UseSqlServer(connection));

            services.AddScoped<IDeviceEFRepository, DeviceEFRepository>();
            services.AddScoped<IDeviceTokenEFRepository, DeviceTokenEFRepository>();
            services.AddScoped<IExperimentEFRepository, ExperimentEFRepository>();
            services.AddScoped<IDeviceEFService, DeviceEFService>();
            services.AddScoped<IExperimentEFService, ExperimentEFService>();

            services.AddScoped<IDeviceDirectRepository, DeviceDirectRepository>();
            services.AddScoped<IDeviceTokenDirectRepository, DeviceTokenDirectRepository>();
            services.AddScoped<IExperimentDirectRepository, ExperimentDirectRepository>();
            services.AddScoped<IDeviceDirectService, DeviceDirectService>();
            services.AddScoped<IExperimentDirectService, ExperimentDirectService>();

            services.AddSingleton<IConfiguration>(Configuration);

            services.AddAutoMapper(typeof(DeviceProfile), typeof(ExperimentProfile));

            services.AddControllersWithViews()
                .AddNewtonsoftJson(options =>
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            services.AddControllers();
            services.AddSwaggerGen();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
                c.OAuthClientId("swagger-ui");
                c.OAuthRealm("swagger-ui-realm");
                c.OAuthAppName("Swagger UI");
            });

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=ExperimentEF}/{action=Index}/{id?}");
            });
        }
    }
}
