using AspNetCoreIntegrationTesting.WebApi.Services;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using AspNetCoreIntegrationTesting.WebApi;
using Autofac;
using Microsoft.Extensions.Logging;

namespace AspNetCoreIntegrationTesting.Tests
{
	public class WebApiFixture : IDisposable
	{
		private readonly TestServer _server;

		public WebApiFixture()
		{
			CorrelationIdMiddlewareLoggerMock = new Mock<ILogger<CorrelationIdMiddleware>>();

			ValuesServiceMock = new Mock<ValuesService>();

			var builder = new WebHostBuilder()
				.ConfigureServices(services =>
				{
					var containerBuilder = new ContainerBuilder();

					containerBuilder.Register(_ => CorrelationIdMiddlewareLoggerMock.Object)
						.As<ILogger<CorrelationIdMiddleware>>()
						.InstancePerLifetimeScope();

					containerBuilder.Register(_ => ValuesServiceMock.Object)
						.InstancePerLifetimeScope();

					services.AddSingleton(containerBuilder);
				})
				.UseStartup<Startup>();

			_server = new TestServer(builder);
			Client = _server.CreateClient();
		}

		public Mock<ILogger<CorrelationIdMiddleware>> CorrelationIdMiddlewareLoggerMock { get; }

		public Mock<ValuesService> ValuesServiceMock { get; }

		public HttpClient Client { get; }

		public void Dispose()
		{
			Client.Dispose();
			_server.Dispose();
		}
	}
}