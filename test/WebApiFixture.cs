using AspNetCoreIntegrationTesting.WebApi;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Net.Http;

namespace AspNetCoreIntegrationTesting.Tests
{
	public class WebApiFixture : IDisposable
	{
		private readonly TestServer _server;

		public WebApiFixture()
		{
			var builder = new WebHostBuilder()
				.UseStartup<Startup>();

			_server = new TestServer(builder);
			Client = _server.CreateClient();
		}

		public HttpClient Client { get; }

		public void Dispose()
		{
			Client.Dispose();
			_server.Dispose();
		}
	}
}