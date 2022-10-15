using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using new2me_api.Data;
using new2me_api.Data.Query;
using new2me_api.Extensions;
using new2me_api.Helpers;
using new2me_api.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<New2meDataContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddControllers();
builder.Services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);
builder.Services.AddScoped<IQuery, Query>();
builder.Services.AddCors();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
//app.ConfigureExceptionHandler();
app.UseMiddleware<CustomExceptionMiddleware>(); // Custom Exception Middleware

app.UseAuthorization();
app.UseCors(m=> m.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()); 

app.MapControllers();

app.Run();
