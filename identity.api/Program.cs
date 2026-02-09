using Identity.Api.Featres.Flow;
using Infrastructure.Core;
using Infrastructure.Core.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCorsPolicy(Consts.ApplicationOrigin, builder.Configuration);
builder.Services.AddOpenApi();
builder.Services.AddFlowServer(builder.Configuration);
builder.Services.AddHttpLogging();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseHttpsRedirection();
}

app.UseCors(Consts.ApplicationOrigin);
app.UseFlowServer();
app.UseForwardedHeaders();
app.UseHttpLogging();

app.Run();
