using Identity.Api.Featres.Flow;
using Infrastructure.Core.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHealthChecks();
builder.Services.AddCorsPolicy(builder.Configuration);
builder.Services.AddFlowServer(builder.Configuration);
builder.Services.AddHttpLogging();
builder.Services.AddHttpContextAccessor();
builder.ConfigureWeb();

var app = builder.Build();

app.MapHealthChecks("/");
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
