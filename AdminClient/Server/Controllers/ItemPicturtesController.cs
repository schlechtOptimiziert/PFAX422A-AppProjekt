using DatabaseDefinition.EntityModel.Repositories.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using TM = TransferModel;

namespace AdminClient.Server.Controllers;

[Route("api/Items")]
public class ItemPicturesController : ControllerBase
{
    private readonly IItemPictureRepository itemPictureRepository;
    private readonly IWebHostEnvironment env;

    public ItemPicturesController(IItemPictureRepository itemPictureRepository, IWebHostEnvironment env)
    {
        this.itemPictureRepository = itemPictureRepository;
        this.env = env;
    }

    [HttpPost("{itemId}/Pictures")]
    public async Task<IEnumerable<long>> AddItemPicturesAsync(List<IFormFile> files, long itemId, CancellationToken cancellationToken)
    {
        var uploadPictureIds = new List<long>();

        foreach (var file in files)
        {
            var path = Path.Combine(env.ContentRootPath, "..", "..", "uploads", file.FileName);

            await using var fs = new FileStream(path, FileMode.Create);
            await file.CopyToAsync(fs, cancellationToken);

            uploadPictureIds.Add(await itemPictureRepository.AddItemPictureAsync(new TM.ItemPicture {ItemId = itemId, FileName = file.FileName}, cancellationToken));
        }

        return uploadPictureIds;
    }

    [HttpGet("{itemId}/Pictures")]
    public Task<IEnumerable<TM.ItemPicture>> GetItemPicturesAsync(long itemId, CancellationToken cancellationToken)
        => itemPictureRepository.GetItemPicturesAsync(itemId, cancellationToken);

    [HttpDelete("{itemId}/Pictures/{id}")]
    public Task DeleteItemAsync(long id, CancellationToken cancellationToken)
        => itemPictureRepository.DeleteItemPictureAsync(id, cancellationToken);
}
