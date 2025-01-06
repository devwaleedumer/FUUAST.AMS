using AMS.Authorization.Permissons;
using AMS.DATA;
using AMS.DatabaseSeed;
using AMS.DatabaseSeed.Seeds.AcademicYearSeeds;
using AMS.DatabaseSeed.Seeds.AdmissionSessionSeeds;
using AMS.DatabaseSeed.Seeds.DegreeLevelSeeds;
using AMS.DatabaseSeed.Seeds.DepartmentSeeds;
using AMS.DatabaseSeed.Seeds.FaculitSeeds;
using AMS.DatabaseSeed.Seeds.PreviousDegreeSeeds;
using AMS.DatabaseSeed.Seeds.ProgramDepartmentSeeds;
using AMS.DatabaseSeed.Seeds.ProgramSeeds;
using AMS.DatabaseSeed.Seeds.ProgramTypeSeeds;
using AMS.DatabaseSeed.Seeds.TimeShiftSeeds;
using AMS.DOMAIN.Identity;
using AMS.Interfaces.Mail;
using AMS.Middlewares;
using AMS.MODELS.MODELS.SettingModels.Identity.Jwt;
using AMS.MODELS.MODELS.SettingModels.Identity.User;
using AMS.MODELS.SettingModels.AppSettings;
using AMS.MODELS.SettingModels.FileStorage;
using AMS.Security;
using AMS.Services.CurrentUser;
using AMS.Services.Hangfire;
using AMS.Services.MailService;
using AMS.SERVICES.DataService;
using AMS.SERVICES.EmailTemplateService;
using AMS.SERVICES.IDataService;
using AMS.SERVICES.Identity.Interfaces;
using AMS.SERVICES.Identity.Services;
using AMS.SERVICES.MailService;
using AMS.SHARED.Interfaces.CurrentUser;
using AMS.SHARED.Interfaces.Hangfire;
using AMS.SHARED.Validator;
using AMS.VALIDATORS.Identity.Role;
using FluentValidation.AspNetCore;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System.Reflection;
using AMS.SERVICES.Dapper;
using AMS.VALIDATORS.Identity.User;

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
                services.AddScoped<IDapperService, DapperService>();

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
            services.AddScoped<IDapperService, DapperService>();

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
                        Name = "FUUAST Admissions developer's Team",
                        Email = "dev.admission@fuuastisb.edu.pk",
                        Url = new Uri("https://github.com/FUUAST.AMS"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Properity",
                        Url = new Uri("https://example.com/license"),
                    }
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
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
            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
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
            }).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, null!);

            services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>()
            .AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();

            //services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, ApplicationUserClaimsPrincipalFactory>();
            return services;
        }
        public static IServiceCollection AddDataSeeder(this IServiceCollection services)
        {
            // should execute sequentially
            return services
            .AddTransient<ApplicationDbInitializer>()
            .AddTransient<ApplicationDbSeeder>()
            .AddTransient<ICustomSeeder, FaculitySeeds>()
            .AddTransient<ICustomSeeder, ProgramTypeSeeds>()
            .AddTransient<ICustomSeeder, DepartmentSeeds>()
            .AddTransient<ICustomSeeder, ProgramSeeds>()
            .AddTransient<ICustomSeeder, DegreeLevelSeeds>()
            .AddTransient<ICustomSeeder, AcademicYearSeeds>()
            .AddTransient<ICustomSeeder, TimeShiftSeeds>()
            .AddTransient<ICustomSeeder, AdmissionSessionSeeds>()
            .AddTransient<ICustomSeeder, ProgramDepartmentSeeds>()
            .AddTransient<ICustomSeeder, DegreeGroupSeeds>()
            .AddTransient<ICustomSeeder, TestTypeSeeds>()
            .AddTransient<CustomSeederRunner>();
        }
        internal static IServiceCollection AddCorsPolicy(this IServiceCollection services, IConfiguration config)
        {
            var corsSetting = config.GetSection(nameof(CorsSettings)).Get<CorsSettings>();
            if (corsSetting is null) return services;
            List<string> origins = new();
            if (corsSetting.Angular is not null) origins.AddRange(corsSetting.Angular.Split(";", StringSplitOptions.RemoveEmptyEntries));
            if (corsSetting.Blazor is not null) origins.AddRange(corsSetting.Blazor.Split(";", StringSplitOptions.RemoveEmptyEntries));
            if (corsSetting.React is not null) origins.AddRange(corsSetting.React.Split(";", StringSplitOptions.RemoveEmptyEntries));
            return services.AddCors((options) =>
            {
                options.AddPolicy("CorsPolicy", policy =>
                {
                    policy.AllowAnyHeader()
                           .AllowAnyMethod()
                           .AllowCredentials()
                           .WithOrigins([.. origins]);
                });
            });
        }
        internal static IApplicationBuilder UseCorsPolicy(this IApplicationBuilder app) =>
    app.UseCors("CorsPolicy");
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions<SecuritySettings>()
           .BindConfiguration(nameof(SecuritySettings));

            services.AddOptions<MailSettings>()
                    .BindConfiguration(nameof(MailSettings));

            services.AddOptions<JwtSettings>()
                    .BindConfiguration(nameof(JwtSettings))
                    .ValidateDataAnnotations()
                    .ValidateOnStart();

            services.AddOptions<SuperAdminSettings>()
                  .BindConfiguration(nameof(SuperAdminSettings))
                  .ValidateDataAnnotations()
                  .ValidateOnStart();

            services.AddOptions<OriginOptions>()
                  .BindConfiguration(nameof(OriginOptions));
                 



            services.AddSingleton<IConfigureOptions<JwtBearerOptions>, ConfigureJwtBearerOptions>();
            services.AddSingleton(resolver => resolver.GetRequiredService<IOptions<SecuritySettings>>().Value);
            services.AddSingleton(resolver => resolver.GetRequiredService<IOptions<MailSettings>>().Value);
            services.AddSingleton(resolver => resolver.GetRequiredService<IOptions<JwtSettings>>().Value);
            services.AddSingleton(resolver => resolver.GetRequiredService<IOptions<SuperAdminSettings>>().Value);
            services.AddSingleton(resolver => resolver.GetRequiredService<IOptions<OriginOptions>>().Value);


            // Add application services like general services.  
            services.AddTransient<IMailService, SmtpMailService>();
            services.AddTransient<IEmailTemplateService, EmailTemplateService>();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IApplicantService, ApplicantService>();
            services.AddTransient<ILocalFileStorageService, LocalFileStorageService>();
            services.AddScoped<IProgramService, ProgramService>();
            services.AddScoped<IDegreeGroupService, DegreeGroupService>();
            services.AddScoped<IFacultyService, FacultyService>();
            services.AddScoped<IDepartmentService, DepartmentService>();
            services.AddScoped<IShiftService, ShiftService>();
            services.AddScoped<IApplicationFormService, ApplicationFormService>();
            services.AddScoped<IProgramtypeService, ProgramTypeService>();
            services.AddScoped<ISessionService, SessionService>();
            services.AddScoped<IAcademicyearService, AcademicyearService>();
            services.AddScoped<IApplicantManagementService, ApplicantManagementService>();
             services.AddScoped<IDashboardservice, Dashboardservice>();



            return services;
        }
        internal static IApplicationBuilder UseSecurityHeaders(this IApplicationBuilder app, IConfiguration config)
        {
            var settings = config.GetSection(nameof(SecurityHeaderSettings)).Get<SecurityHeaderSettings>();

            if (settings?.Enable is true)
            {
                app.Use(async (context, next) =>
                {
                    if (!context.Response.HasStarted)
                    {
                        if (!string.IsNullOrWhiteSpace(settings.Headers.XFrameOptions))
                        {
                            context.Response.Headers.Add(HeaderNames.XFRAMEOPTIONS, settings.Headers.XFrameOptions!);
                        }

                        if (!string.IsNullOrWhiteSpace(settings.Headers.XContentTypeOptions))
                        {
                            context.Response.Headers.Add(HeaderNames.XCONTENTTYPEOPTIONS, settings.Headers.XContentTypeOptions);
                        }

                        if (!string.IsNullOrWhiteSpace(settings.Headers.ReferrerPolicy))
                        {
                            context.Response.Headers.Add(HeaderNames.REFERRERPOLICY, settings.Headers.ReferrerPolicy);
                        }

                        if (!string.IsNullOrWhiteSpace(settings.Headers.PermissionsPolicy))
                        {
                            context.Response.Headers.Add(HeaderNames.PERMISSIONSPOLICY, settings.Headers.PermissionsPolicy);
                        }

                        if (!string.IsNullOrWhiteSpace(settings.Headers.SameSite))
                        {
                            context.Response.Headers.Add(HeaderNames.SAMESITE, settings.Headers.SameSite);
                        }

                        if (!string.IsNullOrWhiteSpace(settings.Headers.XXSSProtection))
                        {
                            context.Response.Headers.Add(HeaderNames.XXSSPROTECTION, settings.Headers.XXSSProtection);
                        }
                    }

                    await next();
                });
            }

            return app;
        }
        public static IServiceCollection AddExceptionMiddleware(this IServiceCollection services) => services.AddExceptionHandler<ExceptionHandler>();
