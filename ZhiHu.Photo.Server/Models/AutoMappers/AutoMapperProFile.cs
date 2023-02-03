using AutoMapper;
using ZhiHu.Photo.Common.Dtos;
using ZhiHu.Photo.Common.Models;
using ZhiHu.Photo.Server.Entities;
using ZhiHu.Photo.Server.Models.ZhiHu;

namespace ZhiHu.Photo.Server.Models.AutoMappers
{
    public class AutoMapperProFile : MapperConfigurationExpression
    {
        public AutoMapperProFile()
        {
            CreateMap<AnswerDto, AnswerEntity>().ReverseMap();
            CreateMap<ImageDto, ImageEntity>().ReverseMap();
            CreateMap<VideoDto, VideoEntity>().ReverseMap();
            CreateMap(typeof(PagedList<>), typeof(PagedList<>));
        }
    }
}
