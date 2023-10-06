
using Microsoft.Extensions.DependencyInjection;
using backend.Handlers.IHandlers;
using backend.Handlers.Implementors;
using backend.Repositories.Implementors;
using backend.Repositories.IRepositories;
using RestSharp;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<IRestClient, RestClient>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
//
builder.Services.AddScoped<IUserRepository,UserRepository>();
builder.Services.AddScoped<IFollowUserRepository, FollowUserRepository>();
builder.Services.AddScoped<IReportPostRepository, ReportPostRepository>();
builder.Services.AddScoped<ISaveListRepository, SaveListRepository>();
//
builder.Services.AddScoped<IFollowUserHandlers, FollowUserHandlers>();
builder.Services.AddScoped<IReportPostHandlers, ReportPostHandlers>();
builder.Services.AddScoped<ISaveListHandlers, SaveListHandlers>();
builder.Services.AddScoped<IUserHandlers, UserHandlers>();
//
builder.Services.AddScoped<IPostHandlers, PostHandlers>();
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
