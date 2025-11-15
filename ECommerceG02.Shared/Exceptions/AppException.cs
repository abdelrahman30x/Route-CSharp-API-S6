using System;

namespace ECommerceG02.Domian.Exceptions
{
    public abstract class AppException : Exception
    {
        public int StatusCode { get; }
        public string ErrorCode { get; }

        protected AppException(string message, int statusCode = 400, string errorCode = "APP_ERROR")
            : base(message)
        {
            StatusCode = statusCode;
            ErrorCode = errorCode;
        }
    }
}
