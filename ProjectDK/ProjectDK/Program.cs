using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using ProjectDK.BL.CommandHandlers;
using ProjectDK.Extensions;
using ProjectDK.HealthChecks;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

var logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console(theme: AnsiConsoleTheme.Code)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);
builder.Logging.AddSerilog(logger);

builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining(typeof(Program));

// Add services to the container.
builder.Services.RegisterRepositories().RegisterServices().AddAutoMapper(typeof(Program));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks()
    .AddCheck<TestHealthCheck>("Test")
    .AddCheck<SQLHealthCheck>("SQL Server").AddUrlGroup(new Uri("https://google.com"),name: "Google Service");

builder.Services.AddMediatR(typeof(GetAllBooksCommandHandler).Assembly);


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

app.RegisterHealthChecks();

app.Run();
