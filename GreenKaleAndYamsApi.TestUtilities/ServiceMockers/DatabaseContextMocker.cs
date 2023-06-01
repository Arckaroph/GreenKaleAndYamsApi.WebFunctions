using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GreenKaleAndYamsApi.Data;
using GreenKaleAndYamsApi.DataEntities;
using GreenKaleAndYamsApi.TestUtilities.ModelGenerators;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace GreenKaleAndYamsApi.Domain.Tests {
	public class DatabaseContextMocker : IDisposable {
		public readonly DatabaseContext DatabaseContext;

		public DatabaseContextMocker() {
			var connection = new SqliteConnection("Filename=:memory:");
			connection.Open();

			DbContextOptions<DatabaseContext> options = new DbContextOptionsBuilder<DatabaseContext>()
				.UseSqlite(connection)
				.Options;
			DatabaseContext = new DatabaseContext(options);
			DatabaseContext.Database.EnsureDeleted();
			DatabaseContext.Database.EnsureCreated();
		}

		public void Dispose() {
		}

		public List<Article> SeedArticles() {
			DatabaseContext.Database.EnsureDeleted();
			DatabaseContext.Database.EnsureCreated();
			List<Article> articles = EntityGenerator.Default_Articles(4);
			articles[1].DatePublished = DateTime.Now.AddDays(4);
			articles[3].DateDeleted = DateTime.Now.AddDays(-4);
			DatabaseContext.Articles.AddRange(articles);
			DatabaseContext.SaveChanges();
			return articles;
		}
	}
}
