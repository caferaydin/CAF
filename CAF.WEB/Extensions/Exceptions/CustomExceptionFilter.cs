using CAF.Application.Extensions.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace CAF.WEB.Extensions.Exceptions;

public class CustomExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        var exception = context.Exception;

        if (context.HttpContext.Request.Headers["Accept"].ToString().Contains("application/json"))
        {
            context.Result = new JsonResult(new
            {
                exception.Message
            })
            {
                StatusCode = exception is UserCreateFailedException ?
                             StatusCodes.Status400BadRequest :
                             StatusCodes.Status500InternalServerError
            };
        }
        else
        {
            // ViewData'yı doğru şekilde oluşturup ata
            var viewResult = new ViewResult
            {
                ViewName = "Error",
                ViewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), context.ModelState)
                {
                    { "Title", "Hata" },
                    { "ErrorMessage", exception.Message }
                }
            };

            context.Result = viewResult;
        }

        context.ExceptionHandled = true;
    }
}