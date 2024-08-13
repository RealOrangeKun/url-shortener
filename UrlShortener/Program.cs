using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using UrlShortener.Config;
using UrlShortener.Data;
using UrlShortener.Models;
using UrlShortener.Repositories;
using UrlShortener.Services;
using UrlShortener.Utilities;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));

builder.Services.AddDbContext<UrlShortenerDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IUrlService, UrlService>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped<IUrlRepository, UrlRepository>();
builder.Services.AddScoped<IUrlShortener, UrlShortenerUtility>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITokenRepository, TokenRepository>();
builder.Services.AddScoped<ITokenService, TokenService>();


builder.Services.AddControllers();


builder.Services.AddAuthentication()
.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
{
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SigningKey"] ?? throw new Exception("Please provide jwt signing key")))
    };

});



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
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
