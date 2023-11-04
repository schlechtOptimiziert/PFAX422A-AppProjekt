using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppProject.Shared;

public class ItemPicture
{
    public long Id { get; set; }
    public long ItemId { get; set; }
    public byte[] Bytes { get; set; }
    public string Description { get; set; }
    public string FileExtension { get; set; }
    public decimal Size { get; set; }

    public static async Task<ItemPicture> BrowserFileToItemPictureAsync(IBrowserFile file, long ItemId, CancellationToken cancellationToken)
    {
        byte[] buffer = new byte[2097152];
        var stream = file.OpenReadStream(2097152, cancellationToken);
        using var memoryStream = new MemoryStream();

        while (await stream.ReadAsync(buffer, cancellationToken).ConfigureAwait(false) > 0)
            await memoryStream.WriteAsync(buffer, cancellationToken).ConfigureAwait(false);

        return new ItemPicture
        {
            ItemId = ItemId,
            Bytes = memoryStream.ToArray(),
            Description = file.Name,
            FileExtension = file.ContentType,
            Size = file.Size,
        };
    }

    public static string ItemPictureToUri(ItemPicture picture)
        => string.Format("data:image/png;base64,{0}", Convert.ToBase64String(picture.Bytes));
}
