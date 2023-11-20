using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DatabaseDefinition.EntityModel.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using TM = TransferModel;

namespace MobileClient.Server.Controllers;

public class CartsController : ControllerBase
{
    private readonly ICartRepository cartRepository;

    public CartsController(ICartRepository cartRepository)
    {
        this.cartRepository = cartRepository;
    }

    [HttpPost("{userId}")]
    public Task AddItemToCartAsync(string userId, [FromBody] long itemId, CancellationToken cancellationToken)
        => cartRepository.AddItemToCartAsync(userId, itemId, cancellationToken);

    [HttpGet("{userId}")]
    public Task<IEnumerable<TM.CartItemLink>> GetCartItemLinksAsync(string userId, CancellationToken cancellationToken)
        => cartRepository.GetCartItemLinksAsync(userId, cancellationToken);

    [HttpPut("{userId}")]
    public Task UpdateItemAmountAsync(string userId, [FromBody] TM.CartItemLink cartItemLink, CancellationToken cancellationToken)
        => cartRepository.UpdateItemAmountAsync(userId, cartItemLink.ItemId, cartItemLink.Amount, cancellationToken);

    [HttpDelete("{userId}/{itemId}")]
    public Task DeleteItemFromCartAsync(string userId, long itemId, CancellationToken cancellationToken)
        => cartRepository.DeleteItemFromCartAsync(userId, itemId, cancellationToken);
}