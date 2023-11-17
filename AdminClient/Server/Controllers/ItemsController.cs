using DatabaseDefinition.EntityModel.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TM = TransferModel;

namespace AdminClient.Server.Controllers;

public class ItemsController : ControllerBase
{
    private readonly IItemRepository itemRepository;

    public ItemsController(IItemRepository itemRepository)
    {
        this.itemRepository = itemRepository;
    }

    [HttpPost]
    public Task<long> AddItemAsync([FromBody] TM.Item item, CancellationToken cancellationToken)
        => itemRepository.AddItemAsync(item, cancellationToken);

    [HttpGet]
    public Task<IEnumerable<TM.Item>> GetItemsAsync(CancellationToken cancellationToken)
        => itemRepository.GetItemsAsync(cancellationToken);

    [HttpGet("{itemId}")]
    public Task<TM.Item> GetItemAsync(long itemId, CancellationToken cancellationToken)
        => itemRepository.GetItemAsync(itemId, cancellationToken);

    [HttpPut]
    public Task UpdateItemAsync([FromBody] TM.Item item, CancellationToken cancellationToken)
        => itemRepository.UpdateItemAsync(item, cancellationToken);

    [HttpDelete("{itemId}")]
    public Task DeleteItemAsync(long itemId, CancellationToken cancellationToken)
        => itemRepository.DeleteItemAsync(itemId, cancellationToken);
}