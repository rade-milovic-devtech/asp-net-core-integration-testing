using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreIntegrationTesting.WebApi.Controllers
{
	[Route("api/[controller]")]
	public class ValuesController : ControllerBase
	{
		[HttpGet]
		public IEnumerable<string> Get() => new string[] { "value1", "value2" };
	}
}