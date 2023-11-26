using DatabaseDefinition.EntityModel.Repositories.Interfaces;
using DatabaseDefintion.Test;
using Xunit;
using TM = TransferModel;

namespace DatabaseDefinition.Test.Cart;

public class OrderRepositoryTests : DatabaseDefinitionTestBase
{
    private readonly IOrderRepository orderRepository;
    private readonly IItemRepository itemRepository;

    public OrderRepositoryTests()
    {
        orderRepository = OrderRepository;
        itemRepository = ItemRepository;
    }

    [Fact]
    public async Task AddOrderTest()
    {
        var user = await CreateUserAsync().ConfigureAwait(false);
        List<TM.Item> testItems = new();
        for (int i = 0; i < 3; i++)
            testItems.Add(CreateRandomItem());
        _ = await AddItemsAsync(testItems.ToArray()).ConfigureAwait(false);
        var items = (await itemRepository.GetItemsAsync(CancellationToken).ConfigureAwait(false)).ToList();
        var testOrder = CreateRandomOrder(user.Id, items.ToArray());
        var orderId = await orderRepository.AddOrderAsync(testOrder, CancellationToken).ConfigureAwait(false);
        var order = await orderRepository.GetOrderAsync(orderId, CancellationToken).ConfigureAwait(false);
        Assert.NotNull(order);
        Assert.Equal(orderId, order.Id);
        Assert.Equal(testOrder.UserId, order.UserId);
        Assert.Equal(testOrder.Date, order.Date);
        var testOrderItems = testOrder.Items.ToList();
        var orderItems = order.Items.ToList();
        for (int i = 0; i < testOrderItems.Count; i++)
            Assert.True(ItemEqualsItem(testOrderItems[i], orderItems[i]));
    }

    [Fact]
    public async Task GetOrdersWithPositionsTest()
    {
        var user = await CreateUserAsync().ConfigureAwait(false);
        List<TM.Item> testItems = new();
        for (int i = 0; i < 3; i++)
            testItems.Add(CreateRandomItem());
        _ = await AddItemsAsync(testItems.ToArray()).ConfigureAwait(false);
        var items = (await itemRepository.GetItemsAsync(CancellationToken).ConfigureAwait(false)).ToList();
        List<TM.Order> testOrders = new();
        for (int i = 0; i < 3; i++)
            testOrders.Add(CreateRandomOrder(user.Id, items[i]));
        _ = await AddOrdersAsync(testOrders.ToArray()).ConfigureAwait(false);
        var orders = (await orderRepository.GetOrdersAsync(user.Id, true, CancellationToken).ConfigureAwait(false)).ToList();
        for (int i = 0; i < 3; i++)
        {
            Assert.NotNull(orders[i]);
            Assert.Equal(testOrders[i].UserId, orders[i].UserId);
            Assert.Equal(testOrders[i].Date, orders[i].Date);
            var testOrderItems = testOrders[i].Items.ToList();
            var orderItems = orders[i].Items.ToList();
            Assert.True(ItemEqualsItem(testOrderItems[0], orderItems[0]));
        }
    }

    [Fact]
    public async Task GetOrdersWithoutPositionsTest()
    {
        var user = await CreateUserAsync().ConfigureAwait(false);
        List<TM.Item> testItems = new();
        for (int i = 0; i < 3; i++)
            testItems.Add(CreateRandomItem());
        _ = await AddItemsAsync(testItems.ToArray()).ConfigureAwait(false);
        var items = (await itemRepository.GetItemsAsync(CancellationToken).ConfigureAwait(false)).ToList();
        List<TM.Order> testOrders = new();
        for (int i = 0; i < 3; i++)
            testOrders.Add(CreateRandomOrder(user.Id, items[i]));
        _ = await AddOrdersAsync(testOrders.ToArray()).ConfigureAwait(false);
        var orders = (await orderRepository.GetOrdersAsync(user.Id, false, CancellationToken).ConfigureAwait(false)).ToList();
        for (int i = 0; i < 3; i++)
        {
            Assert.NotNull(orders[i]);
            Assert.Equal(testOrders[i].UserId, orders[i].UserId);
            Assert.Equal(testOrders[i].Date, orders[i].Date);
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
        _ = await AddItemsAsync(testItems.ToArray()).ConfigureAwait(false);
        var items = (await itemRepository.GetItemsAsync(CancellationToken).ConfigureAwait(false)).ToList();
        var testOrder = CreateRandomOrder(user.Id, items.ToArray());
        var orderIds = await AddOrdersAsync(testOrder).ConfigureAwait(false);
        var order = await orderRepository.GetOrderAsync(orderIds.First(), CancellationToken).ConfigureAwait(false);
        Assert.NotNull(order);
        Assert.Equal(orderIds.First(), order.Id);
        Assert.Equal(testOrder.UserId, order.UserId);
        Assert.Equal(testOrder.Date, order.Date);
        var testOrderItems = testOrder.Items.ToList();
        var orderItems = order.Items.ToList();
        for (int i = 0; i < testOrderItems.Count; i++)
            Assert.True(ItemEqualsItem(testOrderItems[i], orderItems[i]));
    }
}
