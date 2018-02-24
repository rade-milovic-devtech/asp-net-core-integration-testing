using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace AspNetCoreIntegrationTesting.Tests
{
	public class SimpleControllerTests : IClassFixture<WebApiFixture>
	{
		private readonly HttpClient client;

		public SimpleControllerTests(WebApiFixture fixture)
		{
			client = fixture.Client;
		}

		[Fact]
		public async Task ValuesApiRespondsWithArrayOfSampleValues()
		{
			var expectedContent = "[\"value1\", \"value2\"]";

			var response = await client.GetAsync("/api/v1/values");

			response.Should().ContainJson(expectedContent);
		}
	}
}