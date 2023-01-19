using ZhiHu.Photo.Server.DatabaseContext.UnitOfWork;
using ZhiHu.Photo.Server.Entities;
using ZhiHu.Photo.Server.Services.Bases;
using ZhiHu.Photo.Server.Services.Interfaces;
using ZhiHu.Photo.Server.Services.Interfaces.Bases;

namespace ZhiHu.Photo.Server.Services
{
    public class ImageService : BaseService<ImageEntity>, IImageService
    {
        public ImageService(IUnitOfWork work) : base(work)
        {
        }
    }
}
