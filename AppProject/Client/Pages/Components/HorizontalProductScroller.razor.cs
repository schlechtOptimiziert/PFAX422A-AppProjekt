using AppProject.Shared;
using Microsoft.AspNetCore.Components;

namespace AppProject.Client.Pages.Components
{
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
}