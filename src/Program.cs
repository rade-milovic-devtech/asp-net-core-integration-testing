using AspNetCoreIntegrationTesting.WebApi.Services;
using Autofac;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetCoreIntegrationTesting.WebApi
{
	public class Program
	{
		public static void Main(string[] args)
		{
			BuildWebHost(args).Run();
		}

		public static IWebHost BuildWebHost(string[] args) =>
			WebHost.CreateDefaultBuilder(args)
				.ConfigureServices(services =>
				{
					var containerBuilder = new ContainerBuilder();
					containerBuilder.RegisterType<ValuesService>().InstancePerLifetimeScope();

					services.AddSingleton(containerBuilder);
				})
				.UseStartup<Startup>()
				.Build();
	}
}