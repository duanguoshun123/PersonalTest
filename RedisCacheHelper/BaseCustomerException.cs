using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisCacheHelper
{
    public class BaseCustomException : Exception
    {
        public BaseCustomException()
            : base()
        {
        }

        public BaseCustomException(string message)
            : base(string.IsNullOrWhiteSpace(message) ? "BaseCustomException" : message)
        {
        }

        public BaseCustomException(string message, Exception innerException)
            : base(string.IsNullOrWhiteSpace(message) ? "BaseCustomException" : message, innerException)
        {
        }

        public BaseCustomException(Exception innerException)
            : base(string.IsNullOrWhiteSpace(innerException.Message) ? "BaseCustomException" : innerException.Message, innerException)
        {
            var innerBaseCustomException = innerException as BaseCustomException;
            if (innerBaseCustomException != null)
            {
                this.CustomStatusCode = innerBaseCustomException.CustomStatusCode;

                this.NotSendLogMail = innerBaseCustomException.NotSendLogMail;
                this.ReturnInfo = innerBaseCustomException.ReturnInfo;
            }
        }

        public BaseCustomException(ReturnInfo returnInfo)
            : this(returnInfo == null ? "returnInfo == null" : string.IsNullOrWhiteSpace(returnInfo.Message) ? "BaseCustomException" : returnInfo.Message)
        {
            ReturnInfo = returnInfo;
        }

        public CustomStatusCode CustomStatusCode { get; set; } = CustomStatusCode.InternalServerError;

        public bool NotSendLogMail { get; set; }
        public ReturnInfo ReturnInfo { get; set; }
    }

    public enum CustomStatusCode
    {
        /// <summary>
        /// Summary:
        ///     Equivalent to HTTP status 400. System.Net.HttpStatusCode.BadRequest indicates
        ///     that the request could not be understood by the server. System.Net.HttpStatusCode.BadRequest
        ///     is sent when no other error is applicable, or if the exact error is unknown or
        ///     does not have its own error code.
        /// </summary>
        BadRequest = 400,

        /// <summary>
        /// Summary:
        ///     Equivalent to HTTP status 401. System.Net.HttpStatusCode.Unauthorized indicates
        ///     that the requested resource requires authentication. The WWW-Authenticate header
        ///     contains the details of how to perform the authentication.
        /// </summary>
        Unauthorized = 401,

        /// <summary>
        /// Summary:
        ///     Equivalent to HTTP status 403. System.Net.HttpStatusCode.Forbidden indicates
        ///     that the server refuses to fulfill the request.
        /// </summary>
        Forbidden = 403,

        /// <summary>
        /// Summary:
        ///     Equivalent to HTTP status 404. System.Net.HttpStatusCode.NotFound indicates that
        ///     the requested resource does not exist on the server.
        /// </summary>
        NotFound = 404,

        /// <summary>
        /// Summary:
        ///     Equivalent to HTTP status 500. System.Net.HttpStatusCode.InternalServerError
        ///     indicates that a generic error has occurred on the server.
        /// </summary>
        InternalServerError = 500,
    }
}
