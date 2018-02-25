using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace AspNetCoreIntegrationTesting.WebApi
{
	public class Startup
	{
		private readonly ContainerBuilder containerBuilder;

		public Startup(ContainerBuilder containerBuilder)
		{
			this.containerBuilder = containerBuilder;
		}

		public IServiceProvider ConfigureServices(IServiceCollection services)
		{
			services.AddMvcCore()
				.AddFormatterMappings()
				.AddJsonFormatters()
				.AddDataAnnotations()
				.AddCors();

			containerBuilder.Populate(services);

			return new AutofacServiceProvider(containerBuilder.Build());
		}

		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseCorrelationId()
				.UseMvc();
		}
	}
}