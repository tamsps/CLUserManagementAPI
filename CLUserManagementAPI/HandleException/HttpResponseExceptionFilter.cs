using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using CLUserManagementAPI.Controllers;
using System.Diagnostics;
using Serilog;

namespace CLUserManagementAPI.HandleException
{
	public class HttpResponseExceptionFilter : IActionFilter, IOrderedFilter
	{
		private readonly ILogger<UserController> _logger;

		public int Order => 10;

		public HttpResponseExceptionFilter(ILogger<UserController> logger)
      {
          _logger = logger;
      }

    public void OnActionExecuting(ActionExecutingContext context)
		{
			MyLog("START", context.RouteData);
		}

		public void OnActionExecuted(ActionExecutedContext context)
		{
			if (context.Exception is HttpResponseException httpResponseException)
			{
				context.Result = new ObjectResult(httpResponseException.Value)
				{
					StatusCode = httpResponseException.StatusCode
				};

				context.ExceptionHandled = true;
			}

			if (context.Exception is not null)
			{
				Log.Debug(context.Exception, "An error occurred");
			}
			MyLog("END", context.RouteData);

		}
		private void MyLog(string methodName, RouteData routeData)
		{
			var controllerName = routeData.Values["controller"];
			var actionName = routeData.Values["action"];
			var message = String.Format("This is custom Log for: {0} controller:{1} action:{2}", methodName, controllerName, actionName);
			_logger.LogDebug(message);

		}

	}
}
