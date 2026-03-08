using Catalog.Api.DatabaseContext;
using Catalog.Api.DatabaseContext.Models;
using Catalog.Api.Extensions;
using Catalog.Api.Featres.Job;
using Catalog.Api.Features.Product;
using FluentValidation;
using Infrastructure.Core.Extensions;
using Infrastructure.Core.Features.Security;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddSecrets();
builder.Services.AddHealthCheck();
using var ctx = SecurityContextFactory.Create(builder.Configuration);
builder.Services.AddClientFlow(builder.Configuration, ctx);
builder.Services.AddCrudServices();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
// Web
builder.Services.AddCorsPolicy(builder.Configuration);
builder.Services.AddHttpLogging();
builder.Services.AddEndpointsApiExplorer().AddSwagger();
builder.Services.AddDefaultContext();
builder.Services.AddCache(builder.Configuration);
//
builder.Services.AddContext<ApplicationDbContext>(builder.Configuration);
builder.Services.AddMapper(typeof(Program).Assembly);
builder.Services.AddHostedService<InitJob>();
builder.ConfigureWeb();

var app = builder.Build();

app.MapGroup("Product")
    .MapList<ApplicationDbContext, ProductModel, long, ProductResponse>()
    .MapDetails<ApplicationDbContext, ProductModel, long, ProductResponse>()
    .MapPost<ApplicationDbContext, ProductModel, long, ProductCreateRequest>()
    .MapPut<ApplicationDbContext, ProductModel, long, ProductUpdateRequest>()
    .MapDelete<ApplicationDbContext, ProductModel, long>();

app.UseImgFiles(builder.Environment.ContentRootPath);
app.MapHealthCheck();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger().UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}
else if (app.Environment.IsProduction())
{
    app.UseHttpsRedirection();
}

app.UseOutputCache();
app.UseWeb();
app.UseCorsPolicy();
app.UseClientFlow();

app.Run();