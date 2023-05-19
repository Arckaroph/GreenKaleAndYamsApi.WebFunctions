using System;
using GreenKaleAndYamsApi.Domain.Models;

namespace GreenKaleAndYamsApi.Domain.Services {
	public interface IArticleService {
		Article GetArticleAsync(Guid id);
		PagedResult<Article> SearchArticlesAsync(ArticleSearchParams param);
		Article AddArticleAsync(Article model);
		Article UpdateArticleAsync(Article model);
		void DeleteArticleAsync(Guid id);
	}
}