using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Chat.Api.ActionFilters;
using Chat.Api.Hubs;
using Chat.Api.Mappings;
using Chat.Core.AppContext;
using Chat.Core.Entity;
using Chat.Core.TokenModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Chat.Api
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
            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.AllowAnyHeader()
                .AllowAnyMethod()
                .SetIsOriginAllowed((host) => true)
                .AllowCredentials();
            }));
            AddDependencies(services);
            services.AddControllers(options => options.EnableEndpointRouting = false);
            AddSwagger(services);
            services.AddSwaggerGen();
            AddMVC(services);
            services.AddDbContext<JobsityChatContext>(options =>
               options.UseSqlServer(this.Configuration.GetConnectionString("DefaultConnection")), ServiceLifetime.Scoped);
            services.AddIdentity<ApplicationUser, ApplicationRole>()
             .AddEntityFrameworkStores<JobsityChatContext>()
             .AddDefaultTokenProviders();
            AddJWT(services);
            services.AddSingleton(provider => new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            }).CreateMapper());
            services.AddSignalR();
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors("MyPolicy");
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            //app.UseMvc(option=> option.EnableEndpointRouting = false).AddNewtonsoftJson();
            app.UseHttpsRedirection();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Api API V1");
            });
            app.UseRouting();
            app.UseAuthentication();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<ChatHub>("api/chat");
            });
            app.UseMvc();
            
        }
        private void AddSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Api",
                    Description = "Api  Web API",
                    TermsOfService = new Uri("https://example.com/terms"),

                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme { In = ParameterLocation.Header, Description = "Please enter JWT with Bearer into field", Name = "Authorization", Type = SecuritySchemeType.ApiKey });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,

                        },
                        new List<string>()
                    }
                });

                //c.CustomSchemaIds(x => x.FullName);
            });
        }
        private void AddMVC(IServiceCollection services)
        {
            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(GlobalExceptionAttribute));
            });
        }

        private void AddDependencies(IServiceCollection services)
        {
            var dependencyInjection = new DependencyInjection();
            dependencyInjection.Map(services, this.Configuration);
        }

        private void AddJWT(IServiceCollection services)
        {
            var jwtAppSettingOptions = Configuration.GetSection(nameof(JwtOptions));
            string secretKey = jwtAppSettingOptions["TokenSecretKey"];
            SymmetricSecurityKey _signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));

            services.Configure<JwtOptions>(options =>
            {
                options.Issuer = jwtAppSettingOptions[nameof(JwtOptions.Issuer)];
                options.Audience = jwtAppSettingOptions[nameof(JwtOptions.Audience)];
                options.SigningCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256);
                options.ValidFor = TimeSpan.FromMinutes(Convert.ToDouble(jwtAppSettingOptions[nameof(JwtOptions.ValidFor)]));
                options.TokenSecretKey = jwtAppSettingOptions[nameof(JwtOptions.TokenSecretKey)];
                options.ExpirationDays = jwtAppSettingOptions[nameof(JwtOptions.ExpirationDays)];
            });

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtAppSettingOptions[nameof(JwtOptions.Issuer)],

                ValidateAudience = true,
                ValidAudience = jwtAppSettingOptions[nameof(JwtOptions.Audience)],

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _signingKey,

                RequireExpirationTime = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(configureOptions =>
            {
                configureOptions.ClaimsIssuer = jwtAppSettingOptions[nameof(JwtOptions.Issuer)];
                configureOptions.TokenValidationParameters = tokenValidationParameters;
                configureOptions.SaveToken = true;
            });
        }
    }
}
