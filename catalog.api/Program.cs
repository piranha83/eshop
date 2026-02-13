using Catalog.Api.DatabaseContext;
using Catalog.Api.DatabaseContext.Models;
using Catalog.Api.Extensions;
using Catalog.Api.Featres.Job;
using Catalog.Api.Features.Product;
using FluentValidation;
using Infrastructure.Core.Extensions;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddClientFlow(builder.Configuration);
builder.Services.AddCrudServices();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
// Web
builder.Services.AddCorsPolicy(builder.Configuration);
builder.Services.AddHttpLogging();
builder.Services.AddEndpointsApiExplorer().AddSwagger();
builder.Services.AddHttpContextAccessor();
//
builder.Services.AddContext<ApplicationDbContext>(builder.Configuration);
builder.Services.AddMapper(typeof(Program).Assembly);
builder.Services.AddHostedService<InitJob>();
// render
var render = true;//builder.Environment.IsDevelopment();//builder.Environment.IsEnvironment("render");
if (render)
{
    builder.WebHost.ConfigureKestrel(serverOptions =>
    {
        serverOptions.ListenAnyIP(1000);
    });
}

var app = builder.Build();

app.MapGroup("Product")
    .MapGet<ApplicationDbContext, ProductModel, long, ProductResponse>()
    .MapPost<ApplicationDbContext, ProductModel, long, ProductCreateRequest>()
    .MapPut<ApplicationDbContext, ProductModel, long, ProductUpdateRequest>()
    .MapDelete<ApplicationDbContext, ProductModel, long>();

app.UseImgFiles(builder.Environment.ContentRootPath);

if (app.Environment.IsDevelopment())
{
    Console.WriteLine("Development");
    app.UseSwagger().UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}
else if (app.Environment.IsProduction())
{
    Console.WriteLine("Production");
    app.UseHttpsRedirection();
}

if (render)
{
    Console.WriteLine("render");
    // proxy render
    app.UseForwardedHeaders(new ForwardedHeadersOptions
    {
        ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
    });
}

app.UseCorsPolicy();
app.UseClientFlow();

app.Run();