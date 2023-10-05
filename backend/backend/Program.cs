using backend.DTO;
using backend.Handlers.IHandlers;
using backend.Handlers.Implementors;
using backend.Models;
using backend.Repositories.Implementors;
using backend.Repositories.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
//
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ITagRepository, TagRepository>();
//
builder.Services.AddScoped<IUserRepository,UserRepository>();
builder.Services.AddScoped<IFollowUserRepositoy, FollowUserRepository>();
//
builder.Services.AddScoped<IFollowUserHandlers, FollowUserHandlers>();
builder.Services.AddScoped<ISaveListHandlers, SaveListHandlers>();
builder.Services.AddScoped<IUserHandlers, UserHandlers>();
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