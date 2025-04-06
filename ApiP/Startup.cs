using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiP.Services;
using ApiP.Data;
using ApiP.Email;
using ApiP.Middleware;
using ApiP.Models;
using ApiP.Models.Valid;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using FluentValidation;
using FluentValidation.AspNetCore;

namespace ApiP
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
            var authenticationSettings = new AuthencticationSettings();
            Configuration.GetSection("Authentication").Bind(authenticationSettings);

            services.AddSingleton(authenticationSettings);
            services.AddAuthentication(option=> {
                option.DefaultAuthenticateScheme = "Bearer";
                option.DefaultScheme = "Bearer";
                option.DefaultChallengeScheme = "Bearer";
            }).AddJwtBearer(cfg=> {
                cfg.RequireHttpsMetadata = false;
                cfg.SaveToken = true;
                cfg.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = authenticationSettings.JwtIssuer,
                    ValidAudience = authenticationSettings.JwtIssuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.JwtKey)),
                };
            });

            services.AddControllers();


            services.AddTransient<IEmailSender, MailKitEmailSender>();
            services.Configure<MailKitEmailSenderOptions>(options =>
            {
                options.Host_Address = "smtp.ethereal.email";
                options.Host_Port = 587;
                options.Host_Username = "izabella.greenholt2@ethereal.email";
                options.Host_Password = "x3tUGBtX921scd7gZK";
                options.Sender_EMail = "izabella.greenholt2@ethereal.email";
                options.Sender_Name = "Policja123Test";
            });


            services.AddAutoMapper(this.GetType().Assembly);
            services.AddControllers().AddFluentValidation();
            services.AddDbContext<MainDbContext>();
            services.AddScoped<DbMigration>();
            services.AddScoped<IMainService, MainService>();
            services.AddScoped<IServerService, ServerService>();
            services.AddScoped<IUsersService, UsersService>();
            services.AddScoped<IAppUsersService, AppUsersService>();
            services.AddScoped<IPrimaryService, PrimaryService>();
            services.AddScoped<IValidator<RegisterDto>, RegisterValidator>();
            services.AddScoped<IPasswordHasher<Users>, PasswordHasher<Users>>();
            services.AddScoped<IPasswordHasher<AppUsers>, PasswordHasher<AppUsers>>();
            services.AddScoped<ErrorMiddleware>();
            services.AddScoped<IValidator<Query>, PageValidator>();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ApiP", Version = "v1" });
            });
            services.AddCors(options => { //polityka corse
                options.AddPolicy("FrontEndClient", builder => builder
                .AllowAnyMethod()
                .AllowAnyHeader()
                .WithOrigins(Configuration["AllowedOrigins"])); //odwolanie do wartosci w appsetings.json
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,DbMigration _seeder)
        {
            _seeder.Seed();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiP v1"));
            }
            app.UseMiddleware<ErrorMiddleware>();
            app.UseAuthentication();
            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors("FrontEndClient");

            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}/*
const transporter = nodemailer.createTransport({
    host: 'smtp.ethereal.email',
    port: 587,
    auth: {
        user: 'izabella.greenholt2@ethereal.email',
        pass: 'x3tUGBtX921scd7gZK'
    }
});*/