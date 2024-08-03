using Microsoft.EntityFrameworkCore;
using UrlShortener.Data;
using UrlShortener.Repositories;
using UrlShortener.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<UrlShortenerDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IUrlService, UrlService>();
builder.Services.AddScoped<IUrlRepository, UrlRepository>();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();
