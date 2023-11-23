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
        var testOrder = new TM.Order
        {
            Date = DateTime.Now,
            UserId = user.Id
        };
        List<TM.Item> testItems = new();
        for (int i = 0; i < 3; i++)
            testItems.Add(CreateRandomItem());
        _ = await AddItemsAsync(testItems.ToArray()).ConfigureAwait(false);
        var items = (await itemRepository.GetItemsAsync(CancellationToken).ConfigureAwait(false)).ToList();
        testOrder.Items = items;
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
}
