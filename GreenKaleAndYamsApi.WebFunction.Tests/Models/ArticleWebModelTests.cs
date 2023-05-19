using GreenKaleAndYamsApi.Domain.Models;
using GreenKaleAndYamsApi.TestUtilities.ModelGenerators;
using GreenKaleAndYamsApi.WebFunction.Models;

namespace GreenKaleAndYamsApi.WebFunction.Tests.Models {
	public class ArticleWebModelTests {
		[Fact]
		public void ConstructorWithEntity() {
			// Arrange
			Article entity = EntityGenerator.Default_Article();

			// Act
			ArticleWebModel model = new ArticleWebModel(entity);

			// Assert
			model.Should().BeEquivalentTo(entity, options => options
				.WithMapping<ArticleWebModel>(src => src.ResourceId, dest => dest.Id)
				.Excluding(src => src.DateDeleted)
				.Excluding(src => src.Id)
			);

		}

		[Fact]
		public void ToModelTests() {
			// Arrange
			ArticleWebModel model = WebModelGenerator.Default_ArticleWebModel();

			// Act
			Article entity = model.ToModel();

			// Assert
			entity.Should().BeEquivalentTo(model, options => options
				.WithMapping<Article>(src => src.Id, dest => dest.ResourceId)
			);
		}
	}
}
