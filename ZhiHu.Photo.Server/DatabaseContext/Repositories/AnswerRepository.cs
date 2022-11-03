using Microsoft.EntityFrameworkCore;
using ZhiHu.Photo.Server.DatabaseContext.UnitOfWork;
using ZhiHu.Photo.Server.Entities;

namespace ZhiHu.Photo.Server.DatabaseContext.Repositories
{
    public class AnswerRepository:Repository<AnswerEntity>
    {
        public AnswerRepository(PhotoDbContext dbContext) : base(dbContext)
        {
        }
    }
}
