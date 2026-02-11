using Identity.Api.Featres.Flow;
using Infrastructure.Core.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCorsPolicy(builder.Configuration);
builder.Services.AddFlowServer(builder.Configuration);
builder.Services.AddHttpLogging();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseHttpsRedirection();
}

app.UseCorsPolicy();
app.UseFlowServer();
app.UseForwardedHeaders();
app.UseHttpLogging();

app.Run();
