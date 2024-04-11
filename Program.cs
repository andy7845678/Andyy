using Andyy.Models;
using Microsoft.EntityFrameworkCore;
using Serilog.Events;
using Serilog;
using Microsoft.AspNetCore.HttpLogging;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
    .WriteTo.Console()
    .WriteTo.Seq("http://localhost:5341")
    .CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);
    builder.Host.UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Services(services)
        .Enrich.FromLogContext()
        .WriteTo.Console()
        .WriteTo.Seq("http://localhost:5341")        
    );
    //DI
    builder.Services.AddDbContext<AndyyContext>(option =>
    option.UseSqlServer(builder.Configuration.GetConnectionString("andyyContext")));

    // Important to call at exit so that batched events are flushed.



    // Add services to the container.

    builder.Services.AddControllers();

    builder.Services.AddEndpointsApiExplorer();

    builder.Services.AddSwaggerGen();

    builder.Services.AddMemoryCache();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}

catch (Exception e)
{
    Log.Error(e, "Host terminated unexpectedly");
}

Log.CloseAndFlush();
