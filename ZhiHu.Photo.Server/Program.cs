using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using ZhiHu.Photo.Server.DatabaseContext;
using ZhiHu.Photo.Server.DatabaseContext.Repositories;
using ZhiHu.Photo.Server.DatabaseContext.UnitOfWork;
using ZhiHu.Photo.Server.Entities;
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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

//app.Services.GetService<IDispatchService>()?.Start();

app.Run();
