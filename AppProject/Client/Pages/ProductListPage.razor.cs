using AppProject.Client.Components;
using AppProject.Shared;
using Microsoft.AspNetCore.Components;

namespace AppProject.Client.Pages
{
    public partial class ProductListPage : BasePage
    {
        private Platform? platform;
        private IEnumerable<Item> items = Enumerable.Empty<Item>();

        [Parameter]
        public string? PlatformString { get; set; }

        [Parameter]
        public string? SearchText { get; set; }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            if (PlatformString == null)
                throw new ArgumentNullException(nameof(PlatformString));
            platform = (Platform)Enum.Parse(typeof(Platform), PlatformString);
        }
        protected override async Task OnInitializedAsync()
        {
            await LoadItemsAsync().ConfigureAwait(false);
        }
        private async Task LoadItemsAsync()
        {
            items = await Service.GetItemsAsync(CancellationToken).ConfigureAwait(false) ?? Enumerable.Empty<Item>();
            if (SearchText != null)
            {
                items = items.Where(i => i.Name.ToLower().Contains(SearchText.ToLower()));
            }
        }
    }
}