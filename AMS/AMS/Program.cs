using AMS.Extensions;
using Microsoft.AspNetCore.Mvc;
using Serilog;

Log.Information("Server Booting Up...");
try
{
    var builder = WebApplication.CreateBuilder(args);

    Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

    builder.Logging.ClearProviders();
    builder.Logging.AddSerilog(Log.Logger);
    builder.Host.UseSerilog();
    var configuration = builder.Configuration;
    var environment = builder.Environment;

    builder.Services.AddDatabaseServices(environment, configuration);
    builder.Services.AddIdentity();
    builder.Services.AddCurrentUserService();
    builder.Services.AddFluentValidation();
    builder.Services.AddDataSeeder();
    builder.Services.AddCorsPolicy(configuration);
    builder.Services.AddControllers(opt =>
    {
        //opt.Filters.Add<ApiValidationFilter>();
        //opt.ModelValidatorProviders.Clear();
    })
        .ConfigureApiBehaviorOptions(options =>
        {
            options.InvalidModelStateResponseFactory = context =>
            {
                return new UnprocessableEntityObjectResult(context.ModelState);
            };
        });
    builder.Services.AddProblemDetails();
    builder.Services.AddApplicationServices(configuration);
    builder.Services.AddHangfireServices(configuration);
    builder.Services.AddSwaggerService();
    builder.Services.AddExceptionMiddleware();
    builder.Services.AddRateLimiter((options) =>
    {
    });
    var app = builder.Build();

    //uncomment the code when data seeding is required
    //await app.Services.InitializeDatabasesAsync();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    app.UseExceptionMiddleware();

    app.UseSerilogRequestLogging();

    app.UseRateLimiter();

    app.UseCorsPolicy();

    app.UseHttpsRedirection();

    app.UseStaticFiles();

    app.UseAuthentication();

    app.UseAuthorization();

    app.UseCurrentUser();

    app.MapControllers();

    app.Run();

}
catch (Exception ex) when (!ex.GetType().Name.Equals("HostAbortedException", StringComparison.Ordinal))
{
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Server Shutting down...");
    Log.CloseAndFlush();
}