using System.Collections.Generic;
using AspNetCoreIntegrationTesting.WebApi.Resources;
using AspNetCoreIntegrationTesting.WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreIntegrationTesting.WebApi.Controllers
{
	[Route("api/v1/values")]
	public class ValuesController : ControllerBase
	{
		private readonly ValuesService service;

		public ValuesController(ValuesService service)
		{
			this.service = service;
		}

		[HttpGet]
		public IEnumerable<string> GetAll() => new string[] { "value1", "value2" };

		[HttpGet]
		[Route("{id:int}", Name = "GetValue")]
		public IActionResult GetOneBy(int id)
		{
			var maybeValue = service.GetValueBy(id);

			return maybeValue.HasValue
				? (IActionResult) Content(maybeValue.Value, "text/plain")
				: NotFound();
		}

		[HttpPost]
		public IActionResult Create([FromBody] CreateValueResource value)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			service.Add($"{value.Value}{value.Id}");

			return CreatedAtRoute("GetValue", new { id = value.Id }, value);
		}
	}
}