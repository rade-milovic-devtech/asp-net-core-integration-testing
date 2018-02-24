using CSharpFunctionalExtensions;

namespace AspNetCoreIntegrationTesting.WebApi.Services
{
	public class ValuesService
	{
		public virtual Maybe<string> GetValueBy(int id) =>
			id >= 1 && id <= 10
				? Maybe<string>.From($"value{id}")
				: Maybe<string>.None;

		public virtual void Add(string value) {}
	}
}