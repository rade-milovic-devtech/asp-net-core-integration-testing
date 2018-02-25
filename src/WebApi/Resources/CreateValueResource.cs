using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIntegrationTesting.WebApi.Resources
{
	public class CreateValueResource
	{
		public int Id { get; set; }
		[Required(ErrorMessage = "Value can't be empty", AllowEmptyStrings = false)]
		public string Value { get; set; }
	}
}