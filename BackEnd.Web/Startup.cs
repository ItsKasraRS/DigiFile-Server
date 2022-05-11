using System.Text;
using BackEnd.Core.Interfaces;
using BackEnd.Core.Services;
using BackEnd.DataLayer.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace BackEnd.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            #region Controller views
            services.AddControllersWithViews()
                .AddNewtonsoftJson(options =>
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                );

            #endregion

            #region Database

            services.AddDbContext<SiteContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DigiShopConnection"));
            });

            #endregion

            #region IoC

            services.AddScoped<IViewRenderService, RenderViewToString>();
            services.AddScoped<IUserService, UserService>(); services.AddScoped<IAdminUserService, AdminUserService>();
            services.AddScoped<IProductService, ProductService>(); services.AddScoped<IAdminProductService, AdminProductService>();
            services.AddScoped<ICategoryService, CategoryService>(); services.AddScoped<IAdminCategoryService, AdminCategoryService>();
            
            #endregion

            services.AddMvcCore()
                .AddAuthorization();
            services.AddAuthorization();

            #region Authentication

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "https://localhost:44308/",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("AngularDigiShopJwtBearer344783"))
                };
            });

            #endregion

            #region Cors

            services.AddCors(options =>
            {
                options.AddPolicy("DigiShopSecretCors_344783", builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            });

            #endregion
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            app.UseStaticFiles();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("DigiShopSecretCors_344783");

            app.UseAuthentication();
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
