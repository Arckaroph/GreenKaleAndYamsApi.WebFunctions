using System;
using System.Diagnostics.Contracts;
using System.IO;
using GreenKaleAndYamsApi.Data;
using GreenKaleAndYamsApi.Domain.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(GreenKaleAndYamsApi.WebFunction.Startup))]
namespace GreenKaleAndYamsApi.WebFunction {
	internal class Startup : FunctionsStartup {
		public override void Configure(IFunctionsHostBuilder builder) {
			Contract.Requires(builder != null);

			IConfigurationRoot configuration = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
				.Build();

			builder.Services.AddDbContext<DatabaseContext>(
				options => options.UseSqlServer(
					configuration.GetSection("GreenKaleAndYams").GetValue<string>("ConnectionString"),
					builder => {
						builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
						builder.CommandTimeout(10);
					}
				)
			);

			builder.Services.AddTransient<IArticleService, ArticleService>();
			builder.Services.AddTransient<DbContext, DatabaseContext>();
		}
	}
}
