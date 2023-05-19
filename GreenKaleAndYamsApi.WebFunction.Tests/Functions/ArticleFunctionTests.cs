using GreenKaleAndYamsApi.Domain.Models;
using GreenKaleAndYamsApi.TestUtilities.ModelGenerators;
using GreenKaleAndYamsApi.TestUtilities.ServiceMockers;
using GreenKaleAndYamsApi.WebFunction.Functions;
using GreenKaleAndYamsApi.WebFunction.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace GreenKaleAndYamsApi.WebFunction.Tests.Functions {
	public class ArticleFunctionTests : IDisposable {
		public readonly ArticleFunction articleFunction;
		public readonly ArticleServiceMocker articleServiceMocker = new ArticleServiceMocker();
		public readonly LoggerMocker<ArticleFunction> loggerMocker = new LoggerMocker<ArticleFunction>();

		public ArticleFunctionTests() {
			articleFunction = new ArticleFunction(
				articleServiceMocker.Mock.Object
				//loggerMocker.Mock.Object
			);
		}

		public void Dispose() {
			articleServiceMocker.Dispose();
			loggerMocker.Dispose();
		}

		[Theory]
		[InlineData(null)]
		[InlineData("IsNull")]
		[InlineData("IdNull")]
		[InlineData("IdEmpty")]
		[InlineData("TitleNull")]
		[InlineData("TitleWhitespace")]
		[InlineData("BodyNull")]
		[InlineData("BodyWhitespace")]
		public void IsInvalidArticle_Tests(string state) {
			// Arrange
			ArticleWebModel model = state == "IsNull" ? null : WebModelGenerator.Default_ArticleWebModel();
			if (model != null ) {
				model.Id = state == "IdNull" ? null : state == "IdEmpty" ? Guid.Empty : model.Id;
				model.Title = state == "TitleNull" ? null : state == "TitleWhitespace" ? "  " : model.Title;
				model.Body = state == "BodyNull" ? null : state == "BodyWhitespace" ? "  " : model.Body;
			}

			// Act
			bool result = articleFunction.IsInvalidArticle(model);

			// Assert
			if (state == null) {
				result.Should().BeFalse();
			} else {
				result.Should().BeTrue();
			}
		}

		[Fact]
		public async Task Get_SuccessTest() {
			// Arrange
			DefaultHttpRequest request = new DefaultHttpRequest(new DefaultHttpContext()) {
				Method = "GET",
			};
			Guid id = Guid.NewGuid();
			Article article = articleServiceMocker.Setup_GetArticleAsync(id);

			// Act
			IActionResult response = await articleFunction.Get(request, id, loggerMocker.Mock.Object).ConfigureAwait(false);

			// Assert
			response.Should().BeAssignableTo<OkObjectResult>();
			response.Should().NotBeNull();
			OkObjectResult result = response as OkObjectResult;
			result.Should().NotBeNull();
			result.Value.Should().BeAssignableTo<ArticleWebModel>();
			ArticleWebModel model = result.Value as ArticleWebModel;
			model.Id.Should().Be(id);
			model.Should().BeEquivalentTo(article, options => options
				.WithMapping<ArticleWebModel>(src => src.ResourceId, dest => dest.Id)
				.Excluding(src => src.DateDeleted)
				.Excluding(src => src.Id)
			);
		}

		[Fact]
		public async Task Get_InvlidIdTest() {
			// Arrange
			var request = new DefaultHttpRequest(new DefaultHttpContext()) {
				Method = "GET",
			};

			// Act
			IActionResult response = await articleFunction.Get(request, Guid.Empty, loggerMocker.Mock.Object).ConfigureAwait(false);

			// Assert
			response.Should().BeAssignableTo<BadRequestResult>();
			articleServiceMocker.VerifyNoCall_GetArticleAsync();
		}

		[Fact]
		public async Task Search_SuccessTest() {
			// Arrange
			DefaultHttpRequest request = new DefaultHttpRequest(new DefaultHttpContext()) {
				Method = "GET",
			};
			PagedResult<Article> pagedModel = articleServiceMocker.Setup_SearchArticlesAsync();
			ArticleSearchParams searchParams = WebModelGenerator.Default_ArticleSearchParams();

			// Act
			IActionResult response = await articleFunction.Search(searchParams, request, searchParams.Query, loggerMocker.Mock.Object).ConfigureAwait(false);

			// Assert
			response.Should().BeAssignableTo<OkObjectResult>();
			response.Should().NotBeNull();
			OkObjectResult result = response as OkObjectResult;
			result.Should().NotBeNull();
			result.Value.Should().BeAssignableTo<PagedResult<ArticleWebModel>>();
			PagedResult<ArticleWebModel> pagedResponse = result.Value as PagedResult<ArticleWebModel>;
			pagedResponse.Should().BeEquivalentTo(pagedModel, options => options
				.Excluding(src => src.Results)
			);
			pagedResponse.Results.Should().BeEquivalentTo(pagedModel.Results, options => options
				.WithMapping<ArticleWebModel>(src => src.ResourceId, dest => dest.Id)
				.Excluding(src => src.DateDeleted)
				.Excluding(src => src.Id)
			);
		}

		[Fact]
		public async Task Add_SuccessTest() {
			// Arrange
			DefaultHttpRequest request = new DefaultHttpRequest(new DefaultHttpContext()) {
				Method = "POST",
			};
			ArticleWebModel webModel = WebModelGenerator.Default_ArticleWebModel();
			articleServiceMocker.Setup_AddArticleAsync();

			// Act
			IActionResult response = await articleFunction.Add(webModel, request, loggerMocker.Mock.Object).ConfigureAwait(false);

			// Assert
			response.Should().BeAssignableTo<CreatedResult>();
			response.Should().NotBeNull();
			CreatedResult result = response as CreatedResult;
			result.Should().NotBeNull();
			result.Value.Should().BeAssignableTo<ArticleWebModel>();
			ArticleWebModel model = result.Value as ArticleWebModel;
			model.Should().BeEquivalentTo(webModel);
		}

		[Fact]
		public async Task Add_InvlidArticleTest() {
			// Arrange
			var request = new DefaultHttpRequest(new DefaultHttpContext()) {
				Method = "POST",
			};
			ArticleWebModel webModel = WebModelGenerator.Default_ArticleWebModel();
			webModel.Id = Guid.Empty;

			// Act
			IActionResult response = await articleFunction.Add(webModel, request, loggerMocker.Mock.Object).ConfigureAwait(false);

			// Assert
			response.Should().BeAssignableTo<BadRequestResult>();
			articleServiceMocker.VerifyNoCall_AddArticleAsync();
		}

		[Fact]
		public async Task Update_SuccessTest() {
			// Arrange
			DefaultHttpRequest request = new DefaultHttpRequest(new DefaultHttpContext()) {
				Method = "PUT",
			};
			ArticleWebModel webModel = WebModelGenerator.Default_ArticleWebModel();
			articleServiceMocker.Setup_UpdateArticleAsync();

			// Act
			IActionResult response = await articleFunction.Update(webModel, request, webModel.Id, loggerMocker.Mock.Object).ConfigureAwait(false);

			// Assert
			response.Should().BeAssignableTo<OkObjectResult>();
			response.Should().NotBeNull();
			OkObjectResult result = response as OkObjectResult;
			result.Should().NotBeNull();
			result.Value.Should().BeAssignableTo<ArticleWebModel>();
			ArticleWebModel model = result.Value as ArticleWebModel;
			model.Should().BeEquivalentTo(webModel);
		}

		[Fact]
		public async Task Update_InvlidArticleTest() {
			// Arrange
			var request = new DefaultHttpRequest(new DefaultHttpContext()) {
				Method = "PUT",
			};
			ArticleWebModel webModel = WebModelGenerator.Default_ArticleWebModel();
			webModel.Id = Guid.Empty;

			// Act
			IActionResult response = await articleFunction.Update(webModel, request, webModel.Id, loggerMocker.Mock.Object).ConfigureAwait(false);

			// Assert
			response.Should().BeAssignableTo<BadRequestResult>();
			articleServiceMocker.VerifyNoCall_UpdateArticleAsync();
		}

		[Fact]
		public async Task Update_InvlidIdTest() {
			// Arrange
			var request = new DefaultHttpRequest(new DefaultHttpContext()) {
				Method = "PUT",
			};
			ArticleWebModel webModel = WebModelGenerator.Default_ArticleWebModel();
			webModel.Id = Guid.Empty;

			// Act
			IActionResult response = await articleFunction.Update(webModel, request, Guid.NewGuid(), loggerMocker.Mock.Object).ConfigureAwait(false);

			// Assert
			response.Should().BeAssignableTo<BadRequestResult>();
			articleServiceMocker.VerifyNoCall_UpdateArticleAsync();
		}

		[Fact]
		public async Task Delete_SuccessTest() {
			// Arrange
			DefaultHttpRequest request = new DefaultHttpRequest(new DefaultHttpContext()) {
				Method = "DELETE",
			};
			Guid id = Guid.NewGuid();
			articleServiceMocker.Setup_DeleteArticleAsync(id);

			// Act
			IActionResult response = await articleFunction.Delete(request, id, loggerMocker.Mock.Object).ConfigureAwait(false);

			// Assert
			response.Should().BeAssignableTo<NoContentResult>();
			response.Should().NotBeNull();
			NoContentResult result = response as NoContentResult;
			result.Should().NotBeNull();
		}

		[Fact]
		public async Task Delete_InvlidIdTest() {
			// Arrange
			var request = new DefaultHttpRequest(new DefaultHttpContext()) {
				Method = "DELETE",
			};

			// Act
			IActionResult response = await articleFunction.Delete(request, Guid.Empty, loggerMocker.Mock.Object).ConfigureAwait(false);

			// Assert
			response.Should().BeAssignableTo<BadRequestResult>();
			articleServiceMocker.VerifyNoCall_DeleteArticleAsync();
		}
	}
}
