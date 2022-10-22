using System.Net;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using new2me_api.Data;
using new2me_api.Data.Query;
using new2me_api.Extensions;
using new2me_api.Helpers;
using new2me_api.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<New2meDataContext>(options => options.UseMySql(builder.Configuration.GetConnectionString("Default"), ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("Default"))));
builder.Services.AddControllers();
builder.Services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);
builder.Services.AddScoped<IQuery, Query>();
builder.Services.AddCors();

var secretKey = builder.Configuration.GetSection("AppSettings:Key").Value;
var key = new SymmetricSecurityKey(Encoding.UTF32.GetBytes(secretKey));
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme) // =="Bearer"
                .AddJwtBearer(opt=>opt.TokenValidationParameters = new TokenValidationParameters{
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = key
                });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Host.ConfigureHostConfiguration(configHost => {
    configHost.AddEnvironmentVariables(prefix: "new2me");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
//app.ConfigureExceptionHandler();
app.UseMiddleware<CustomExceptionMiddleware>(); // Custom Exception Middleware

app.UseCors(m=> m.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()); 

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
