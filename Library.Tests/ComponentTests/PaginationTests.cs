using Bunit;
using Library.Web.Components.UI;
using Library.Core.Models.Items;

public class LibraryItemCardTests : BunitContext
{
  [Fact]
  public void Pagination_ShouldDisplayCorrectPageInfo()
  {
    using var ctx = new BunitContext();

    var cut = ctx.Render<Pagination>(parameters => parameters
        .Add(p => p.CurrentPage, 2)
        .Add(p => p.TotalPages, 5)
    );

    var span = cut.Find("span");
    span.MarkupMatches(@"<span class=""text-white opacity-75 small uppercase fw-bold"" style=""letter-spacing: 0.1em;"">
                            Page 2 of 5
                         </span>");
  }
  [Fact]
  public void Pagination_ShouldDisableButtonsAtBoundaries()
  {
    using var ctx = new BunitContext();

    var cut = ctx.Render<Pagination>(parameters => parameters
        .Add(p => p.CurrentPage, 1)
        .Add(p => p.TotalPages, 3)
    );

    var buttons = cut.FindAll("button");
    Assert.True(buttons[0].HasAttribute("disabled"));
    Assert.False(buttons[1].HasAttribute("disabled"));
  }
  [Fact]
  public void Pagination_ClickingNext_ShouldTriggerCallbackWithIncrementedPage()
  {
    using var ctx = new BunitContext();
    int signaledPage = 0;

    var cut = ctx.Render<Pagination>(parameters => parameters
        .Add(p => p.CurrentPage, 2)
        .Add(p => p.TotalPages, 5)
        .Add(p => p.OnPageChanged, (int page) => signaledPage = page)
    );

    var nextButton = cut.FindAll("button")[1];
    nextButton.Click();

    Assert.Equal(3, signaledPage);
  }
}