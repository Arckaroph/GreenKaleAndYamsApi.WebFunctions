using System.IO;
using GreenKaleAndYamsApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace GreenKaleAndYamsApi.WebFunction.Data {
	public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<DatabaseContext> {
		public DatabaseContext CreateDbContext(string[] args) {
			IConfigurationRoot configuration = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
				.Build();

			var builder = new DbContextOptionsBuilder<DatabaseContext>();

			builder.UseSqlServer(configuration.GetSection("GreenKaleAndYams").GetValue<string>("ConnectionString"));

			return new DatabaseContext(builder.Options);
		}
	}
}
