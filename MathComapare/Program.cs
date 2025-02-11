using MathComapare.Models;
using AspNetCoreRateLimit;
using MathComapare.Context;
using Microsoft.EntityFrameworkCore;
using System.Net.WebSockets;
using MathComapare.Websocket;
using StackExchange.Redis;
using MathComapare.Repositories.Interfaces;
using MathComapare.Repositories;
using MathComapare.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(option =>
{
    option.AddPolicy("AllowSpecificOrigins", policy =>
    {
        policy.WithOrigins("https://checkmath.io.vn", "http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddScoped<IMathExpressionService, MathComparisonService>();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<IMathCompareRepositories, MathCompareRepositories>();
builder.Services.AddSingleton<WebSocketConnectionManager>();
builder.Services.AddSingleton<WebsocketHandler>();
//builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect("localhost:6379"));

builder.Services.AddLogging(option =>
{
    option.AddConsole();
    option.AddDebug();
});

builder.Services.AddLogging();
builder.Services.AddMemoryCache();
//builder.Services.Configure<IpRateLimitOptions>(options =>
//{
//    options.EnableEndpointRateLimiting = true;
//    options.StackBlockedRequests = false;
//    options.GeneralRules = new List<RateLimitRule> 
//    {
//        new()
//        {
//            Period = "1s",
//            Limit = 10,
//            Endpoint = "*"
//        }
//    };
//    options.HttpStatusCode = 429;
//    options.QuotaExceededResponse = new QuotaExceededResponse
//    {
//        Content = "{\"message\": \"Too many requests. Please try again later.\"}",
//        ContentType = "application/json",
//        StatusCode = 429
//    };
//});

builder.Services.AddInMemoryRateLimiting();
builder.Services.AddDbContext<CheckmathDBContext>(option =>
    option.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Map("/ws", async (HttpContext context, WebsocketHandler handler) =>
{
    await handler.HandlerWebsocket(context);
});




app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("AllowSpecificOrigins");

app.UseIpRateLimiting();

app.UseAuthorization();
app.MapControllers();

app.Run();