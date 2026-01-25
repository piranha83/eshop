
using System.Text.Json;
using Catalog.Api.DatabaseContext;
using Catalog.Api.Extensions;
using Infrastructure.Core;
using Infrastructure.Core.Extensions;
var builder = WebApplication.CreateBuilder(args);

// Web
builder.Services.AddCorsPolicy(Consts.ApplicationOrigin, builder.Configuration);
builder.Services.AddHttpLogging();
builder.Services.AddEndpointsApiExplorer().AddSwaggerGen();
builder.Services.AddHttpContextAccessor();
//
builder.Services.AddContext<ApplicationDbContext>(builder.Configuration);

var app = builder.Build();

await app.Init();

var jsonConfiguration = JsonConfiguration.Default();
jsonConfiguration.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;

app.AddControllers<ApplicationDbContext>(jsonConfiguration);
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