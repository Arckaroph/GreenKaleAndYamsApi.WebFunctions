using GreenKaleAndYamsApi.Data;
using GreenKaleAndYamsApi.DataEntities;
using GreenKaleAndYamsApi.Domain.Models;
using GreenKaleAndYamsApi.Domain.Services;
using GreenKaleAndYamsApi.TestUtilities;
using GreenKaleAndYamsApi.TestUtilities.ModelGenerators;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace GreenKaleAndYamsApi.Domain.Tests.Services {
	public class ArticleServiceTests {
		private readonly DatabaseContextMocker databaseMocker;
		private readonly ArticleService articleService;

		public ArticleServiceTests() {
			databaseMocker = new DatabaseContextMocker();
			databaseMocker.SeedArticles();
			articleService = new ArticleService(databaseMocker.DatabaseContext);
		}

		[Fact]
		public async Task GetArticleAsync_SuccessTests() {
			// Arrange
			List<Article> articles = databaseMocker.DatabaseContext.Articles.ToList();
			int which = RandomValues.GetInt(0, articles.Count - 1);

			// Act
			var response = await articleService.GetArticleAsync(articles[which].ResourceId).ConfigureAwait(false);

			// Assert
			response.Should().NotBeNull();
			response.Should().BeEquivalentTo(articles[which]);
		}

		[Fact]
		public async Task GetArticleAsync_NullTests() {
			// Arrange
			Guid id = Guid.NewGuid();

			// Act
			var response = await articleService.GetArticleAsync(id).ConfigureAwait(false);

			// Assert
			response.Should().BeNull();
		}

		[Fact]
		public async Task AddArticleAsync_SuccessTests() {
			// Arrange
			int countBefore = databaseMocker.DatabaseContext.Articles.Count();
			Article article = EntityGenerator.Default_Article();
			article.Id = 0;

			// Act
			Article response = await articleService.AddArticleAsync(article).ConfigureAwait(false);

			// Assert
			response.Should().NotBeNull();
			response.Should().BeEquivalentTo(article);
			response.Id.Should().NotBe(0);
			countBefore.Should().Be(databaseMocker.DatabaseContext.Articles.Count() - 1);
		}

		[Fact]
		public async Task AddArticleAsync_AlreadyExistsTests() {
			// Arrange
			int countBefore = databaseMocker.DatabaseContext.Articles.Count();
			Article article = databaseMocker.DatabaseContext.Articles.First();

			// Act / Assert
			await articleService.Invoking(x => x.AddArticleAsync(article))
				.Should().ThrowAsync<DbUpdateException>().ConfigureAwait(false);
		}

		[Fact]
		public async Task UpdateArticleAsync_SuccessTests() {
			// Arrange
			Article oldArticle = databaseMocker.DatabaseContext.Articles.First();
			Article article = EntityGenerator.Default_Article();
			article.Id = oldArticle.Id;
			article.ResourceId = oldArticle.ResourceId;
			article.Should().NotBeEquivalentTo(oldArticle);

			// Act
			var response = await articleService.UpdateArticleAsync(article).ConfigureAwait(false);

			// Assert
			response.Should().NotBeNull();
			response.Should().BeEquivalentTo(article);
		}

		[Fact]
		public async Task UpdateArticleAsync_NotFoundTests() {
			// Arrange
			Article article = EntityGenerator.Default_Article();

			// Act / Assert
			await articleService.Invoking(x => x.UpdateArticleAsync(article))
				.Should().ThrowAsync<Exception>().ConfigureAwait(false);
		}

		[Fact]
		public async Task SearchArticlesAsync_NoneFoundTests() {
			// Arrange
			ArticleSearchParams param = WebModelGenerator.Default_ArticleSearchParams();

			// Act
			var response = await articleService.SearchArticlesAsync(param).ConfigureAwait(false);

			// Assert
			response.Should().NotBeNull();
			response.Results.Should().BeEmpty();
		}

		[Fact]
		public async Task SearchArticlesAsync_EmptyQueryTests() {
			// Arrange
			if (databaseMocker.DatabaseContext.Articles.Count() == 0) {
				databaseMocker.SeedArticles();
			}
			ArticleSearchParams param = WebModelGenerator.Default_ArticleSearchParams();
			param.Page = 1;
			param.SearchText = null;

			// Act
			var response = await articleService.SearchArticlesAsync(param).ConfigureAwait(false);

			// Assert
			response.Should().NotBeNull();
			response.Results.Should().HaveCount(2);
		}

		[Fact]
		public async Task SearchArticlesAsync_EmptyQueryIncludeDeletedTests() {
			// Arrange
			databaseMocker.DatabaseContext.Database.EnsureCreated();
			ArticleSearchParams param = WebModelGenerator.Default_ArticleSearchParams();
			param.Page = 1;
			param.SearchText = null;
			param.IncludeDeleted = true;

			// Act
			var response = await articleService.SearchArticlesAsync(param).ConfigureAwait(false);

			// Assert
			response.Should().NotBeNull();
			response.Results.Should().HaveCountGreaterThanOrEqualTo(3, JsonConvert.SerializeObject(param) + JsonConvert.SerializeObject(databaseMocker.DatabaseContext.Articles.Select(x => new {x.Id, x.DatePublished, x.DateDeleted}).ToList()));
		}

		[Fact]
		public async Task SearchArticlesAsync_EmptyQueryIncludeUnpublishedTests() {
			// Arrange
			if (databaseMocker.DatabaseContext.Articles.Count() == 0) {
				databaseMocker.SeedArticles();
			}
			ArticleSearchParams param = WebModelGenerator.Default_ArticleSearchParams();
			param.Page = 1;
			param.SearchText = null;
			param.IncludeUnpublished = true;

			// Act
			var response = await articleService.SearchArticlesAsync(param).ConfigureAwait(false);

			// Assert
			response.Should().NotBeNull();
			response.Results.Should().HaveCountGreaterThanOrEqualTo(3);
		}

		[Fact]
		public async Task DeleteArticleAsync_SuccessTests() {
			// Arrange
			DateTime now = DateTime.Now.AddHours(-1);
			Article article = EntityGenerator.Default_Article();
			await databaseMocker.DatabaseContext.AddAsync(article).ConfigureAwait(false);
			await databaseMocker.DatabaseContext.SaveChangesAsync().ConfigureAwait(false);
			article.DateDeleted.Should().BeNull();

			// Act
			await articleService.DeleteArticleAsync(article.ResourceId).ConfigureAwait(false);

			// Assert
			article.DateDeleted.Should().BeAfter(now);
		}

		[Fact]
		public async Task DeleteArticleAsync_NotFoundTests() {
			// Arrange
			Guid id = Guid.NewGuid();

			// Act / Assert
			await articleService.Invoking(x => x.DeleteArticleAsync(id))
				.Should().ThrowAsync<Exception>().ConfigureAwait(false);
		}
	}
}
