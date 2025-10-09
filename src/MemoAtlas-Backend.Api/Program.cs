using MemoAtlas_Backend.Api.Data;
using MemoAtlas_Backend.Api.Middleware;
using MemoAtlas_Backend.Api.Models.Configurations;
using MemoAtlas_Backend.Api.Repositories;
using MemoAtlas_Backend.Api.Repositories.Interfaces;
using MemoAtlas_Backend.Api.Services;
using MemoAtlas_Backend.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<AuthOptions>(builder.Configuration.GetSection("AuthOptions"));

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHttpContextAccessor();
builder.Services.AddExceptionHandler<ExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddSingleton<IPasswordHasherService, PasswordHasherService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITagRepository, TagRepository>();
builder.Services.AddScoped<ITagGroupRepository, TagGroupRepository>();
builder.Services.AddScoped<IPromptRepository, PromptRepository>();
builder.Services.AddScoped<IMemoRepository, MemoRepository>();
builder.Services.AddScoped<ISessionRepository, SessionRepository>();

builder.Services.AddScoped<IUserContext, UserContext>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ISessionService, SessionService>();
builder.Services.AddScoped<ITagGroupService, TagGroupService>();
builder.Services.AddScoped<ITagService, TagService>();
builder.Services.AddScoped<IPromptService, PromptService>();
builder.Services.AddScoped<IPromptAnswerService, PromptAnswerService>();
builder.Services.AddScoped<IMemoService, MemoService>();

var app = builder.Build();

app.UseExceptionHandler();
app.UseMiddleware<SessionMiddleware>();

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
