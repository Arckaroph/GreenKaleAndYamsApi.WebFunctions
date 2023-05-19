using System;
using System.Collections.Generic;
using GreenKaleAndYamsApi.Domain.Models;

namespace GreenKaleAndYamsApi.TestUtilities.ModelGenerators {
	public static class EntityGenerator {
		public static Article Default_Article(Guid? id = null) {
			return new Article() {
				Id = RandomValues.GetInt(),
				ResourceId = id ?? RandomValues.GetGuid(),
				Title = RandomValues.GetSentence(3),
				Body = RandomValues.GetSentence(25),
				DatePublished = RandomValues.GetDate(new DateTime(2010, 1, 1)),
			};
		}

		public static PagedResult<Article> Default_PagedResultArticle(int count = 3) {
			int totalPages = RandomValues.GetInt(3, 6);
			PagedResult<Article> pagedArticles = new PagedResult<Article>() {
				Results = new List<Article>(count * 3),
				Page = RandomValues.GetInt(1, totalPages),
				TotalPages = totalPages,
				TotalResults = totalPages * count,
			};
			for (int i = 0; i < count; i++) {
				pagedArticles.Results.Add(Default_Article());
			}
			return pagedArticles;
		}
	}
}
