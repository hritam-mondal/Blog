using Blog.API.Behaviors;
using Blog.API.Infrastructure.Data;
using Blog.API.Infrastructure.IoC;
using Carter;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var assembly = typeof(Program).Assembly;
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var client = new MongoClient(connectionString);

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseMongoDB(client, "blog"));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));
builder.Services.AddValidatorsFromAssembly(assembly);
builder.Services.AddCarter();
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
builder.Services.RegisterRepositories();
builder.Services.AddHttpContextAccessor();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "MyAllowSpecificOrigins",
        policy =>
        {
            policy.WithOrigins("https://localhost:7277").AllowAnyMethod().AllowAnyHeader();
        });
});

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.MapCarter();

app.UseCors("MyAllowSpecificOrigins");

app.Run();