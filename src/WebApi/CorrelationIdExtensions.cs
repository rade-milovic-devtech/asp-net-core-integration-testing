using Microsoft.AspNetCore.Builder;

namespace AspNetCoreIntegrationTesting.WebApi
{
	public static class CorrelationIdExtensions
	{
		public static IApplicationBuilder UseCorrelationId(this IApplicationBuilder app) =>
			app.UseMiddleware<CorrelationIdMiddleware>();
	}
}