using ECommerceG02.Domian.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceG02.Shared.Models
{
    public class ErrorResponse
    {
        public bool Success { get; set; } = false; 
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string ErrorCode { get; set; }

        public ErrorResponse() { }

        public ErrorResponse(int statusCode, string message, string errorCode)
        {
            StatusCode = statusCode;
            Message = message;
            ErrorCode = errorCode;
        }

        public ErrorResponse(AppException ex)
        {
            StatusCode = ex.StatusCode;
            Message = ex.Message;
            ErrorCode = ex.ErrorCode;
        }
    }
}

