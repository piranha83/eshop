using Infrastructure.Core.Extensions;
using Order.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpLogging();
builder.Services.AddHealthCheck();
builder.Services.AddCorsPolicy(builder.Configuration);
builder.Services.AddEndpointsApiExplorer().AddSwagger();
builder.Services.AddOrderLogic(builder.Configuration);
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

app.AddOrderHandler();
app.MapHealthCheck();
app.UseWeb();
app.UseCorsPolicy();
app.Run();