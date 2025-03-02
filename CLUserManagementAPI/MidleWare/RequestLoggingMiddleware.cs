using System.Diagnostics;

namespace CLUserManagementAPI.MidleWare
{
	public class RequestLoggingMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ILogger<RequestLoggingMiddleware> _logger;

		public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
		{
			_next = next;
			_logger = logger;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			// Bắt đầu đo thời gian xử lý request
			var stopwatch = Stopwatch.StartNew();

			try
			{
				// Ghi log thông tin request
				_logger.LogInformation($"Incoming Request: {context.Request.Method} {context.Request.Path}");

				// Chuyển request đến middleware tiếp theo
				await _next(context);

				// Ghi log thông tin response
				_logger.LogInformation($"Outgoing Response: {context.Response.StatusCode} - Elapsed: {stopwatch.ElapsedMilliseconds}ms");
			}
			catch (Exception ex)
			{
				// Ghi log lỗi nếu có
				_logger.LogError(ex, $"An error occurred while processing the request: {context.Request.Method} {context.Request.Path}");
				throw; // Re-throw để middleware xử lý lỗi tiếp theo có thể bắt được
			}
			finally
			{
				stopwatch.Stop();
			}
		}
	}
}
