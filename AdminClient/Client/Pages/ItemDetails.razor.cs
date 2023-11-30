using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using TransferModel;

namespace AdminClient.Client.Pages;

partial class ItemDetails : BasePage
{
    MudForm form;
    Color badgeColor = Color.Success;
    string badgeIcon = Icons.Material.Outlined.Lock;

    private Item item = new();
    private IEnumerable<string> fileNames = Enumerable.Empty<string>();
    private IEnumerable<Platform> platforms = Enumerable.Empty<Platform>();


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
            item = await GetItemAsync().ConfigureAwait(false);
            platforms = await GetPlatformsAsync().ConfigureAwait(false);
        }

        IsLoading = false;
    }

    private async Task SaveItem()
    {
        await form.Validate().ConfigureAwait(false);
        if (!form.IsValid)
            return;

        IsLoading = true;
        if (IsCreate)
        {
            var itemId = await AddItemAsync().ConfigureAwait(false);
            NavigationManager.NavigateTo($"/items/{itemId}", true);
            return;
        }
        else
        {
            await UpdateItem().ConfigureAwait(false);
        }

        item = await GetItemAsync().ConfigureAwait(false);
        badgeColor = Color.Success;
        badgeIcon = Icons.Material.Outlined.Lock;
        IsLoading = false;
    }
    private async Task UpdateItem()
        => await Service.UpdateItemAsync(item, CancellationToken).ConfigureAwait(false);

    private async Task<long> AddItemAsync()
        => await Service.AddItemAsync(item, CancellationToken).ConfigureAwait(false);

    private async Task<Item> GetItemAsync()
    {
        if (Id.HasValue)
            return await Service.GetItemAsync(Id.Value, CancellationToken).ConfigureAwait(false) ?? throw new ArgumentException($"Could not find item with id '{Id}'");
        return null;
    }

    private async Task<IEnumerable<Platform>> GetPlatformsAsync()
        => await Service.GetPlatformsAsync(CancellationToken).ConfigureAwait(false);

    private async Task AddPlatformToItem(long platformId)
    {
        if (Id.HasValue)
            await Service.AddPlatformToItemAsync(Id.Value, platformId, CancellationToken).ConfigureAwait(false);
        item = await GetItemAsync().ConfigureAwait(false);
    }

    private void FieldChanged()
    {
        badgeColor = Color.Error;
        badgeIcon = Icons.Material.Filled.LockOpen;
    }

    private async Task RemovePlatformFromItem(long platformId)
    {
        if (Id.HasValue)
            await Service.RemovePlatformFromItemAsync(Id.Value, platformId, CancellationToken).ConfigureAwait(false);
        item = await GetItemAsync().ConfigureAwait(false);
    }

    private async Task DeleteItemAsync()
    {
        await Service.DeleteItemAsync(item.Id, CancellationToken);
        NavigationManager.NavigateTo("/Items");
    }

    private async Task OnInputFileChange(InputFileChangeEventArgs e)
    {
        int maxAllowedFiles = int.MaxValue;
        long maxFileSize = long.MaxValue;

        using var content = new MultipartFormDataContent();

        foreach (var file in e.GetMultipleFiles(maxAllowedFiles))
        {
            var fileContent = new StreamContent(file.OpenReadStream(maxFileSize));
            fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);

            content.Add(
                content: fileContent,
                name: "\"files\"",
                fileName: file.Name);
        }

        var uploadFileNames = await Service.UploadFiles(content, CancellationToken).ConfigureAwait(false);
        if(uploadFileNames != null)
            fileNames = fileNames.Concat(uploadFileNames);
    }
}