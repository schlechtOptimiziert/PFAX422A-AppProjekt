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

    [HttpPost("{userId}/Items")]
    public Task AddItemToCartAsync(string userId, [FromBody] long itemId, CancellationToken cancellationToken)
        => cartRepository.AddItemToCartAsync(userId, itemId, cancellationToken);

    [HttpGet("{userId}/Items")]
    public Task<IEnumerable<TM.CartItemLink>> GetCartItemLinksAsync(string userId, CancellationToken cancellationToken)
        => cartRepository.GetCartItemLinksAsync(userId, cancellationToken);

    [HttpPut("{userId}/Items/{itemId}")]
    public Task UpdateItemAmountAsync(string userId, long itemId, [FromBody] TM.CartItemLink cartItemLink, CancellationToken cancellationToken)
        => cartRepository.UpdateItemAmountAsync(userId, itemId, cartItemLink.Amount, cancellationToken);

    [HttpDelete("{userId}/Items/{itemId}")]
    public Task DeleteItemFromCartAsync(string userId, long itemId, CancellationToken cancellationToken)
        => cartRepository.DeleteItemFromCartAsync(userId, itemId, cancellationToken);
}