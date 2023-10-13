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
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ITagRepository, TagRepository>();
builder.Services.AddScoped<ICategoryTagRepository, CategoryTagRepository>();
builder.Services.AddScoped<IPostCategoryRepository, PostCategoryRepository>();
builder.Services.AddScoped<IPostTagRepository, PostTagRepository>();
builder.Services.AddScoped<IImageRepository, ImageRepository>();
builder.Services.AddScoped<IVideoRepository, VideoRepository>();
builder.Services.AddScoped<IPostImageRepository, PostImageRepository>();
builder.Services.AddScoped<IPostVideoRepository, PostVideoRepository>();
//
builder.Services.AddScoped<IFollowUserHandlers, FollowUserHandlers>();
builder.Services.AddScoped<IReportPostHandlers, ReportPostHandlers>();
builder.Services.AddScoped<ISaveListHandlers, SaveListHandlers>();
builder.Services.AddScoped<IUserHandlers, UserHandlers>();
builder.Services.AddScoped<ICategoryHandlers, CategoryHandlers>();
builder.Services.AddScoped<ITagHandlers, TagHandlers>();
builder.Services.AddScoped<IImageHandlers, ImageHandlers>();
builder.Services.AddScoped<IVideoHandlers, VideoHandlers>();
builder.Services.AddScoped<IPostHandlers, PostHandlers>();
builder.Services.AddScoped<IPostRepository, PostRepository>();
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
