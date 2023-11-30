using DatabaseDefinition.EntityModel.Repositories.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using TM = TransferModel;

namespace MobileClient.Server.Controllers;

[Route("api/Items")]
public class ItemPicturesController : ControllerBase
{
    private readonly IItemPictureRepository itemPictureRepository;

    public ItemPicturesController(IItemPictureRepository itemPictureRepository, IWebHostEnvironment env)
    {
        this.itemPictureRepository = itemPictureRepository;
    }

    [HttpGet("{itemId}/Pictures")]
    public Task<IEnumerable<TM.ItemPicture>> GetItemPicturesAsync(long itemId, CancellationToken cancellationToken)
        => itemPictureRepository.GetItemPicturesAsync(itemId, cancellationToken);

    [HttpGet("{itemId}/Pictures/CoverPicture")]
    public Task<TM.ItemPicture> GetItemCoverPictureAsync(long itemId, CancellationToken cancellationToken)
        => itemPictureRepository.GetItemCoverPictureAsync(itemId, cancellationToken);
}
