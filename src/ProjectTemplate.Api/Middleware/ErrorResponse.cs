using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectTemplate.Api.Middleware
{
    public class ErrorResponse
    {
        /// <summary>
        /// Error message
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Error message for the inner exception
        /// </summary>
        public string InnerExceptionMessage { get; set; }

        /// <summary>
        /// Stack trace for the exception
        /// </summary>
        public string StackTrace { get; set; }

        /// <summary>
        /// Additional data
        /// </summary>
        public System.Collections.IDictionary Data { get; set; }
    }
}
