using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GreenKaleAndYamsApi.DataEntities {
	[Table("Articles")]
	public class Article {
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		[Required]
		public Guid ResourceId { get; set; }

		[Required]
		[StringLength(250)]
		public string Title { get; set; }

		[Required]
		public string Body { get; set; }

		//[Required]
		//[ForeignKey("AuthorId")]
		//public Subject Author { get; set; }

		public DateTime? DatePublished { get; set; }

		public DateTime? DateDeleted { get; set; }
	}
}
