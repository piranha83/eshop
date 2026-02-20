using Catalog.Api.DatabaseContext;
using Catalog.Api.DatabaseContext.Models;
using Catalog.Api.Extensions;
using Catalog.Api.Featres.Job;
using Catalog.Api.Features.Product;
using FluentValidation;
using Infrastructure.Core.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHealthChecks();
builder.Services.AddClientFlow(builder.Configuration,
    builder.Environment.IsDevelopment() && builder.Configuration.IsRender());
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
builder.ConfigureWeb();

var app = builder.Build();

app.MapGroup("Product")
    .MapGet<ApplicationDbContext, ProductModel, long, ProductResponse>()
    .MapPost<ApplicationDbContext, ProductModel, long, ProductCreateRequest>()
    .MapPut<ApplicationDbContext, ProductModel, long, ProductUpdateRequest>()
    .MapDelete<ApplicationDbContext, ProductModel, long>();

app.UseImgFiles(builder.Environment.ContentRootPath);
app.MapHealthChecks("/");
if (app.Environment.IsDevelopment())
{
    app.UseSwagger().UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}
else if (app.Environment.IsProduction())
{
    app.UseHttpsRedirection();
}

app.UseWeb();
app.UseCorsPolicy();
app.UseClientFlow();

app.Run();