using Identity.Api.Featres.Flow;
using Infrastructure.Core.Extensions;
using Infrastructure.Core.Features.Security;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddSecrets();
using var ctx = SecurityContextFactory.Create(builder.Configuration);
builder.Services.AddHealthCheck();
builder.Services.AddCorsPolicy(builder.Configuration);
builder.Services.AddFlowServer(builder.Configuration, ctx);
builder.Services.AddHttpLogging();
builder.Services.AddDefaultContext();
builder.ConfigureWeb();

var app = builder.Build();

app.MapHealthCheck();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseHttpsRedirection();
}

app.UseWeb();
app.UseCorsPolicy();
app.UseFlowServer();
app.UseForwardedHeaders();
app.UseHttpLogging();

app.Run();
