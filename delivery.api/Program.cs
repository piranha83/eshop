using Delivery.Api.Extensions;
using Infrastructure.Core.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpLogging();
builder.Services.AddHealthCheck();
builder.Services.AddCorsPolicy(builder.Configuration);
builder.Services.AddEndpointsApiExplorer().AddSwagger();
//
builder.Services.AddBuisness(builder.Configuration);
builder.ConfigureWeb();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger().UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}
else if (app.Environment.IsProduction())
{
    app.UseHttpsRedirection();
}
app.AddHandlers();
app.MapHealthCheck();
app.UseWeb();
app.UseCorsPolicy();
app.Run();