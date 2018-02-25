using System.Net.Http;
using System.Threading.Tasks;
using AspNetCoreIntegrationTesting.WebApi;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

using static AspNetCoreIntegrationTesting.WebApi.CorrelationIdMiddleware;

namespace AspNetCoreIntegrationTesting.Tests
{
	public class MiddlewareTests : IClassFixture<WebApiFixture>
	{
		private readonly Mock<ILogger<CorrelationIdMiddleware>> loggerMock;

		private readonly HttpClient client;

		public MiddlewareTests(WebApiFixture fixture)
		{
			loggerMock = fixture.CorrelationIdMiddlewareLoggerMock;

			client = fixture.Client;
		}

		[Fact]
		public async Task ReturnsSameCorrelationIdInResponseIfItIsSetInRequest()
		{
			var requestCorrelationId = "0bdb10c1-3878-40d6-805a-35b0e056b950";

			var request = new HttpRequestMessage(HttpMethod.Get, "/api/v1/values");
			request.Headers.Add(CorrelationIdHeader, requestCorrelationId);
			var response = await client.SendAsync(request);

			response.Should().HaveCorrelationIdWithValue(requestCorrelationId);
			loggerMock.VerifyDebugMessageIsWritten();
		}

		[Fact]
		public async Task ReturnsCorrelationIdInResponseIfItIsNotSetInRequest()
		{
			var response = await client.GetAsync("/api/v1/values");

			response.Should().HaveCorrelationId();
			loggerMock.VerifyDebugMessageIsWritten();
		}

		[Fact]
		public async Task CorrelationIdIsLoggedForEveryRequest()
		{
			var requestCorrelationId = "0bdb10c1-3878-40d6-805a-35b0e056b950";

			var request = new HttpRequestMessage(HttpMethod.Get, "/api/v1/values");
			request.Headers.Add(CorrelationIdHeader, requestCorrelationId);
			await client.SendAsync(request);

			loggerMock.VerifyDebugMessageIsWrittenContaining(requestCorrelationId);
		}
	}
}