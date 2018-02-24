using AspNetCoreIntegrationTesting.WebApi.Services;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Net.Http;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using AspNetCoreIntegrationTesting.WebApi;
using Autofac;

namespace AspNetCoreIntegrationTesting.Tests
{
	public class WebApiFixture : IDisposable
	{
		private readonly TestServer _server;

		public WebApiFixture()
		{
			ValuesServiceMock = new Mock<ValuesService>();

			var builder = new WebHostBuilder()
				.ConfigureServices(services =>
				{
					var containerBuilder = new ContainerBuilder();
					containerBuilder.Register(_ => ValuesServiceMock.Object).InstancePerLifetimeScope();

					services.AddSingleton(containerBuilder);
				})
				.UseStartup<Startup>();

			_server = new TestServer(builder);
			Client = _server.CreateClient();
		}

		public Mock<ValuesService> ValuesServiceMock { get; }

		public HttpClient Client { get; }

		public void Dispose()
		{
			Client.Dispose();
			_server.Dispose();
		}
	}
}