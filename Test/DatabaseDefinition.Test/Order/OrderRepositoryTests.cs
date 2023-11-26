using DatabaseDefinition.EntityModel.Repositories.Interfaces;
using DatabaseDefintion.Test;
using Xunit;
using TM = TransferModel;

namespace DatabaseDefinition.Test.Cart;

public class OrderRepositoryTests : DatabaseDefinitionTestBase
{
    private readonly IOrderRepository orderRepository;
    private readonly ICartRepository cartRepository;

    public OrderRepositoryTests()
    {
        orderRepository = OrderRepository;
        cartRepository = CartRepository;
    }

    [Fact]
    public async Task AddOrderTest()
    {
        var user = await CreateUserAsync().ConfigureAwait(false);
        List<TM.Item> testItems = new();
        for (int i = 0; i < 3; i++)
            testItems.Add(CreateRandomItem());
        var itemIds = await AddItemsAsync(testItems.ToArray()).ConfigureAwait(false);
        await AddItemsToCartAsync(user.Id, itemIds.ToArray());
        var testOrder = CreateRandomOrder(user.Id);
        var orderId = await orderRepository.AddOrderAsync(testOrder, CancellationToken).ConfigureAwait(false);
        var order = await orderRepository.GetOrderAsync(orderId, CancellationToken).ConfigureAwait(false);
        testOrder.Items = testItems;
        Assert.NotNull(order);
        Assert.Equal(orderId, order.Id);
        Assert.True(OrderEqualsOrder(order, testOrder));

        var cartItems = await cartRepository.GetCartItemLinksAsync(user.Id, CancellationToken).ConfigureAwait(false);
        Assert.Empty(cartItems);
    }

    [Fact]
    public async Task GetOrdersWithPositionsTest()
    {
        var user = await CreateUserAsync().ConfigureAwait(false);
        List<TM.Item> testItems = new();
        for (int i = 0; i < 3; i++)
            testItems.Add(CreateRandomItem());
        var itemIds = (await AddItemsAsync(testItems.ToArray()).ConfigureAwait(false)).ToList();
        List<TM.Order> testOrders = new();
        for (int i = 0; i < 3; i++)
        {
            await AddItemsToCartAsync(user.Id, itemIds[i]);
            testOrders.Add(CreateRandomOrder(user.Id));
            _ = await AddOrdersAsync(testOrders[i]).ConfigureAwait(false);
        }
        var orders = (await orderRepository.GetOrdersAsync(user.Id, true, CancellationToken).ConfigureAwait(false)).ToList();
        for (int i = 0; i < 3; i++)
        {
            testOrders[i].Items = new List<TM.Item>() { testItems[i] };
            Assert.NotNull(orders[i]);
            Assert.True(OrderEqualsOrder(orders[i], testOrders[i]));
        }
    }

    [Fact]
    public async Task GetOrdersWithoutPositionsTest()
    {
        var user = await CreateUserAsync().ConfigureAwait(false);
        List<TM.Item> testItems = new();
        for (int i = 0; i < 3; i++)
            testItems.Add(CreateRandomItem());
        var itemIds = await AddItemsAsync(testItems.ToArray()).ConfigureAwait(false);
        await AddItemsToCartAsync(user.Id, itemIds.ToArray());
        List<TM.Order> testOrders = new();
        for (int i = 0; i < 3; i++)
            testOrders.Add(CreateRandomOrder(user.Id));
        _ = await AddOrdersAsync(testOrders.ToArray()).ConfigureAwait(false);
        var orders = (await orderRepository.GetOrdersAsync(user.Id, false, CancellationToken).ConfigureAwait(false)).ToList();
        for (int i = 0; i < 3; i++)
        {
            Assert.NotNull(orders[i]);
            Assert.True(OrderEqualsOrder(orders[i], testOrders[i]));
            Assert.Null(orders[i].Items);
        }
    }

    [Fact]
    public async Task GetOrderTest()
    {
        var user = await CreateUserAsync().ConfigureAwait(false);
        List<TM.Item> testItems = new();
        for (int i = 0; i < 3; i++)
            testItems.Add(CreateRandomItem());
        var itemIds = await AddItemsAsync(testItems.ToArray()).ConfigureAwait(false);
        await AddItemsToCartAsync(user.Id, itemIds.ToArray());
        var testOrder = CreateRandomOrder(user.Id);
        var orderIds = await AddOrdersAsync(testOrder).ConfigureAwait(false);
        var order = await orderRepository.GetOrderAsync(orderIds.First(), CancellationToken).ConfigureAwait(false);
        testOrder.Items = testItems;
        Assert.NotNull(order);
        Assert.Equal(orderIds.First(), order.Id);
        Assert.True(OrderEqualsOrder(order, testOrder));
    }
}
