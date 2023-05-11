namespace GreenKaleAndYamsApi.WebFunction.Services {
	public class ArticleSearchParams {
		public string Query { get; set; }
		public int Page { get; set; } = 1;
		public bool IncludeUnpublished { get; set; } = false;
	}
}
