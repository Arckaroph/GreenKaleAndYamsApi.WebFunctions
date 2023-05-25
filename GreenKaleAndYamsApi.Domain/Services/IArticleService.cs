using System;
using System.Threading.Tasks;
using GreenKaleAndYamsApi.DataEntities;
using GreenKaleAndYamsApi.Domain.Models;

namespace GreenKaleAndYamsApi.Domain.Services {
	public interface IArticleService {
		Task<Article> GetArticleAsync(Guid id);
		Task<PagedResult<Article>> SearchArticlesAsync(ArticleSearchParams param);
		Task<Article> AddArticleAsync(Article model);
		Task<Article> UpdateArticleAsync(Article model);
		Task DeleteArticleAsync(Guid id);
	}
}