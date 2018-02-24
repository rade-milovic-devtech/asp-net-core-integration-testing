using AspNetCoreIntegrationTesting.WebApi.Services;
using Moq;

namespace AspNetCoreIntegrationTesting.Tests
{
	public static class ValuesServiceMockExtensions
	{
		public static void VerifyNewValueIsAdded(this Mock<ValuesService> valuesServiceMock, string expectedValue)
		{
			valuesServiceMock.Verify(service => service.Add(expectedValue), Times.Once);
			valuesServiceMock.ResetCalls();
		}

		public static void VerifyNoValueIsAdded(this Mock<ValuesService> valuesServiceMock)
		{
			valuesServiceMock.Verify(service => service.Add(It.IsAny<string>()), Times.Never);
			valuesServiceMock.ResetCalls();
		}
	}
}