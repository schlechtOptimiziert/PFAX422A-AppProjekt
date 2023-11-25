using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using TransferModel;

namespace AdminClient.Client.Pages;

partial class PlatformDetails : BasePage
{
    MudForm form;
    Color badgeColor = Color.Success;
    string badgeIcon = Icons.Material.Outlined.Lock;

    private Platform platform = new();

    [Parameter]
    public long? Id { get; set; }

    protected override async Task OnInitializedAsync()
    {
        IsLoading = true;
        await base.OnInitializedAsync().ConfigureAwait(false);

        if (Id == null)
            IsCreate = true;

        if (!IsCreate)
        {
            platform = await GetPlatformAsync().ConfigureAwait(false);
        }

        IsLoading = false;
    }

    private async Task SavePlatform()
    {
        await form.Validate().ConfigureAwait(false);
        if (!form.IsValid)
            return;

        IsLoading = true;
        if (IsCreate)
        {
            var platformId = await AddPlatformAsync().ConfigureAwait(false);
            NavigationManager.NavigateTo($"/platforms/{platformId}", true);
            return;
        }
        else
        {
            await UpdatePlatform().ConfigureAwait(false);
        }

        platform = await GetPlatformAsync().ConfigureAwait(false);
        badgeColor = Color.Success;
        badgeIcon = Icons.Material.Outlined.Lock;
        IsLoading = false;
    }
    private async Task UpdatePlatform()
        => await Service.UpdatePlatformAsync(platform, CancellationToken).ConfigureAwait(false);

    private async Task<long> AddPlatformAsync()
        => await Service.AddPlatformAsync(platform, CancellationToken).ConfigureAwait(false);

    private async Task<Platform> GetPlatformAsync()
    {
        if (Id.HasValue)
            return await Service.GetPlatformAsync(Id.Value, CancellationToken).ConfigureAwait(false) ?? throw new ArgumentException($"Could not find platform with id '{Id}'");
        return null;
    }

    private void FieldChanged()
    {
        badgeColor = Color.Error;
        badgeIcon = Icons.Material.Filled.LockOpen;
    }

    private async Task DeletePlatformAsync()
    {
        if (Id.HasValue)
            await Service.DeletePlatformAsync(Id.Value, CancellationToken);
        else
            throw new ArgumentException("Platform id has no value");
        NavigationManager.NavigateTo("/Platforms");
    }
}
