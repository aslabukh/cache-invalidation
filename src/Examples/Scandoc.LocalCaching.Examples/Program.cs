using Scandoc.LocalCaching;
using Scandoc.LocalCaching.Examples.Caches;
using Scandoc.LocalCaching.Examples.Infrastructure;
using StackExchange.Redis;
using IDatabase = Scandoc.LocalCaching.Examples.Infrastructure.IDatabase;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IDatabase, Database>();
var redis = ConnectionMultiplexer.Connect(builder.Configuration["Redis:Url"], opt =>
{
    opt.User = builder.Configuration["Redis:User"];
    opt.Password = builder.Configuration["Redis:Password"];
    opt.ConnectTimeout = 1000;
    opt.AbortOnConnectFail = false;
});
builder.Services.AddLocalCaching("weather-forecast", redis, b => b
    .AddCache<ISummariesCache, SummariesCache>("Summaries"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();