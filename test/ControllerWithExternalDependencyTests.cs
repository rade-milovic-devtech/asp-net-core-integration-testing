using AspNetCoreIntegrationTesting.WebApi.Services;
using CSharpFunctionalExtensions;
using FluentAssertions;
using Moq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace AspNetCoreIntegrationTesting.Tests
{
	public class ControllerWithExternalDependencyTests : IClassFixture<WebApiFixture>
	{
		private readonly Mock<ValuesService> valuesServiceMock;

		private readonly HttpClient client;

		public ControllerWithExternalDependencyTests(WebApiFixture fixture)
		{
			client = fixture.Client;
			valuesServiceMock = fixture.ValuesServiceMock;
		}

		[Fact]
		public async Task ValuesApiRespondsWithAValueWhenItIsFound()
		{
			var expectedContent = "secretValue";

			valuesServiceMock.Setup(service => service.GetValueBy(10))
				.Returns(Maybe<string>.From("secretValue"));

			var response = await client.GetAsync("/api/values/10");

			response.EnsureSuccessStatusCode();
			var content = await response.Content.ReadAsStringAsync();
			content.Should().Be(expectedContent);
		}

		[Fact]
		public async Task ValuesApiRespondsWithNotFoundStatusWhenItIsNotFound()
		{
			valuesServiceMock.Setup(service => service.GetValueBy(It.IsAny<int>()))
				.Returns(Maybe<string>.None);

			var response = await client.GetAsync("/api/values/10");

			response.StatusCode.Should().Be(HttpStatusCode.NotFound);
		}
	}
}