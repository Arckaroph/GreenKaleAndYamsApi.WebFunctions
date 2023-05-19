using System;
using GreenKaleAndYamsApi.Domain.Models;
using GreenKaleAndYamsApi.WebFunction.Models;

namespace GreenKaleAndYamsApi.TestUtilities.ModelGenerators {
	public static class WebModelGenerator {

		public static ArticleWebModel Default_ArticleWebModel() {
			return new ArticleWebModel() {
				Id = RandomValues.GetGuid(),
				Title = RandomValues.GetSentence(3),
				Body = RandomValues.GetSentence(25),
				DatePublished = RandomValues.GetDate(new DateTime(2010, 1, 1)),
			};
		}

		public static ArticleSearchParams Default_ArticleSearchParams() {
			return new ArticleSearchParams() {
				Query = RandomValues.GetSentence(2),
				Page = RandomValues.GetInt(1, 3),
				IncludeUnpublished = false,
			};
		}
	}
}
