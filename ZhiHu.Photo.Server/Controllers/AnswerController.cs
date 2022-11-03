using Microsoft.AspNetCore.Mvc;
using ZhiHu.Photo.Common.Models;
using ZhiHu.Photo.Common.Parameters;
using ZhiHu.Photo.Server.Controllers.Base;
using ZhiHu.Photo.Server.Extensions;
using ZhiHu.Photo.Server.Services.Interfaces;

namespace ZhiHu.Photo.Server.Controllers
{
    public class AnswerController : BaseController
    {
        private readonly IAnswerService _service;
        private readonly IScanService _scan;

        public AnswerController(IAnswerService service, IScanService scan)
        {
            _service = service;
            _scan = scan;
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
                var list = await _service.QueryAsync(parameter);
                return new ApiResponse(true, list);
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
        public async Task<ApiResponse> StartScan()
        {
            try
            {
               await _scan.ScanInsert();
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
        public async Task<ApiResponse> StartLastScan()
        {
            try
            {
                await _scan.LastScanInsert();
                return new ApiResponse("已启动", true);
            }
            catch (Exception e)
            {
                return new ApiResponse(e.Message);
            }
        }
    }
}
