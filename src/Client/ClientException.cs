using System;

namespace AspNetCoreIntegrationTesting.Client
{
	public class ClientException : Exception
	{
		public ClientException(string message, Exception innerException)
			: base(message, innerException) {}
	}
}