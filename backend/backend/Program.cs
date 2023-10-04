<<<<<<< HEAD

using backend.DTO;
using backend.Models;
using backend.Repositories.Implementors;
using backend.Repositories.IRepositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using RestSharp;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<IRestClient, RestClient>();
builder.Services.AddScoped<IUserRepository,UserRepository>();
=======
using backend.Models;
using backend.Repositories.Implementors;
using backend.Repositories.IRepositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
>>>>>>> main
builder.Services.AddControllers();
builder.Services.AddTransient<FBlogAcademyContext>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();


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
