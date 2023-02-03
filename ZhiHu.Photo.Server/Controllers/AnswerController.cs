using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ZhiHu.Photo.Common.Dtos;
using ZhiHu.Photo.Common.Models;
using ZhiHu.Photo.Common.Parameters;
using ZhiHu.Photo.Server.Controllers.Base;
using ZhiHu.Photo.Server.Extensions;
using ZhiHu.Photo.Server.Services.Interfaces;
using ZhiHu.Photo.Server.Services.Interfaces.Bases;

namespace ZhiHu.Photo.Server.Controllers
{
    public class AnswerController : BaseController
    {
        private readonly IAnswerService _service;
        private readonly IScanService _scan;
        private readonly IMapper _mapper;

        public AnswerController(IAnswerService service, IScanService scan, IMapper mapper)
        {
            _service = service;
            _scan = scan;
            _mapper = mapper;
        }
        /// <summary>
        /// 查询回答
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("[Action]")]
        public async Task<ApiResponse> GetAll([FromQuery] QueryParameter parameter)
        {
            try
            {
                var list = await _service.QueryAnswerAndImageAsync(parameter);
                return new ApiResponse(true, _mapper.Map<PagedList<AnswerDto>>(list));
            }
            catch (Exception e)
            {
                return new ApiResponse(e.Message);
            }
        }
        /// <summary>
        /// 开始扫描
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("[Action]")]
        public ApiResponse StartScan()
        {
            try
            {
                _scan.ScanInsert();
                return new ApiResponse("已启动", true);
            }
            catch (Exception e)
            {
                return new ApiResponse(e.Message);
            }
        }

        /// <summary>
        /// 扫描更新最前面的
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("[Action]")]
        public ApiResponse StartLastScan()
        {
            try
            {
                _scan.LastScanInsert();
                return new ApiResponse("已启动", true);
            }
            catch (Exception e)
            {
                return new ApiResponse(e.Message);
            }
        }
        /// <summary>
        /// 刷新数据Images
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("[Action]")]
        public ApiResponse LocalRefreshImage()
        {
            try
            {
                _scan.LocalRefreshImage();
                return new ApiResponse("正在刷新", true);
            }
            catch (Exception e)
            {
                return new ApiResponse(e.Message);
            }
        }
        /// <summary>
        /// 刷新数据Images
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("[Action]")]
        public ApiResponse ImagePatching()
        {
            try
            {
                _scan.ImagePatching();
                return new ApiResponse("正在补漏", true);
            }
            catch (Exception e)
            {
                return new ApiResponse(e.Message);
            }
        }
    }
}
