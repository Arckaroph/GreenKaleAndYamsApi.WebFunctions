using System;
using System.Linq;
using System.Collections.Generic;

namespace GreenKaleAndYamsApi.WebFunction.Services {
	public class ArticleService {
		public class Data {
			public List<Article> Articles { get; set; }
		}

		private readonly Data database = new Data() {
			Articles = new List<Article> {
				new Article() {
					Title = "Article Title",
					Body = "asdf dfk eka fid fieek dk",
					Id = 1,
					ResourceId = new Guid("e072505d-244e-4fe6-9572-7bef516d86aa"),
					DatePublished = DateTime.Now,
				}
			}
		};

		public ArticleService() {
		}

		public Article GetArticleAsync(Guid id) {
			return database.Articles
				.FirstOrDefault(x => x.ResourceId == id);
		}

		public Article AddArticleAsync(Article model) {
			database.Articles.Add(model);
			return model;
		}

		public Article UpdateArticleAsync(Article model) {
			Article entity = database.Articles
				.FirstOrDefault(x => x.ResourceId == model.ResourceId);
			entity.Title = model.Title;
			entity.Body = model.Body;
			entity.DatePublished = model.DatePublished;
			return entity;
		}

		public PagedResult<Article> SearchArticlesAsync(ArticleSearchParams param) {
			PagedResult<Article> result = new PagedResult<Article>() {
				Results = database.Articles,
				TotalResults = 0,
				TotalPages = 1,
				Page = param.Page,
			};

			return result;
		}

		public void DeleteArticleAsync(Guid id) {
		}
	}
}
