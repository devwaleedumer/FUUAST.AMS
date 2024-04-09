using AMS.Authorization.Permissons;
using AMS.DATA;
using AMS.DOMAIN.Identity;
using AMS.Interfaces.Mail;
using AMS.Middlewares;
using AMS.MODELS.MODELS.SettingModels.Identity.Jwt;
using AMS.MODELS.MODELS.SettingModels.Identity.User;
using AMS.Services.CurrentUser;
using AMS.Services.MailService;
using AMS.SERVICES.EmailTemplateService;
using AMS.SERVICES.Identity.Interfaces;
using AMS.SERVICES.Identity.Services;
using AMS.SERVICES.MailService;
using AMS.SHARED.Interfaces.CurrentUser;
using AMS.VALIDATORS.Identity.Role;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace AMS.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IServiceCollection AddDatabaseServices(this IServiceCollection services, IWebHostEnvironment env, IConfiguration configuration)
        {
            if (env.IsDevelopment())
            {
                services.AddDbContext<AMSContext>(options =>
                {
                    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), o =>
                    {
                        o.CommandTimeout((int)TimeSpan.FromMinutes(2).TotalSeconds);
                        o.MigrationsAssembly(Assembly.GetAssembly(typeof(AMSContext))!.FullName);

                    });
                    options.EnableSensitiveDataLogging();
                    
                });
            }
            else
            {
                services.AddDbContext<AMSContext>(options =>
                {
                    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), o =>
                    {
                        o.CommandTimeout((int)TimeSpan.FromMinutes(2).TotalSeconds);
                        o.MigrationsAssembly(Assembly.GetAssembly(typeof(AMSContext))!.FullName);
                    });
                });
            }
            //services.AddTransient<IInitialDataSeeder, InitialDataSeeder>();
            return services;
        }

        public static IServiceCollection AddSwaggerService(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "AMS API",
                    Version = "v1",
                    Description = "API for Admission Management System",
                    TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "Waleed Umer",
                        Email = "dev.waleedumer@gmail.com",
                        Url = new Uri("https://github.com/devwaleedumer"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Properity",
                        Url = new Uri("https://example.com/license"),
                    }
                });
                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

            });

                return services;

        }
        public static IServiceCollection AddIdentity(this IServiceCollection services)
        {
            services.AddIdentity<ApplicationUser,ApplicationRole>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.User.RequireUniqueEmail = true;
                })
                    .AddEntityFrameworkStores<AMSContext>()
                    .AddDefaultTokenProviders();


            services.AddSingleton<IConfigureOptions<JwtBearerOptions>, ConfigureJwtBearerOptions>();

            services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            });

            services
          .AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>()
          .AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();

            // adding access policies
            //services.AddAuthorization(op =>
            //{
            //    op.AddModulePermissionPolicies();
            //    op.AddActionPermissionPolicies();
            //});

            //services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, ApplicationUserClaimsPrincipalFactory>();
            return services;
        }

        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            //services.Configure<CERPSettings>(configuration.GetSection(CERPSettings.SectionName));
            //services.AddSingleton(s => s.GetRequiredService<IOptions<CERPSettings>>().Value);
            services.AddOptions<SecuritySettings>()
                    .BindConfiguration(nameof(SecuritySettings)); 
            
            services.AddOptions<MailSettings>()
                    .BindConfiguration(nameof(MailSettings));

            services.AddOptions<JwtSettings>()
                    .BindConfiguration(nameof(JwtSettings))
                    .ValidateDataAnnotations()
                    .ValidateOnStart();
            



            services.AddSingleton<IConfigureOptions<JwtBearerOptions>, ConfigureJwtBearerOptions>();
            services.AddSingleton(resolver => resolver.GetRequiredService<IOptions<SecuritySettings>>().Value);
            services.AddSingleton(resolver => resolver.GetRequiredService<IOptions<MailSettings>>().Value);
            services.AddSingleton(resolver => resolver.GetRequiredService<IOptions<JwtSettings>>().Value);


            // Add application services like general services.  
            services.AddTransient<IMailService, SmtpMailService>();
            services.AddTransient<IEmailTemplateService, EmailTemplateService>();

            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IRoleService, RoleService>();
            services.AddTransient<ITokenService, TokenService>();

            ////added from Services.Extensions
            //services.AddDataServices();

            return services;
        }

        public static IServiceCollection AddExceptionMiddleware(this IServiceCollection services) =>
    services.AddScoped<ExceptionMiddleware>();
