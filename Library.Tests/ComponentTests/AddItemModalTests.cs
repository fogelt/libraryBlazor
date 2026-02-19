using Bunit;
using Library.Web.Components.UI;
using Library.Core.DTOs;
using Moq;
using Microsoft.Extensions.DependencyInjection;
using Library.Core.Interfaces;

public class AddItemModalTests : BunitContext
{
  [Fact]
  public void Modal_ShouldChangeLabel_WhenCategorySwitchesToDVD()
  {
    var mockService = new Mock<ILibraryService>();
    Services.AddSingleton(mockService.Object);

    var cut = Render<AddItemModal>();

    var dvdOption = cut.FindAll(".dropdown-item").First(x => x.TextContent.Contains("DVD"));
    dvdOption.Click();

    Assert.Contains("Seconds", cut.Markup);
  }

  [Fact]
  public async Task FormSubmit_ShouldCallService_WithCorrectData()
  {
    var mockService = new Mock<ILibraryService>();
    bool saveTriggered = false;

    mockService.Setup(s => s.AddItemFromDtoAsync(It.IsAny<AddLibraryItemDto>(), It.IsAny<string>()))
               .ReturnsAsync(true);

    Services.AddSingleton(mockService.Object);

    var cut = Render<AddItemModal>(parameters => parameters
        .Add(p => p.OnSave, () => saveTriggered = true)
    );

    cut.Find("input[placeholder*='978']").Change("12345"); // ISBN
    cut.Find("input[placeholder*='Enter title']").Change("Test Movie"); // Title
    cut.Find("input[placeholder*='Name']").Change("Test Author"); // Author

    cut.Find("textarea").Change("This is a required description.");

    cut.FindAll("input[type='number']")[0].Change("2024");

    await cut.Find("form").SubmitAsync();

    mockService.Verify(s => s.AddItemFromDtoAsync(
        It.Is<AddLibraryItemDto>(dto => dto.Title == "Test Movie"),
        It.IsAny<string>()),
        Times.Once);
    Assert.True(saveTriggered);
  }

  [Fact]
  public async Task Form_ShouldShowError_WhenISBNMissing()
  {
    var mockService = new Mock<ILibraryService>();
    Services.AddSingleton(mockService.Object);

    var cut = Render<AddItemModal>();

    cut.Find("input[placeholder*='Enter title']").Change("Test Movie");
    cut.Find("input[placeholder*='Name']").Change("Test Author");
    cut.Find("textarea").Change("A valid description.");

    await cut.Find("form").SubmitAsync();

    mockService.Verify(s => s.AddItemFromDtoAsync(It.IsAny<AddLibraryItemDto>(), It.IsAny<string>()),
        Times.Never);

    var validationMessage = cut.Find(".text-danger.small");
    Assert.Equal("ISBN is required", validationMessage.TextContent);
  }

  [Fact]
  public void ImagePreview_ShouldShowIcon_WhenUrlIsEmpty()
  {
    var mockService = new Mock<ILibraryService>();
    Services.AddSingleton(mockService.Object);

    var cut = Render<AddItemModal>();

    Assert.NotNull(cut.Find(".bi-image"));
  }
}