using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_AuthTest_Jwt_Header
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
            services.AddCors();
            // jwt 인증에 사용될 secret key
            var key = Encoding.ASCII.GetBytes("aaaaaaaaaaaaaaaaaaaa");
            // 인증 스킴 설정
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            // 인증에 사용될 jwtBearer 설정
            .AddJwtBearer(x =>
            {
                x.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context =>
                    {
                        return Task.CompletedTask;
                    }
                };
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
            // 권한 검증 정책 설정
            services.AddAuthorization(options =>
            {
                // 여러 정책 추가 가능
                // 여기에 등록한 정책 이름을 각 컨트롤러에 적용
                // 토큰을 파싱했을때 토큰에 ("Member", "true") claim이 있어야 한다는 정책
                options.AddPolicy("MemberOnly", policy => policy.RequireClaim("Member", "true"));
                // 토큰에 ("Member", "true") claim도 있고 ("Admin", "true") claim도 있어야 한다는 정책
                options.AddPolicy("AdminOnly", policy => policy
                .RequireClaim("Member", "true")
                .RequireClaim("Admin", "true")
                );
            });

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API_AuthTest_Jwt_Header", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API_AuthTest_Jwt_Header v1"));
            }

            app.UseRouting();

            // cors 설정
            app.UseCors(builder => builder
                .AllowAnyHeader()
                .AllowAnyMethod()
                .SetIsOriginAllowed(origin => true)
                .AllowCredentials()
                .WithExposedHeaders("access_token")
                );

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
