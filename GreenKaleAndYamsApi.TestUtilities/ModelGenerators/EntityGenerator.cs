using System;
using System.Collections.Generic;
using GreenKaleAndYamsApi.DataEntities;
using GreenKaleAndYamsApi.Domain.Models;

namespace GreenKaleAndYamsApi.TestUtilities.ModelGenerators {
	public static class EntityGenerator {
		public static Article Default_Article(Guid? id = null) {
			return new Article() {
				Id = RandomValues.GetInt(),
				ResourceId = id ?? RandomValues.GetGuid(),
				Title = RandomValues.GetSentence(3),
				Body = RandomValues.GetSentence(25),
				DatePublished = RandomValues.GetDate(new DateTime(2010, 1, 1), DateTime.Now.AddDays(-1)),
			};
		}
		public static List<Article> Default_Articles(int count = 3) {
			List<Article> articles = new List<Article>(count * 3);
			for (int i = 0; i < count; i++) {
				articles.Add(Default_Article());
			}
			return articles;
		}

		public static PagedResult<Article> Default_PagedResultArticle(int count = 3) {
			int totalPages = RandomValues.GetInt(3, 6);
			PagedResult<Article> pagedArticles = new PagedResult<Article>() {
				Results = Default_Articles(count),
				Page = RandomValues.GetInt(1, totalPages),
				TotalPages = totalPages,
				TotalResults = totalPages * count,
			};
			return pagedArticles;
		}
	}
}
