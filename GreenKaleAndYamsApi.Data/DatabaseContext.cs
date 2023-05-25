using GreenKaleAndYamsApi.DataEntities;
using Microsoft.EntityFrameworkCore;

namespace GreenKaleAndYamsApi.Data {
	public class DatabaseContext : DbContext {
		public DatabaseContext(
			DbContextOptions<DatabaseContext> options
		) : base(options) { }

		public DbSet<Article> Articles { get; set; }
	}
}
