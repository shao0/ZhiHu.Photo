using Microsoft.AspNetCore.Mvc;
using Quartz;
using ZhiHu.Photo.Common.Models;
using ZhiHu.Photo.Server.Controllers.Base;
using ZhiHu.Photo.Server.Services.Interfaces;

namespace ZhiHu.Photo.Server.Controllers
{
    public class DispatchController : BaseController
    {
        private readonly IDispatchService _dispatch;

        public DispatchController(IDispatchService dispatch)
        {
            _dispatch = dispatch;
        }

        [HttpGet]
        [Route("[Action]")]
        public async Task<ApiResponse> Start()
        {
            try
            {
                await _dispatch.Start();
                return new ApiResponse(true, new object());
            }
            catch (Exception e)
            {
                return new ApiResponse(e.Message);
            }
        }
    }
}
