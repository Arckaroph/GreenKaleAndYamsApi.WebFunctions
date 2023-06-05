using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GreenKaleAndYamsApi.Data;
using GreenKaleAndYamsApi.DataEntities;
using GreenKaleAndYamsApi.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace GreenKaleAndYamsApi.Domain.Services {
	public class ArticleService : IArticleService {
		private readonly DatabaseContext databaseContext;

		public ArticleService(
			DatabaseContext databaseContext
		) {
			this.databaseContext = databaseContext;
		}

		public Task<Article> GetArticleAsync(Guid id) {
			return databaseContext.Articles
				.AsNoTracking()
				.FirstOrDefaultAsync(x => x.ResourceId == id);
		}

		public async Task<Article> AddArticleAsync(Article model) {
			var workEntity = await databaseContext.Articles
				.AddAsync(model).ConfigureAwait(false);
			await databaseContext.SaveChangesAsync().ConfigureAwait(false);
			model.Id = workEntity.Entity.Id;
			return model;
		}

		public async Task<Article> UpdateArticleAsync(Article model) {
			Article entity = await databaseContext.Articles
				//.Include(x => x.Author)
				.FirstOrDefaultAsync(x => x.ResourceId == model.ResourceId).ConfigureAwait(false);
			if (entity == null) {
				//throw new NotFoundException(); // TODO: implement
				throw new Exception("Not Found Exception");
			}

			if (!string.IsNullOrWhiteSpace(model.Title)) {
				entity.Title = model.Title;
			}
			if (!string.IsNullOrWhiteSpace(model.Body)) {
				entity.Body = model.Body;
			}
			//if (model.Author != null && model.Author.Id != entity.Author.Id) {
			//	Subject author = await databaseContext.Subjects
			//		.FirstOrDefaultAsync(x => x.SubjectId == model.Author.SubjectId).ConfigureAwait(false);
			//	if (author == null) {
			//		//throw new NotFoundException(); // TODO: implement
			//		throw new Exception("Not Found Exception");
			//	}
			//	entity.Author = author;
			//}
			if (model.DatePublished != null) {
				entity.DatePublished = model.DatePublished;
			}
			if (model.DateDeleted == null) {
				entity.DateDeleted = model.DateDeleted;
			}

			await databaseContext.SaveChangesAsync().ConfigureAwait(false);
			return entity;
		}

		public async Task<PagedResult<Article>> SearchArticlesAsync(ArticleSearchParams param) {
			IQueryable<Article> query = databaseContext.Articles;

			if (!param.IncludeDeleted) {
				query = query.Where(x => x.DateDeleted == null);
			}
			if (!param.IncludeUnpublished) {
				query = query.Where(x => x.DatePublished < DateTime.Now);
			}
			if (!string.IsNullOrWhiteSpace(param.SearchText)) {
				param.SearchText = param.SearchText.Trim();
				//param.SearchText = param.SearchText.Replace(" ", " OR ");
				//query = query.Where(x => EF.Functions.Contains(x.Title, $"\"{param.SearchText}\"") || EF.Functions.Contains(x.Body, $"\"{param.SearchText}\""));
				query = query.Where(x => x.Title.Contains(param.SearchText) || x.Body.Contains(param.SearchText));
			}

			int totalResults = await query
				.CountAsync().ConfigureAwait(false);

			int resultsPerPage = 10; // TODO: to appsettings.json
			List<Article> articles = await query
				.Skip(resultsPerPage * (param.Page - 1))
				.Take(resultsPerPage)
				.AsNoTracking()
				.ToListAsync().ConfigureAwait(false);

			PagedResult<Article> result = new PagedResult<Article>() {
				Results = articles,
				TotalResults = totalResults,
				TotalPages = (int)Math.Ceiling(totalResults / (double)resultsPerPage),
				Page = param.Page,
			};

			return result;
		}

		public async Task DeleteArticleAsync(Guid id) {
			Article entity = await databaseContext.Articles
				.FirstOrDefaultAsync(x => x.ResourceId == id).ConfigureAwait(false);
			if (entity == null) {
				throw new Exception("Not Found Exception"); // TODO: exception not found message
			}

			entity.DateDeleted = DateTime.Now;
			await databaseContext.SaveChangesAsync().ConfigureAwait(false);
		}
	}
}
