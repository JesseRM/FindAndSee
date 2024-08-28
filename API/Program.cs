using API.Image;
using API.Services;
using Application.Core;
using Application.Finds;
using Application.Images;
using Application.Likes;
using Application.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Persistance.Repositories.Users;
using Persistence.DbAccess;
using Persistence.Repositories.Images;
using Persistence.Repositories.Likes;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(opt =>
{
    opt.AddPolicy(
        "CorsPolicy",
        policy =>
        {
            policy
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()
                .WithOrigins("http://localhost:3000", "https://findandsee-ui.onrender.com");
        }
    );
});

// Add services to the container.
builder.Services.AddControllers(options =>
{
    var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
    options.Filters.Add(new AuthorizeFilter(policy));
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerServices();
builder.Services.AddScoped<ISqlDataAccess, SqlDataAccess>();
builder.Services.AddScoped<IUserData, UserData>();
builder.Services.AddScoped<IFindData, FindData>();
builder.Services.AddScoped<IImageData, ImageData>();
builder.Services.AddScoped<ILikeData, LikeData>();
builder.Services.AddScoped<IMapper, Mapper>();
builder.Services.AddScoped<IImageAccessor, ImageAccessor>();
builder.Services.AddIdentityServices(builder.Configuration);
builder.Services.AddAutoMapper(typeof(MappingProfiles).Assembly);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CorsPolicy");

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
