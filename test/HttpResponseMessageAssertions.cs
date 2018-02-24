using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;
using Newtonsoft.Json.Linq;
using System.Net.Http;

namespace AspNetCoreIntegrationTesting.Tests
{
	public class HttpResponseMessageAssertions : ObjectAssertions
	{
		public HttpResponseMessageAssertions(object value) : base(value) {}

		public void ContainJson(string expectedJson, string reason = "", params object[] reasonArgs)
		{
			var response = Subject as HttpResponseMessage;
			var contentType = response.Content.Headers?.ContentType?.MediaType;

			Execute.Assertion
				.ForCondition(response.IsSuccessStatusCode)
				.ForCondition(contentType == "application/json")
				.BecauseOf(reason, reasonArgs)
				.FailWith("Expected response to be successful and have content of type \"application/json\", " +
					$"but it had status code of {response.StatusCode} and content types \"{string.Join(", ", contentType)}\"");

			var stringContent = response.Content.ReadAsStringAsync();
			var jsonContent = JToken.Parse(stringContent.Result);
			jsonContent.Should().BeEquivalentTo(JToken.Parse(expectedJson));
		}
	}

	public static class HttpResponseMessageExtensions
	{
		public static HttpResponseMessageAssertions Should(this HttpResponseMessage httpResponseMessage) =>
			new HttpResponseMessageAssertions(httpResponseMessage);
	}
}