namespace GreenKaleAndYamsApi.Domain.Models {
	public class ArticleSearchParams {
		public string Query { get; set; }
		public int Page { get; set; } = 1;
		public bool IncludeUnpublished { get; set; } = false;
		public bool IncludeDeleted { get; set; } = false;
	}
}
