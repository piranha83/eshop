using FluentValidation;
using Infrastructure.Core.Extensions;
using Payment.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpLogging();
builder.Services.AddHealthCheck();
builder.Services.AddCorsPolicy(builder.Configuration);
builder.Services.AddEndpointsApiExplorer().AddSwagger();
//
builder.Services.AddValidatorsFromAssemblies([typeof(ContractApi).Assembly, typeof(Program).Assembly]);
builder.Services.AddMapper(typeof(Program).Assembly);
builder.Services.AddCrudServices();
builder.Services.AddPaymenProcess(builder.Configuration);
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
app.MapGroup("Payment");
app.MapHealthCheck();
app.UseWeb();
app.UseCorsPolicy();
app.Run();