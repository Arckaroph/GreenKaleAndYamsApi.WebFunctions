using GreenKaleAndYamsApi.DataEntities;
using GreenKaleAndYamsApi.TestUtilities.ModelGenerators;
using GreenKaleAndYamsApi.WebFunction.Models;

namespace GreenKaleAndYamsApi.WebFunction.Tests.Models {
	public class ArticleWebModelTests {
		[Fact]
		public void ConstructorWithEntity() {
			// Arrange
			Article entity = EntityGenerator.Default_Article();

			// Act
			ArticleWebModel result = new ArticleWebModel(entity);

			// Assert
			result.Should().BeEquivalentTo(entity, options => options
				.WithMapping<ArticleWebModel>(src => src.ResourceId, dest => dest.Id)
				.Excluding(src => src.DateDeleted)
				.Excluding(src => src.Id)
			);
		}

		[Fact]
		public void ConstructorWithNullEntity() {
			// Arrange
			ArticleWebModel webModel = new ArticleWebModel();

			// Act
			ArticleWebModel result = new ArticleWebModel(null);

			// Assert
			result.Should().BeEquivalentTo(webModel);
		}

		[Fact]
		public void ToModelTests() {
			// Arrange
			ArticleWebModel webModel = WebModelGenerator.Default_ArticleWebModel();

			// Act
			Article result = webModel.ToModel();

			// Assert
			result.Should().BeEquivalentTo(webModel, options => options
				.WithMapping<Article>(src => src.Id, dest => dest.ResourceId)
			);
		}
	}
}
