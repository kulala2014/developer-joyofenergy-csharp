using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JOIEnergy.Utility.Filters
{
    public class CustomExceptionFilterAttribute : Attribute, IExceptionFilter
    {
        private readonly ILogger<CustomExceptionFilterAttribute> _logger;

        public CustomExceptionFilterAttribute(ILogger<CustomExceptionFilterAttribute> logger)
        {
            _logger = logger;
        }
        public void OnException(ExceptionContext context)
        {
            if (!context.ExceptionHandled)
            {
                _logger.LogError($"While requesting {context.HttpContext.Request.Path}, exception occured, the message is {context.Exception}");
                context.Result = new JsonResult(new
                {
                    Result = false,
                    Code = 200,
                    Message = context.Exception.Message
                });
                context.ExceptionHandled = true;
            }
        }
    }
}
