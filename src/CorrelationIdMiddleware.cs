using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System;
using System.Threading.Tasks;

namespace AspNetCoreIntegrationTesting.WebApi
{
	public class CorrelationIdMiddleware
	{
		public const string CorrelationIdHeader = "X-Correlation-ID";

		private readonly ILogger<CorrelationIdMiddleware> logger;
		private readonly RequestDelegate next;

		public CorrelationIdMiddleware(ILogger<CorrelationIdMiddleware> logger, RequestDelegate next)
		{
			this.logger = logger;
			this.next = next;
		}

		public Task Invoke(HttpContext context)
		{
			if (context.Request.Headers.TryGetValue(CorrelationIdHeader, out StringValues correlationId))
			{
				context.TraceIdentifier = correlationId;
			}
			else
			{
				context.TraceIdentifier = Guid.NewGuid().ToString();
			}

			logger.LogDebug($"Received request with correlation id {context.TraceIdentifier}");

			context.Response.OnStarting(() =>
			{
				context.Response.Headers.Add(CorrelationIdHeader, new[] { context.TraceIdentifier });
				return Task.CompletedTask;
			});

			return next(context);
		}
	}
}