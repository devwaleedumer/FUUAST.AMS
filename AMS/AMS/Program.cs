
using AMS.Extensions;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Net.Mime;


namespace AMS
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .CreateLogger();

            builder.Logging.ClearProviders();
            builder.Logging.AddSerilog(Log.Logger);

            var configuration = builder.Configuration;
            var envirnoment = builder.Environment;

            builder.Services.AddIdentity();

            builder.Services.AddDatabaseServices(envirnoment, configuration);

            builder.Services.AddApplicationServices(configuration);

            builder.Services.AddCurrentUserService();

            builder.Services.AddFluentValidation();

            //builder.Services.AddProblemDetails();

            // Add services to the container.
            builder.Services.AddControllers().ConfigureApiBehaviorOptions(options => 
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    return new UnprocessableEntityObjectResult(context.ModelState);
                };
            });

            builder.Services.AddApplicationServices(configuration);
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
           builder.Services.AddSwaggerService();
            builder.Services.AddExceptionMiddleware();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
         
            app.UseCurrentUser();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.UseExceptionMiddleware();

            app.Run();
        }
    }
}
