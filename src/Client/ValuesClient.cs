using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace AspNetCoreIntegrationTesting.Client
{
	public class ValuesClient : IDisposable
	{
		private readonly HttpClient httpClient;

		public ValuesClient(string baseUrl)
		{
			if (string.IsNullOrWhiteSpace(baseUrl))
				throw new ArgumentException("can't be empty", nameof(baseUrl));

			httpClient = new HttpClient
			{
				BaseAddress = new Uri(baseUrl)
			};
		}

		public ValuesClient(HttpClient httpClient)
		{
			this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
		}

		public async Task<IEnumerable<string>> GetAll()
		{
			try
			{
				var response = await httpClient.GetAsync("/api/v1/values");
				response.EnsureSuccessStatusCode();

				var content = await response.Content.ReadAsStringAsync();

				return JsonConvert.DeserializeObject<IEnumerable<string>>(content);
			}
			catch(Exception ex)
			{
				throw new ClientException("Unknown error occurred while getting all the values.", ex);
			}
		}

		public void Dispose()
		{
			httpClient?.Dispose();
		}
	}
}