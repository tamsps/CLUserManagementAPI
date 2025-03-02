using System.Net;
using System.Text.Json;

namespace CLUserManagementAPI.MidleWare
{
	public class ExceptionHandlingMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ILogger<ExceptionHandlingMiddleware> _logger;

		public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
		{
			_next = next;
			_logger = logger;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			try
			{
				await _next(context); // Chuyển request đến middleware tiếp theo
			}
			catch (Exception ex)
			{
				// Ghi log lỗi
				_logger.LogError(ex, "An unhandled exception has occurred.");

				// Xử lý response
				context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
				context.Response.ContentType = "application/json";

				// Trả về thông báo lỗi dưới dạng JSON
				var response = new
				{
					error = "An error occurred while processing your request.",
					details = ex.Message // Chỉ trả về message cho client, không trả về stack trace
				};

				await context.Response.WriteAsync(JsonSerializer.Serialize(response));
			}
		}
	}
}
