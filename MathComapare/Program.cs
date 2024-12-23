using MathComapare.Models;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using AspNetCoreRateLimit;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IMathExpressionService, MathComparisonService>();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddLogging(option =>
{
    option.AddConsole();
    option.AddDebug();
});

builder.Services.AddLogging();
builder.Services.AddMemoryCache();
builder.Services.Configure<IpRateLimitOptions>(options =>
{
    options.EnableEndpointRateLimiting = true;
    options.StackBlockedRequests = false;
    options.RealIpHeader = "X-Real-IP"; 
    options.GeneralRules = new List<RateLimitRule> 
    {
        new()
        {
            Period = "1s",
            Limit = 10,
            Endpoint = "*"
        }
    };
    options.HttpStatusCode = 429;
    options.QuotaExceededResponse = new QuotaExceededResponse
    {
        Content = "{\"message\": \"Too many requests. Please try again later.\"}",
        ContentType = "application/json",
        StatusCode = 429
    };
});

builder.Services.AddInMemoryRateLimiting();
builder.Services.AddCors(option =>
{
    option.AddPolicy("AllowSpecificOrigins", policy =>
    {
        policy.WithOrigins("https://checkmath.io.vn/ ", "http://localhost:3000")
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});


var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseIpRateLimiting();
app.UseRouting();
app.UseCors("AllowSpecificOrigins");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
