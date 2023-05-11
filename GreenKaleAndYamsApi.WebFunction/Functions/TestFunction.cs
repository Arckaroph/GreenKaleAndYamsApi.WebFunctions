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

	public class TestFunction {

		[FunctionName("OnlyGet")]
		public IActionResult OnlyGet(
			[HttpTrigger(AuthorizationLevel.Function, "get", Route = "Function")] HttpRequest req,
			ILogger log
		) {
			log.LogInformation("C# HTTP trigger OnlyGet");

			string name = req.Query["name"];
			name = string.IsNullOrWhiteSpace(name) ? null : name;

			string method = req.Method;

			string responseMessage = $"OnlyGet\nmethod       = {method}\nname         = {name}";

			return new OkObjectResult(responseMessage);
		}

		[FunctionName("OnlyPost")]
		public async Task<IActionResult> OnlyPost(
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

			string responseMessage = $"OnlyPost\nmethod       = {method}\nname         = {name}\nname in body = {nameBody}";

			return new OkObjectResult(responseMessage);
		}
		public class BodyClass {
			public string Name { get; set; }
			public string FullName { get; set; }
			public int Num { get; set; }
		}

		[FunctionName("FunctionCombined")]
		public async Task<IActionResult> Function(
			[HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
			ILogger log
		) {
			log.LogInformation("C# HTTP trigger Function Combined");

			string name = req.Query["name"];
			name = string.IsNullOrWhiteSpace(name) ? null : name;

			string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
			dynamic data = JsonConvert.DeserializeObject(requestBody);
			string nameBody = data?.name;
			nameBody = string.IsNullOrWhiteSpace(nameBody) ? null : nameBody;

			string method = req.Method;

			string responseMessage = $"Function Combined\nmethod       = {method}\nname         = {name}\nname in body = {nameBody}";

			return new OkObjectResult(responseMessage);
		}

		[FunctionName("FunctionUrlParam")]
		public async Task<IActionResult> FunctionUrlParam(
			[HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "Function/{id}")]
			[FromBody] BodyClass body,
			HttpRequest req,
			[FromRoute] int id,
			[FromQuery] string name,
			ILogger log
		) {
			log.LogInformation("C# HTTP trigger Function with URL params");

			name = string.IsNullOrWhiteSpace(name) ? "null" : name;

			string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
			dynamic data = JsonConvert.DeserializeObject(requestBody);
			string nameBody = body.FullName;
			nameBody = string.IsNullOrWhiteSpace(nameBody) ? "null" : nameBody;

			string method = req.Method;
			
			string responseMessage = $"Function with URL params\nmethod      {id} = {method}\nname         = {name}\nname in body = {nameBody}\nnum in body  ={body.Num}";

			return new OkObjectResult(responseMessage);
		}
	}
}
