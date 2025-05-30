﻿using AMS.MODELS.MODELS.SettingModels.Identity.Jwt;
using AMS.SHARED.Exceptions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace AMS.Extensions
{
    public class ConfigureJwtBearerOptions : IConfigureNamedOptions<JwtBearerOptions>
    {
        private readonly JwtSettings _jwtSettings;

        public ConfigureJwtBearerOptions(IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }

        public void Configure(JwtBearerOptions options)
        {
            Configure(string.Empty, options);
        }

        public void Configure(string? name, JwtBearerOptions options)
        {
            if (name != JwtBearerDefaults.AuthenticationScheme)
            {
                return;
            }

            byte[] key = Encoding.ASCII.GetBytes(_jwtSettings.Key);

            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateLifetime = true,
                ValidateAudience = false,
                RoleClaimType = ClaimTypes.Role,
                ClockSkew = TimeSpan.Zero
            };
            options.Events = new JwtBearerEvents
            {
                OnChallenge = context =>
                {
                    context.HandleResponse();
                    if (!context.Response.HasStarted)
                    {
                        throw new UnauthorizedException("Authentication Failed.");
                    }

                    return Task.CompletedTask;
                },
                OnForbidden = _ => throw new ForbiddenException("You are not authorized to access this resource."),
                OnMessageReceived = context =>
                {
                    var token = context.Request.Cookies["token"];

                    if (!string.IsNullOrEmpty(token))
                    {
                        // Read the token out of the query string
                        context.Token = token;
                    }

                    return Task.CompletedTask;
                }
            };
        }
    }
}
