using AspNetCoreIntegrationTesting.WebApi.Services;
using CSharpFunctionalExtensions;
using FluentAssertions;
using Moq;
using System.Net;
using System.Net.Http;
using System.Text;
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

			var response = await client.GetAsync("/api/v1/values/10");

			response.EnsureSuccessStatusCode();
			var content = await response.Content.ReadAsStringAsync();
			content.Should().Be(expectedContent);
		}

		[Fact]
		public async Task ValuesApiRespondsWithNotFoundStatusWhenItIsNotFound()
		{
			valuesServiceMock.Setup(service => service.GetValueBy(It.IsAny<int>()))
				.Returns(Maybe<string>.None);

			var response = await client.GetAsync("/api/v1/values/10");

			response.StatusCode.Should().Be(HttpStatusCode.NotFound);
		}

		[Fact]
		public async Task ValuesApiRespondsWithCreatedStatusWhenItIsSuccessfullyAdded()
		{
			var requestJson = @"
			{
				""id"": 10,
				""value"": ""value""
			}";
			var expectedAddedValue = "value10";

			var requestContent = new StringContent(requestJson, Encoding.UTF8, "application/json");
			var response = await client.PostAsync("/api/v1/values", requestContent);

			response.StatusCode.Should().Be(HttpStatusCode.Created);
			response.Should().ContainJson(requestJson);
			response.Headers.Location.PathAndQuery.Should().Be("/api/v1/values/10");

			valuesServiceMock.VerifyNewValueIsAdded(expectedAddedValue);
		}

		[Fact]
		public async Task ValuesApiRespondsWithBadRequestStatusWhenNewValueDataIsInvalid()
		{
			var requestJson = @"
			{
				""id"": 10
			}";
			var expectedResponseContent = @"
			{
				""Value"": [""Value can't be empty""]
			}";

			var requestContent = new StringContent(requestJson, Encoding.UTF8, "application/json");
			var response = await client.PostAsync("/api/v1/values", requestContent);

			response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
			response.Should().ContainJson(expectedResponseContent);

			valuesServiceMock.VerifyNoValueIsAdded();
		}

		[Fact]
		public async Task ValuesApiRespondsWithBadRequestStatusWhenRequestBodyIsEmpty()
		{
			var expectedResponseContent = @"
			{
				"""": [""A non-empty request body is required.""]
			}";

			var requestContent = new StringContent("", Encoding.UTF8, "application/json");
			var response = await client.PostAsync("/api/v1/values", requestContent);

			response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
			response.Should().ContainJson(expectedResponseContent);

			valuesServiceMock.VerifyNoValueIsAdded();
		}
	}
}