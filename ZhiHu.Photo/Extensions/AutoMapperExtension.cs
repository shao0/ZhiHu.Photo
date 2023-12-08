using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using ZhiHu.Photo.Common.Dtos;
using ZhiHu.Photo.Common.Interfaces;
using ZhiHu.Photo.Common.Models;
using ZhiHu.Photo.Models;

namespace ZhiHu.Photo.Extensions
{
    public static class AutoMapperExtension
    {
        public static IMapper Instance { get; set; } =  new Mapper(new MapperConfiguration(CreateMapperConfiguration));
        /// <summary>
        /// 转换
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="s"></param>
        /// <returns></returns>
        public static T Map<T>(this object s)
            => Instance.Map<T>(s);

        private static void CreateMapperConfiguration(IMapperConfigurationExpression config)
        {
            config.CreateMap(typeof(ApiResponse<>),typeof(ApiResponse<>));
            config.CreateMap(typeof(PagedList<>),typeof(PagedList<>));
            config.CreateMap<AnswerDto, AnswerInfo>();
            config.CreateMap<ImageDto, ImageInfo>();
            config.CreateMap<VideoDto, VideoInfo>();
            
        }


        public static IServiceCollection AutoMapper(this IServiceCollection services)
        {
            services.AddSingleton(Instance);

            return services;
        }
    }
}
