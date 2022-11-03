using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhiHu.Photo.Common.Models
{
    public class ApiResponse
    {

        public ApiResponse()
        {

        }
        public ApiResponse(string message, bool status = false)
        {
            Message = message;
            Status = status;
        }

        public ApiResponse(bool status, object? data)
        {
            Status = status;
            Data = data;
        }
        public string? Message { get; set; }

        public bool Status { get; set; }

        public object? Data { get; set; }
    }

    public class ApiResponse<T>
    {
        public ApiResponse()
        {
            
        }
        public ApiResponse(string message, bool status = false)
        {
            Message = message;
            Status = status;
        }

        public ApiResponse(bool status, T? data)
        {
            Status = status;
            Data = data;
        }
        public string? Message { get; set; }

        public bool Status { get; set; }

        public T? Data { get; set; }

    }
}
