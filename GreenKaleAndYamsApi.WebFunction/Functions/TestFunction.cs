using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace GreenKaleAndYamsApi.WebFunction.Functions {
	public static class TestFunction {
		[FunctionName("OnlyGet")]
		public static IActionResult OnlyGet(
			[HttpTrigger(AuthorizationLevel.Function, "get", Route = "Function")] HttpRequest req,
			ILogger log
		) {
			log.LogInformation("C# HTTP trigger OnlyGet");

			string name = req.Query["name"];
			name = string.IsNullOrWhiteSpace(name) ? null : name;

			string method = req.Method;

			string responseMessage = $"OnlyGet :: {method} :: name = {name}";

			return new OkObjectResult(responseMessage);
		}

		[FunctionName("OnlyPost")]
		public static async Task<IActionResult> OnlyPost(
			[HttpTrigger(AuthorizationLevel.Function, "post", Route = "Function")] HttpRequest req,
			ILogger log
		) {
			log.LogInformation("C# HTTP trigger OnlyPost");

			string name = req.Query["name"];
			name = string.IsNullOrWhiteSpace(name) ? null : name;

			string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
			dynamic data = JsonConvert.DeserializeObject(requestBody);
			string nameBody = data?.name;
			nameBody = string.IsNullOrWhiteSpace(nameBody) ? null : nameBody;

			string method = req.Method;

			string responseMessage = $"OnlyPost :: {method} :: name = {name} :: nameBody = {nameBody}";

			return new OkObjectResult(responseMessage);
		}

		[FunctionName("FunctionParam")]
		public static async Task<IActionResult> FunctionParam(
			[HttpTrigger(AuthorizationLevel.Function, "get", "post", "put", "delete", Route = null)] HttpRequest req,
			ILogger log
		) {
			log.LogInformation("C# HTTP trigger FunctionParam");

			string name = req.Query["name"];
			name = string.IsNullOrWhiteSpace(name) ? null : name;

			string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
			dynamic data = JsonConvert.DeserializeObject(requestBody);
			string nameBody = data?.name;
			nameBody = string.IsNullOrWhiteSpace(nameBody) ? null : nameBody;

			string method = req.Method;

			string responseMessage = $"FunctionParam :: {method} :: name = {name} :: nameBody = {nameBody}";

			return new OkObjectResult(responseMessage);
		}

		[FunctionName("FunctionUrlParam")]
		public static async Task<IActionResult> FunctionUrlParam(
			[HttpTrigger(AuthorizationLevel.Function, "get", "post", "put", "delete", Route = "FunctionUrlParam/{id}")] HttpRequest req,
			int id,
			ILogger log
		) {
			log.LogInformation("C# HTTP trigger FunctionUrlParam");

			string name = req.Query["name"];
			name = string.IsNullOrWhiteSpace(name) ? "null" : name;

			string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
			dynamic data = JsonConvert.DeserializeObject(requestBody);
			string nameBody = data?.name;
			nameBody = string.IsNullOrWhiteSpace(nameBody) ? "null" : nameBody;

			string method = req.Method;

			string responseMessage = $"FunctionUrlParam :: {method} :: name = {name} :: nameBody = {nameBody}";

			return new OkObjectResult(responseMessage);
		}
	}
}
