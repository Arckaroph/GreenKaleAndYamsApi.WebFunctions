using System;
using System.ComponentModel.DataAnnotations;
using GreenKaleAndYamsApi.DataEntities;

namespace GreenKaleAndYamsApi.WebFunction.Models {
	public class ArticleWebModel {
		public Guid? Id { get; set; }
		[Required]
		public string Title { get; set; }
		[Required]
		public string Body { get; set; }
		//public SubjectWebModel Author { get; set; }
		public DateTime? DatePublished { get; set; }

		public ArticleWebModel() { }

		public ArticleWebModel(Article model) {
			if (model == null) {
				return;
			}
			Id = model.ResourceId;
			Title = model.Title;
			Body = model.Body;
			//Author = new SubjectWebModel(model.Author);
			DatePublished = model.DatePublished;
		}

		public Article ToModel() {
			return new Article() {
				ResourceId = Id ?? Guid.NewGuid(),
				Title = Title,
				Body = Body,
				//Author = Author?.ToModel(),
				DatePublished = DatePublished,
			};
		}
	}
}
