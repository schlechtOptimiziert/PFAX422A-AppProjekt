﻿using Microsoft.AspNetCore.Mvc;
using DatabaseDefinition.EntityModel.Repositories.Interfaces;
using TM = TransferModel;
using System.Threading;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MobileClient.Server.Controllers;

[Route("api/Items")]
public class ItemPicturesController : ControllerBase
{
    private readonly IItemPictureRepository itemPictureRepository;

    public ItemPicturesController(IItemPictureRepository itemPictureRepository)
    {
        this.itemPictureRepository = itemPictureRepository;
    }

    [HttpPost("{itemId}/Pictures")]
    public Task<long> AddItemPictureAsync([FromBody] TM.ItemPicture picture, CancellationToken cancellationToken)
        => itemPictureRepository.AddItemPictureAsync(picture, cancellationToken);

    [HttpGet("{itemId}/Pictures")]
    public Task<IEnumerable<TM.ItemPicture>> GetItemPicturesAsync(long itemId, CancellationToken cancellationToken)
        => itemPictureRepository.GetItemPicturesAsync(itemId, cancellationToken);

    [HttpDelete("{itemId}/Pictures/{id}")]
    public Task DeleteItemAsync(long id, CancellationToken cancellationToken)
        => itemPictureRepository.DeleteItemPictureAsync(id, cancellationToken);
}
