using Bunit;
using Library.Web.Components.UI;
using Library.Web.Services;
using Library.Core.DTOs;
using Moq;
using Microsoft.Extensions.DependencyInjection;

public class AddItemModalTests : BunitContext
{
  [Fact]
  public void Modal_ShouldChangeLabel_WhenCategorySwitchesToDVD()
  {
    var mockService = new Mock<LibraryService>(null!, null!);
    Services.AddSingleton(mockService.Object);

    var cut = Render<AddItemModal>();

    var dvdOption = cut.FindAll(".dropdown-item").First(x => x.TextContent.Contains("DVD"));
    dvdOption.Click();

    var label = cut.Find("label.small.opacity-50.mb-1:nth-of-type(1)");
    Assert.Contains("Seconds", cut.Markup);
  }

  [Fact]
  public async Task FormSubmit_ShouldCallService_WithCorrectData()
  {
    var mockService = new Mock<LibraryService>(null!, null!);
    bool saveTriggered = false;

    mockService.Setup(s => s.AddItemFromDtoAsync(It.IsAny<LibraryItemDto>(), It.IsAny<string>()))
               .ReturnsAsync(true);

    Services.AddSingleton(mockService.Object);

    var cut = Render<AddItemModal>(parameters => parameters
        .Add(p => p.OnSave, () => saveTriggered = true)
    );

    cut.Find("input[placeholder*='e.g. 978']").Change("12345");
    cut.Find("input[placeholder*='Enter title']").Change("Test Movie");

    await cut.Find("form").SubmitAsync();

    mockService.Verify(s => s.AddItemFromDtoAsync(
        It.Is<LibraryItemDto>(dto => dto.Title == "Test Movie"),
        It.IsAny<string>()),
        Times.Once);
    Assert.True(saveTriggered);
  }

  [Fact]
  public void ImagePreview_ShouldShowIcon_WhenUrlIsEmpty()
  {
    var mockService = new Mock<LibraryService>(null!, null!);
    Services.AddSingleton(mockService.Object);
    var cut = Render<AddItemModal>();

    Assert.NotNull(cut.Find(".bi-image"));
  }
}