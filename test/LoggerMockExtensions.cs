using AspNetCoreIntegrationTesting.WebApi;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Internal;
using Moq;
using System;
using System.Linq;

namespace AspNetCoreIntegrationTesting.Tests
{
	public static class LoggerMockExtensions
	{
		public static void VerifyDebugMessageIsWritten(
			this Mock<ILogger<CorrelationIdMiddleware>> loggerMock)
		{
			loggerMock.Verify(logger =>
				logger.Log(
					LogLevel.Debug,
					0,
					It.IsAny<FormattedLogValues>(),
					null,
					It.IsAny<Func<object, Exception, string>>()),
				Times.Once);
			loggerMock.ResetCalls();
		}

		public static void VerifyDebugMessageIsWrittenContaining(
			this Mock<ILogger<CorrelationIdMiddleware>> loggerMock,
			string subString)
		{
			loggerMock.Verify(logger =>
				logger.Log(
					LogLevel.Debug,
					0,
					It.Is<FormattedLogValues>(formattedLogValues =>
						formattedLogValues.Any(pair =>
							pair.Value.ToString().Contains(subString))),
					null,
					It.IsAny<Func<object, Exception, string>>()),
				Times.Once);
			loggerMock.ResetCalls();
		}
	}
}