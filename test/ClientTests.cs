using AspNetCoreIntegrationTesting.Client;
using FluentAssertions;
using RichardSzalay.MockHttp;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace AspNetCoreIntegrationTesting.Tests
{
	public class ClientTests : IDisposable
	{
		private readonly MockHttpMessageHandler mockHttp;

		private readonly ValuesClient client;

		public ClientTests()
		{
			mockHttp = new MockHttpMessageHandler();

			var httpClient = mockHttp.ToHttpClient();
			httpClient.BaseAddress = new Uri("http://localhost");
			client = new ValuesClient(httpClient);
		}

		public void Dispose()
		{
			client?.Dispose();
		}

		[Fact]
		public async Task GetsAllTheValues()
		{
			var expectedValues = new[] { "value1", "value2" };

			mockHttp.When("http://localhost/api/v1/values")
				.Respond("application/json", "[\"value1\", \"value2\"]");

			var values = await client.GetAll();

			values.Should().BeEquivalentTo(expectedValues);
		}

		[Fact]
		public void ThrowsExceptionWhenRequestIsNotSuccessful()
		{
			mockHttp.When("http://localhost/api/v1/values")
				.Respond(HttpStatusCode.InternalServerError);

			Func<Task> getValues = async () => await client.GetAll();

			getValues.Should().Throw<ClientException>();
		}
	}
}