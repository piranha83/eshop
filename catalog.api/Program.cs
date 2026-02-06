using Catalog.Api.DatabaseContext;
using Catalog.Api.DatabaseContext.Models;
using Catalog.Api.Extensions;
using Catalog.Api.Features.Product;
using FluentValidation;
using Infrastructure.Core;
using Infrastructure.Core.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCrudServices();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
// Web
builder.Services.AddCorsPolicy(Consts.ApplicationOrigin, builder.Configuration);
builder.Services.AddHttpLogging();
builder.Services.AddEndpointsApiExplorer().AddSwaggerGen();
builder.Services.AddHttpContextAccessor();
//
builder.Services.AddContext<ApplicationDbContext>(builder.Configuration);
builder.Services.AddMapper(typeof(Program).Assembly);

var app = builder.Build();

await app.Init();

app.MapGroup("Product")
    .MapGet<ApplicationDbContext, ProductModel, long, ProductResponse>()
    .MapPost<ApplicationDbContext, ProductModel, long, ProductCreateRequest>()
    .MapPut<ApplicationDbContext, ProductModel, long, ProductUpdateRequest>()
    .MapDelete<ApplicationDbContext, ProductModel, long>();

app.UseImgFiles(builder.Environment.ContentRootPath);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger().UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseHttpsRedirection();
}

app.UseCors(Consts.ApplicationOrigin);

app.Run();