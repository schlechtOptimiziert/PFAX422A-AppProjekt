using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DatabaseDefinition.EntityModel.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using TM = TransferModel;

namespace MobileClient.Server.Controllers;

[Route("api/Users/{userId}/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderRepository orderRepository;

    public OrdersController(IOrderRepository orderRepository)
    {
        this.orderRepository = orderRepository;
    }

    [HttpPost]
    public Task<long> AddOrderAsync([FromBody] TM.Order order, CancellationToken cancellationToken)
        => orderRepository.AddOrderAsync(order, cancellationToken);

    [HttpGet]
    public Task<IEnumerable<TM.Order>> GetOrdersAsync(string userId, [FromQuery] bool includePositions, CancellationToken cancellationToken)
        => orderRepository.GetOrdersAsync(userId, includePositions, cancellationToken);

    [HttpGet("{orderId}")]
    public Task<TM.Order> GetOrderAsync(long orderId, CancellationToken cancellationToken)
        => orderRepository.GetOrderAsync(orderId, cancellationToken);
}