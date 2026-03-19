using CertificateStore.Api.Data;
using CertificateStore.Api.Services;
using CertificateStore.Api.Settings;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDbSettings"));

builder.Services.AddSingleton<MongoDbContext>();

builder.Services.AddSingleton<IRootCertificateService, RootCertificateService>();
builder.Services.AddSingleton<IUserCertificateService, UserCertificateService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseAuthorization();

app.MapControllers();

app.Run();