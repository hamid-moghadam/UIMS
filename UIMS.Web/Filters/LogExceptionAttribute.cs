using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UIMS.Web.Models;
using UIMS.Web.Services;

namespace UIMS.Web.Filters
{
    public class LogExceptionAttribute : ExceptionFilterAttribute,IFilterFactory
    {
        private readonly ExceptionLogService _exceptionLogService;

        public LogExceptionAttribute(ExceptionLogService exceptionLogService)
        {
            
            _exceptionLogService = exceptionLogService;
        }

        public bool IsReusable => true;

        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            return serviceProvider.GetService(typeof(LogExceptionAttribute)) as IFilterMetadata;
        }

        public override void OnException(ExceptionContext context)
        {
            var exception = context.Exception;
            _exceptionLogService.AddAsync(new ExceptionLog()
            {
                HelpLink = exception.HelpLink,
                InnerException = exception.InnerException,
                Message = exception.Message,
                Source = exception.Source,
                StackTrace = exception.StackTrace
            }).Wait();
            _exceptionLogService.SaveChanges();
            base.OnException(context);
        }
    }
}