#pragma warning disable CS0618 // Type or member is obsolete
        public static IServiceCollection AddFluentValidation(this IServiceCollection services) =>
          services.AddFluentValidation(config =>
          {
              config.RegisterValidatorsFromAssembly(Assembly.GetAssembly(typeof(CreateOrUpdateRoleRequestValidator)));
              config.AutomaticValidationEnabled = true;

          });
#pragma warning restore CS0618 // Type or member is obsolete
        public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder app) =>
            app.UseMiddleware<ExceptionMiddleware>();

        public static IServiceCollection AddCurrentUserService(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services
           .AddScoped<CurrentUserMiddleware>()
           .AddScoped<ICurrentUser, CurrentUser>()
           .AddScoped(sp => (ICurrentUserInitializer)sp.GetRequiredService<ICurrentUser>());
            return services;
        }
        public static IApplicationBuilder UseCurrentUser(this IApplicationBuilder app) =>
           app.UseMiddleware<CurrentUserMiddleware>();


        //public static IServiceCollection AddHangfireServices(this IServiceCollection services, IConfiguration configuration)
        //{
        //    var connString = configuration.GetConnectionString("DefaultConnection");
        //    services.AddHangfire(configuration => configuration
        //            .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
        //            .UseSimpleAssemblyNameTypeSerializer()
        //            .UseRecommendedSerializerSettings()
        //            .UseSimpleAssemblyNameTypeSerializer()
        //            .UseSqlServerStorage(connString, new SqlServerStorageOptions
        //            {
        //                CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
        //                SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
        //                QueuePollInterval = TimeSpan.Zero,
        //                UseRecommendedIsolationLevel = true,
        //                DisableGlobalLocks = true
        //            }));
        //    services.AddHangfireServer();
        //    return services;
        //}

        //public static void LoadAppSettings(IAppSettingsService appSettingsService, ILogger<Startup> logger)
        //{
        //    try
        //    {
        //        var setting = appSettingsService.GetAppSettings();
        //        AppSettingsProvider.BookingsPerSlot = setting.BookingsPerSlot;
        //        AppSettingsProvider.MinutesPerSlot = setting.MinutesPerSlot;
        //        AppSettingsProvider.SlotAvailabilityMinutes = setting.SlotAvailabilityMinutes;
        //        AppSettingsProvider.SlotSelectionExpiryMinutes = setting.SlotSelectionExpiryMinutes;
        //        AppSettingsProvider.NotificationsFromEmail = setting.NotificationsFromEmail;
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.LogError(ex, ex.Message);
        //        AppSettingsProvider.BookingsPerSlot = 10;
        //        AppSettingsProvider.MinutesPerSlot = 30;
        //        AppSettingsProvider.SlotAvailabilityMinutes = 5;
        //        AppSettingsProvider.SlotSelectionExpiryMinutes = 15;
        //        AppSettingsProvider.NotificationsFromEmail = "no-replay@wolverhampton.gov.uk";
        //    }
        //}


        //internal static IApplicationBuilder Initialize(this IApplicationBuilder app)
        //{
        //    using (var serviceScope = app.ApplicationServices.CreateScope())
        //    {
        //        var settings = serviceScope.ServiceProvider.GetService<CERPSettings>();
        //        if (settings != null && settings.DataSeederEnabled)
        //        {
        //            var initializers = serviceScope.ServiceProvider.GetServices<IInitialDataSeeder>();
        //            foreach (var initializer in initializers)
        //            {
        //                initializer.Initialize();
        //            }
        //        }
        //    }
        //    return app;
        //}
    }
}
