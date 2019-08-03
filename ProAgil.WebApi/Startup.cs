﻿using System.IO;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using ProAgil.Domain.Identity;
using ProAgil.Repository;
using ProAgil.WebApi.Helpers;

namespace ProAgil.WebApi
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
            services.AddDbContext<ProAgilContext>(
                options => options.UseMySql(Configuration.GetConnectionString("MySqlConnection"))
            );
            services.AddScoped<IProAgilRepository,ProAgilRepository>();
            
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AutoMappingProfiles());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            IdentityBuilder builder = services.AddIdentityCore<User>( options => 
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequiredLength = 4;
                }
            );
            
            builder = new IdentityBuilder(builder.UserType,typeof(Role),builder.Services);
            builder.AddEntityFrameworkStores<ProAgilContext>();
            builder.AddRoleManager<RoleManager<Role>>();
            builder.AddRoleValidator<RoleValidator<Role>>();
            builder.AddSignInManager<SignInManager<User>>();
            
            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer( options => 
                        {                            
                            var key = Encoding.UTF8.GetBytes(Configuration.GetSection("AppSettings:Token").Value);
                            var tokenValidationParameters = new TokenValidationParameters();
                            
                            tokenValidationParameters.ValidateIssuerSigningKey = true;
                            tokenValidationParameters.IssuerSigningKey = new SymmetricSecurityKey(key);
                            tokenValidationParameters.ValidateIssuer = false;
                            tokenValidationParameters.ValidateAudience = false;
                            options.TokenValidationParameters = tokenValidationParameters;
                        }
                    );

            services
                .AddMvc( options => 
                    {
                        var policy = new AuthorizationPolicyBuilder()
                            .RequireAuthenticatedUser()
                            .Build();
                        
                        options.Filters.Add(new AuthorizeFilter(policy));
                    }
                )
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddJsonOptions(options => 
                    {
                        //Set date configurations
                        options
                            .SerializerSettings
                            .DateTimeZoneHandling = DateTimeZoneHandling.Local;

                        options
                            .SerializerSettings
                            .ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                    }
                );

            services.AddCors();
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
            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
            app.UseStaticFiles();
            app.UseStaticFiles( new StaticFileOptions(){
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(),@"Resources")),
                RequestPath = new PathString("/Resources")
            });
            app.UseMvc();
        }
    }
}
