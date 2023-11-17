using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using TransferModel;
using MobileClient.Client.Pages;

namespace MobileClient.Client.Components;

public partial class HorizontalProductScroller
{
    [Parameter]
    public string Class { get; set; } = default!;
    [Parameter]
    public string Style { get; set; } = default!;
    [Parameter]
    public string Title { get; set; } = default!;

    [Parameter]
    public IEnumerable<Item> Items { get; set; } = default!;




}
