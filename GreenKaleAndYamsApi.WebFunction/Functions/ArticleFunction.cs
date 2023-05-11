using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using GreenKaleAndYamsApi.WebFunction.Models;
using GreenKaleAndYamsApi.WebFunction.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace GreenKaleAndYamsApi.WebFunction.Functions {
	public class ArticleFunction {
		private readonly ArticleService articleService;

		public ArticleFunction(
			ArticleService articleService
		) {
			this.articleService = articleService;
		}

		internal bool IsInvalidArticle(ArticleWebModel model) {
			return model == null
				|| string.IsNullOrWhiteSpace(model.Title)
				|| string.IsNullOrWhiteSpace(model.Body)
				|| model.Id == null || model.Id == Guid.Empty;
		}

		[FunctionName("ArticleGet")]
		public async Task<IActionResult> Get(
			[HttpTrigger(AuthorizationLevel.Function, "get", Route = "Article/{id}")]
			HttpRequest req,
			[FromRoute] Guid id,
			ILogger log
		) {
			log.LogInformation("C# HTTP trigger function processed a request: {Method} in {Service}", req.Method, this.GetType());

			if (id == Guid.Empty) {
				// throw new ArgumentInvalidException() // TODO: implement
				throw new Exception("Required argument value is null or empty");
			}

			Article article = articleService.GetArticleAsync(id);
			ArticleWebModel response = new ArticleWebModel(article);
			//Article article = await articleService.GetArticleAsync(id).ConfigureAwait(false);

			return new OkObjectResult(response);
		}

		[FunctionName("ArticleSearch")]
		public async Task<IActionResult> Search(
			[HttpTrigger(AuthorizationLevel.Function, "get", Route = "Articles")]
			[FromQuery] ArticleSearchParams param,
			HttpRequest req,
			[FromQuery] string query,
			ILogger log
		) {
			log.LogInformation("C# HTTP trigger function processed a request: {Method} in {Service}", req.Method, this.GetType());

			if (string.IsNullOrWhiteSpace(param.Query)) {
				// throw new ArgumentInvalidException() // TODO: implement
				throw new Exception("Required argument value is null or empty");
			}
			if (param.Page < 1) {
				param.Page = 1;
			}

			PagedResult<Article> results = articleService.SearchArticlesAsync(param);
			////PagedResult<Article> results = await articleService.SearchArticlesAsync(param).ConfigureAwait(false);
			PagedResult<ArticleWebModel> response = new PagedResult<ArticleWebModel>() {
				Results = results.Results.Select(x => new ArticleWebModel(x)).ToList(),
				TotalResults = results.TotalResults,
				TotalPages = results.TotalPages,
				Page = results.Page,
			};

			return new OkObjectResult(response);
		}

		[FunctionName("ArticleAdd")]
		public async Task<IActionResult> Add(
			[HttpTrigger(AuthorizationLevel.Function, "post", Route = "Article")]
			[FromBody] ArticleWebModel webModel,
			HttpRequest req,
			ILogger log
		) {
			log.LogInformation("C# HTTP trigger function processed a request: {Method} in {Service}", req.Method, this.GetType());

			if (IsInvalidArticle(webModel)) {
				// throw new ArgumentInvalidException() // TODO: implement
				throw new Exception("Required argument value is null or empty");
			}

			Article article = articleService.AddArticleAsync(webModel.ToModel());
			//Article article = await articleService.AddArticleAsync(webModel.ToModel()).ConfigureAwait(false);
			ArticleWebModel response = new ArticleWebModel(article);

			return new OkObjectResult(response);
		}

		[FunctionName("ArticleUpdate")]
		public async Task<IActionResult> Update(
			[HttpTrigger(AuthorizationLevel.Function, "put", Route = "Article/{id}")]
			[FromBody] ArticleWebModel webModel,
			HttpRequest req,
			[FromRoute] Guid? id,
			ILogger log
		) {
			log.LogInformation("C# HTTP trigger function processed a request: {Method} in {Service}", req.Method, this.GetType());
			if (webModel.Id == null) {
				webModel.Id = id;
			}

			if (IsInvalidArticle(webModel)) {
				// throw new ArgumentInvalidException() // TODO: implement
				throw new Exception("Required argument value is null or empty");
			}
			if (webModel.Id != id) {
				// throw new ArgumentInvalidException() // TODO: implement
				throw new Exception("Ids do not match");
			}

			Article article = articleService.UpdateArticleAsync(webModel.ToModel());
			//Article article = await articleService.UpdateArticleAsync(webModel.ToModel()).ConfigureAwait(false);
			ArticleWebModel response = new ArticleWebModel(article);

			return new OkObjectResult(response);
		}

		[FunctionName("ArticleDelete")]
		public async Task<IActionResult> Delete(
			[HttpTrigger(AuthorizationLevel.Function, "delete", Route = "Article/{id}")]
			HttpRequest req,
			[FromRoute] Guid id,
			ILogger log
		) {
			log.LogInformation("C# HTTP trigger function processed a request: {Method} in {Service}", req.Method, this.GetType());

			if (id == Guid.Empty) {
				// throw new ArgumentInvalidException() // TODO: implement
				throw new Exception("Required argument value is null or empty");
			}

			articleService.DeleteArticleAsync(id);
			//await articleService.DeleteArticleAsync(id).ConfigureAwait(false);
			return new NoContentResult();
		}
	}
}
