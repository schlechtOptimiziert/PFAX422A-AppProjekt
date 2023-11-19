using System.Collections.Generic;
using ClientComponents.Components;
using Microsoft.AspNetCore.Components;
using TransferModel;

namespace MobileClient.Client.Components;

public partial class HorizontalProductScroller : BaseComponent
{
    [Parameter]
    public string Class { get; set; } = default!;

    [Parameter]
    public string Style { get; set; } = default!;

    [Parameter]
    public string Title { get; set; } = default!;

    [Parameter]
    public IEnumerable<Item> Items { get; set; } = default!;

    private void NavigateToList(long id)
        => base.NavigationManager.NavigateTo($"/items/{id}");
}

