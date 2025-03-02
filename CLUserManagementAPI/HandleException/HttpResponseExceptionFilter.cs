using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using CLUserManagementAPI.Controllers;
using System.Diagnostics;
using Serilog;

namespace CLUserManagementAPI.HandleException
{
	public class HttpResponseExceptionFilter : IActionFilter, IOrderedFilter
	{
		public int Order => int.MaxValue - 10;


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
			var message = String.Format("{0} controller:{1} action:{2}", methodName, controllerName, actionName);
			Log.Debug(message);

		}

	}
}
