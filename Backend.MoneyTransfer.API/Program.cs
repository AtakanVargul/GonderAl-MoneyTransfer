using Serilog;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.HttpOverrides;
using Backend.MoneyTransfer.Application;
using Backend.MoneyTransfer.Infrastructure;
using Backend.MoneyTransfer.Application.Common.Filters.ExceptionFilter;
using Backend.MoneyTransfer.Application.Common.Localization;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddControllers(options =>
        options.Filters.Add<ApiExceptionFilterAttribute>())
    .AddFluentValidation(x => x.AutomaticValidationEnabled = false)
    .AddNewtonsoftJson();

builder.Services.AddLogging();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(configuration);
builder.Services.AddIdentityConfiguration(configuration);
builder.Services.AddJwtAuthorization(configuration);
//builder.Services.AddMasstransitService(configuration);

builder.Host.UseSerilog((ctx, lc) =>
{
    lc.ReadFrom.Configuration(ctx.Configuration)
    .Enrich.WithMachineName()
    .Enrich.WithProcessId()
    .Enrich.WithThreadId();
});

builder.Services.ConfigureLocalization();

var app = builder.Build();

app.ConfigureLocalization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();