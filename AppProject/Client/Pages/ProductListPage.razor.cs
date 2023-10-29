using AppProject.Client.Pages.Components;
using AppProject.Shared;
using Microsoft.AspNetCore.Components;

namespace AppProject.Client.Pages
{
    public partial class ProductListPage : BasePage
    {
        private Platform? platform;
        private IEnumerable<Item> items = Enumerable.Empty<Item>();

        [Parameter]
        public int? PlatformInt { get; set; }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            if (PlatformInt == null)
                throw new ArgumentNullException(nameof(PlatformInt));
            platform = (Platform)PlatformInt;
        }
        protected override async Task OnInitializedAsync()
        {
            await LoadItemsAsync().ConfigureAwait(false);
        }
        private async Task LoadItemsAsync()
            => items = await Service.GetItemsAsync(CancellationToken).ConfigureAwait(false) ?? Enumerable.Empty<Item>();
    }
}