#pragma warning disable CS0618 // Type or member is obsolete
        public static IServiceCollection AddFluentValidation(this IServiceCollection services)
        {
            services.AddFluentValidation(config =>
            {
                config.RegisterValidatorsFromAssembly(Assembly.GetAssembly(typeof(ChangePasswordRequestValidator)));
                config.AutomaticValidationEnabled = true;

            });
            services.AddSingleton<ICustomValidatorFactory, CustomValidatorFactory>();
            return services;

        }
#pragma warning restore CS0618 // Type or member is obsolete
        public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder app) =>
            app.UseExceptionHandler();
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
        public static async Task InitializeDatabasesAsync(this IServiceProvider services, CancellationToken cancellationToken = default)
        {
            // Create a new scope to retrieve scoped services
            using var scope = services.CreateScope();
            await scope.ServiceProvider.GetRequiredService<ApplicationDbInitializer>()
                .InitializeAsync(cancellationToken);
        }
        public static IServiceCollection AddHangfireServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connString = configuration.GetConnectionString("DefaultConnection");
            services.AddHangfire(configuration => configuration
                    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseRecommendedSerializerSettings()
                    .UseSqlServerStorage(connString, new SqlServerStorageOptions
                    {
                        CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                        SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                        QueuePollInterval = TimeSpan.Zero,
                        UseRecommendedIsolationLevel = true,
                        DisableGlobalLocks = true
                    })).AddHangfireServer();
            JobStorage.Current = new SqlServerStorage(connString);

            services.AddTransient<IJobService, HangfireService>();
            return services;
        }
        public static IServiceCollection AddServices(this IServiceCollection services, Type interfaceType, ServiceLifetime lifetime)
        {
            // registring 
            var interfaceTypes = AppDomain.CurrentDomain
                .GetAssemblies().SelectMany(s => s.GetTypes()
                .Where(t => interfaceType.IsAssignableFrom(t) && !t.IsAbstract && t.IsClass)
                .Select(t => new
                {
                    Service = t.GetInterfaces().FirstOrDefault(),
                    Implementation = t
                }));
            foreach (var type in interfaceTypes)
            {
                services.AddService(type.Service!, type.Implementation, lifetime);
            }
            return services;
        }
        public static IServiceCollection AddService(this IServiceCollection services, Type serviceType, Type implementationType, ServiceLifetime lifetime) => lifetime switch
        {
            ServiceLifetime.Transient => services.AddTransient(serviceType, implementationType),
            ServiceLifetime.Scoped => services.AddScoped(serviceType, implementationType),
            ServiceLifetime.Singleton => services.AddSingleton(serviceType, implementationType),
            _ => throw new ArgumentException("Invalid lifeTime", nameof(lifetime))

        };
    }
}
