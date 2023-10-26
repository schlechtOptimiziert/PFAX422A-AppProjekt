﻿using AppProject.Server.EntityModel.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AppProject.Server.Controllers;

public class ItemsController : ControllerBase
{
    private readonly IItemRepository itemRepository;

    public ItemsController(IItemRepository itemRepository)
    {
        this.itemRepository = itemRepository;
    }

    [HttpPost]
    public Task<long> AddItemAsync([FromBody] Shared.Item item, CancellationToken cancellationToken)
        => itemRepository.AddItemAsync(item, cancellationToken);

    [HttpGet]
    public Task<IEnumerable<Shared.Item>> GetItemsAsync(CancellationToken cancellationToken)
        => itemRepository.GetItemsAsync(cancellationToken);

    [HttpGet("{itemId}")]
    public Task<Shared.Item> GetItemAsync(long itemId, CancellationToken cancellationToken)
        => itemRepository.GetItemAsync(itemId, cancellationToken);

    [HttpPut]
    public Task UpdateItemAsync([FromBody] Shared.Item item, CancellationToken cancellationToken)
        => itemRepository.UpdateItemAsync(item, cancellationToken);

    [HttpDelete("{itemId}")]
    public Task DeleteItemAsync(long itemId, CancellationToken cancellationToken)
        => itemRepository.DeleteItemAsync(itemId, cancellationToken);
}