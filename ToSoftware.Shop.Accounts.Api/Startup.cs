using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ToSoftware.Shop.Accounts.Api.Data.Repositories;
using ToSoftware.Shop.Accounts.Api.Domain;
using ToSoftware.Shop.Accounts.Api.Domain.Repositories.Contracts;
using ToSoftware.Shop.Accounts.Api.Domain.Services;
using ToSoftware.Shop.Accounts.Api.Domain.Services.Contracts;
using ToSoftware.Shop.Accounts.Api.Extensions;

namespace ToSoftware.Shop.Accounts.Api
{
    public class Startup
    {
        IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.Configure<NoSQLSettings>(Configuration.GetSection(nameof(NoSQLSettings)))
                .Configure<JWTSettings>(Configuration.GetSection(nameof(JWTSettings)));

            services.AddTransient<IUserService, UserService>()
                .AddTransient<IUserRepository, UserRepository>();

            services.AddJWTAuthentication(Configuration.GetSection("JWTSettings:Secret").Value);

            services.AddHttpContextAccessor();

            services.AddCors();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true)
                .AllowCredentials());

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}