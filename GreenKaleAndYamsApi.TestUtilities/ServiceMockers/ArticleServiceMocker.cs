using System;
using GreenKaleAndYamsApi.DataEntities;
using GreenKaleAndYamsApi.Domain.Models;
using GreenKaleAndYamsApi.Domain.Services;
using GreenKaleAndYamsApi.TestUtilities.ModelGenerators;
using Moq;

namespace GreenKaleAndYamsApi.TestUtilities.ServiceMockers {
	public class ArticleServiceMocker : IDisposable {
		public readonly Mock<IArticleService> Mock = new Mock<IArticleService>();
		public bool shouldVerifyAll;

		public ArticleServiceMocker(bool shouldVerifyAll = true) {
			this.shouldVerifyAll = shouldVerifyAll;
		}

		public void Dispose() {
			Mock.VerifyAll();
		}


		public Article Setup_GetArticleAsync(Guid? id = null) {
			if (id == null) {
				id = Guid.NewGuid();
			}

			Article article = EntityGenerator.Default_Article(id);

			Mock.Setup(x => x.GetArticleAsync(article.ResourceId))
				.ReturnsAsync(article);
			return article;
		}

		public void Setup_AddArticleAsync() {
			Mock.Setup(x => x.AddArticleAsync(It.IsAny<Article>()))
				.ReturnsAsync((Article x) => x);
		}

		public void Setup_UpdateArticleAsync() {
			Mock.Setup(x => x.UpdateArticleAsync(It.IsAny<Article>()))
				.ReturnsAsync((Article x) => x);
		}

		public PagedResult<Article> Setup_SearchArticlesAsync(int count = 3) {
			PagedResult<Article> pagedResult = EntityGenerator.Default_PagedResultArticle(count);

			Mock.Setup(x => x.SearchArticlesAsync(It.IsAny<ArticleSearchParams>()))
				.ReturnsAsync(pagedResult);
			return pagedResult;
		}

		public void Setup_DeleteArticleAsync(Guid? id = null) {
			if (id == null) {
				Mock.Setup(x => x.DeleteArticleAsync(It.IsAny<Guid>()));
			} else {
				Mock.Setup(x => x.DeleteArticleAsync(id.Value));
			}
		}

		public void VerifyNoCall_GetArticleAsync() {
			Mock.Verify(x => x.GetArticleAsync(It.IsAny<Guid>()), Times.Never);
		}

		public void VerifyNoCall_SearchArticlesAsync() {
			Mock.Verify(x => x.SearchArticlesAsync(It.IsAny<ArticleSearchParams>()), Times.Never);
		}

		public void VerifyNoCall_AddArticleAsync() {
			Mock.Verify(x => x.AddArticleAsync(It.IsAny<Article>()), Times.Never);
		}

		public void VerifyNoCall_UpdateArticleAsync() {
			Mock.Verify(x => x.UpdateArticleAsync(It.IsAny<Article>()), Times.Never);
		}

		public void VerifyNoCall_DeleteArticleAsync() {
			Mock.Verify(x => x.DeleteArticleAsync(It.IsAny<Guid>()), Times.Never);
		}
	}
}
