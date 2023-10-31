using Microsoft.Extensions.DependencyInjection;
using backend.Handlers.IHandlers;
using backend.Handlers.Implementors;
using backend.Repositories.Implementors;
using backend.Repositories.IRepositories;
using RestSharp;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "FBlogAcademy", Version = "v1" });
    var filePath = Path.Combine(System.AppContext.BaseDirectory, "backend.xml");
    c.IncludeXmlComments(filePath);
});
// Add services to the container.
builder.Services.AddSingleton<IRestClient, RestClient>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder =>
        {
            builder.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod(); //THIS LINE RIGHT HERE IS WHAT YOU NEED
        });
});
//
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IFollowUserRepository, FollowUserRepository>();
builder.Services.AddScoped<IReportPostRepository, ReportPostRepository>();
builder.Services.AddScoped<ISaveListRepository, SaveListRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IPostCategoryRepository, PostCategoryRepository>();
builder.Services.AddScoped<ITagRepository, TagRepository>();
builder.Services.AddScoped<IPostTagRepository, PostTagRepository>();
builder.Services.AddScoped<ICategoryTagRepository, CategoryTagRepository>();
builder.Services.AddScoped<IImageRepository, ImageRepository>();
builder.Services.AddScoped<IVideoRepository, VideoRepository>();
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<IVoteCommentRepository, VoteCommentRepository>();
builder.Services.AddScoped<IVotePostRepository, VotePostRepository>();
builder.Services.AddScoped<IPostListRepository, PostListRepository>();
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
builder.Services.AddScoped<ICommentHandlers, CommentHandlers>();
builder.Services.AddScoped<IVoteCommentHandlers, VoteCommentHandlers>();
builder.Services.AddScoped<IVotePostHandlers, VotePostHandlers>();
builder.Services.AddScoped<IPostListHandlers, PostListHandlers>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "FBlogAcademy V1");
        options.RoutePrefix = string.Empty;
        options.DocumentTitle = "FBlogAcademyBackEnd";
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
