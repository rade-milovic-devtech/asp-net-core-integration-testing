using FluentAssertions.Execution;
using FluentAssertions.Formatting;
using FluentAssertions.Json;
using FluentAssertions.Primitives;
using Newtonsoft.Json.Linq;
using System.Net.Http;

namespace AspNetCoreIntegrationTesting.Tests
{
	public class HttpResponseMessageAssertions
		: ReferenceTypeAssertions<HttpResponseMessage, HttpResponseMessageAssertions>
	{
		public HttpResponseMessageAssertions(HttpResponseMessage subject)
		{
			Subject = subject;
		}

		protected override string Identifier => nameof(HttpResponseMessage);

		public void ContainJson(string expectedJson, string reason = "", params object[] reasonArgs)
		{
			var contentType = Subject.Content.Headers?.ContentType?.MediaType;
			var stringContent = Subject.Content.ReadAsStringAsync();
			var jsonContent = JToken.Parse(stringContent.Result);
			var expectedJsonContent = JToken.Parse(expectedJson);

			var jsonFormatter = new JTokenFormatter();
			var jsonFormattingContext = new FormattingContext { UseLineBreaks = false };
			var message = "Expected response to have content of type application/json with value " +
				jsonFormatter.Format(expectedJsonContent, jsonFormattingContext, null).Replace("{", "{{").Replace("}", "}}") +
				$", but it had content of type {string.Join(", ", contentType)} and value " +
				jsonFormatter.Format(jsonContent, jsonFormattingContext, null).Replace("{", "{{").Replace("}", "}}");

			Execute.Assertion
				.ForCondition(contentType == "application/json")
				.ForCondition(JToken.DeepEquals(jsonContent, expectedJsonContent))
				.BecauseOf(reason, reasonArgs)
				.FailWith(message);
		}
	}

	public static class HttpResponseMessageExtensions
	{
		public static HttpResponseMessageAssertions Should(this HttpResponseMessage httpResponseMessage) =>
			new HttpResponseMessageAssertions(httpResponseMessage);
	}
}