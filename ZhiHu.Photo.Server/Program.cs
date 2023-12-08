using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using ZhiHu.Photo.Server.DatabaseContext;
using ZhiHu.Photo.Server.DatabaseContext.Repositories;
using ZhiHu.Photo.Server.DatabaseContext.UnitOfWork;
using ZhiHu.Photo.Server.Entities;
using ZhiHu.Photo.Server.Extensions;
using ZhiHu.Photo.Server.Models.AutoMappers;
using ZhiHu.Photo.Server.Quartzs;
using ZhiHu.Photo.Server.Quartzs.Jobs;
using ZhiHu.Photo.Server.Services;
using ZhiHu.Photo.Server.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var connectionString = builder.Configuration.GetConnectionString("ContextMysql");
builder.Services.AddDbContext<PhotoDbContext>(options =>
        options
            .UseLoggerFactory(PhotoDbContext.ConsoleLoggerFactory)
            .UseMySql(connectionString, new MySqlServerVersion("1.0")))
    .AddUnitOfWork<PhotoDbContext>()
    .AddCustomRepository<AnswerEntity, AnswerRepository>()
    ;
builder.Services.AddSwaggerGen(
    options =>
    {
        options.SwaggerDoc("V1", new OpenApiInfo()
        {
            Title = "知乎回答",
            Version = "1.0",
            Description = "知乎回答",
        });
        var file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ZhiHu.Photo.Server.xml");
        options.IncludeXmlComments(file, true);

        #region JWT验证
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Description = "输入 JWT",
            Name = "Authorization",
            BearerFormat = "JWT",
            Scheme = "Bearer"
        });
        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[] {}
            }
        });
        #endregion
    });
var url = builder.Configuration["Url"];
builder.WebHost.UseUrls(url);

var automakerConfig = new MapperConfiguration(config =>
{
    config.AddProfile(new AutoMapperProFile());
});

builder.Services.AddSingleton(automakerConfig.CreateMapper());

builder.Services.AddTransient<GirlPhotoJob>();
builder.Services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
builder.Services.AddSingleton<IJobFactory, IocJobFactory>();
builder.Services.AddAutoServices();

var app = builder.Build();

app.Services.Initial();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(n => n.SwaggerEndpoint("/swagger/V1/swagger.json", "V1"));
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
//app.Services.GetService<IDispatchService>()?.Start();

app.Run();
