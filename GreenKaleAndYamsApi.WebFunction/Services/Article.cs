using System;

namespace GreenKaleAndYamsApi.WebFunction.Services {
	public class Article {
		public int Id { get; set; }

		public Guid ResourceId { get; set; }

		public string Title { get; set; }

		public string Body { get; set; }

		public DateTime? DatePublished { get; set; }

		public DateTime? DateDeleted { get; set; }
	}
}
