using System.Diagnostics.Contracts;
using System.IO;
using GreenKaleAndYamsApi.Domain.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(GreenKaleAndYamsApi.WebFunction.Startup))]
namespace GreenKaleAndYamsApi.WebFunction {
	internal class Startup : FunctionsStartup {
		public override void Configure(IFunctionsHostBuilder builder) {
			Contract.Requires(builder != null);

			var configBuilder = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("local.settings.json", optional: true, reloadOnChange: true);

			var configuration = configBuilder.Build();

			builder.Services.AddTransient<ArticleService>();
		}
	}
}
