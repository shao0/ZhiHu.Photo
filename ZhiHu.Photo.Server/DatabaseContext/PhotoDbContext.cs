using Microsoft.EntityFrameworkCore;
using ZhiHu.Photo.Server.Entities;

namespace ZhiHu.Photo.Server.DatabaseContext
{
    /// <summary>
    /// 
    /// </summary>
    public class PhotoDbContext:DbContext
    {
        public PhotoDbContext(DbContextOptions<PhotoDbContext> options) : base(options)
        {

        }

        public DbSet<AnswerEntity> Answer { get; set; }

        public static readonly ILoggerFactory ConsoleLoggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddFilter((category, level) =>
                category == DbLoggerCategory.Database.Command.Name && level == LogLevel.Information
            ).AddConsole();
        });
    }
}
