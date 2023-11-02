using AppProject.Server.EntityModel.Repositories;
using AppProject.Server.EntityModel.Repositories.Interfaces;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;

namespace AppProject.Server.Controllers;

[Route("FirstApp/api/Items")]
[Route("SecondApp/api/Items")]
public class ItemPicturesController : ControllerBase
{
    private readonly IItemPictureRepository itemPictureRepository;

    public ItemPicturesController(IItemPictureRepository itemPictureRepository)
    {
        this.itemPictureRepository = itemPictureRepository;
    }

    [HttpPost("{itemId}/Pictures")]
    public Task<long> AddItemPictureAsync([FromBody] Shared.ItemPicture picture, CancellationToken cancellationToken)
        => itemPictureRepository.AddItemPictureAsync(picture, cancellationToken);

    [HttpGet("{itemId}/Pictures")]
    public Task<IEnumerable<Shared.ItemPicture>> GetItemPicturesAsync(long itemId, CancellationToken cancellationToken)
    {
        Console.WriteLine("Test");
        return itemPictureRepository.GetItemPicturesAsync(itemId, cancellationToken);
    }
}
