using DatabaseDefinition.EntityModel.Repositories.Interfaces;
using Xunit;
using TM = TransferModel;

namespace DatabaseDefintion.Test.Item;

public class ItemRepositoryTests : DatabaseDefinitionTestBase
{
    private readonly IItemRepository itemRepository;

    public ItemRepositoryTests()
    {
        itemRepository = ItemRepository;
    }

    [Fact]
    public async Task AddItemTest()
    {
        var testItem = CreateRandomItem();
        var itemId = await itemRepository.AddItemAsync(testItem, CancellationToken).ConfigureAwait(false);
        var item = await itemRepository.GetItemAsync(itemId, CancellationToken).ConfigureAwait(false);
        Assert.NotNull(item);
        Assert.True(ItemEqualsItem(testItem, item));
    }

    [Fact]
    public async Task GetItemsTest()
    {
        List<TM.Item> testItems = new();
        for (int i = 0; i < 3; i++)
            testItems.Add(CreateRandomItem());
        _ = await AddItemsAsync(testItems.ToArray()).ConfigureAwait(false);
        var items = (await itemRepository.GetItemsAsync(CancellationToken).ConfigureAwait(false)).ToList();
        Assert.NotNull(items);
        for (int i = 0; i < 3; i++)
        {
            Assert.NotNull(items[i]);
            Assert.True(ItemEqualsItem(testItems[i], items[i]));
        }
    }

    [Fact]
    public async Task GetItemTest()
    {
        List<TM.Item> testItems = new();
        for (int i = 0; i < 3; i++)
            testItems.Add(CreateRandomItem());
        var ids = (await AddItemsAsync(testItems.ToArray()).ConfigureAwait(false)).ToList();
        var item = await itemRepository.GetItemAsync(ids[0], CancellationToken).ConfigureAwait(false);
        Assert.NotNull(item);
        Assert.Equal(ids[0], item.Id);
        Assert.True(ItemEqualsItem(testItems[0], item));
    }

    [Fact]
    public async Task GetItemThrowsForUnknownTest()
    {
        List<TM.Item> testItems = new();
        for (int i = 0; i < 3; i++)
            testItems.Add(CreateRandomItem());
        _ = await AddItemsAsync(testItems.ToArray()).ConfigureAwait(false);
        await Assert.ThrowsAsync<ArgumentException>(async () =>
            await itemRepository.GetItemAsync(50, CancellationToken).ConfigureAwait(false)
            ).ConfigureAwait(false);
    }

    [Fact]
    public async Task UpdateItemTest()
    {
        List<TM.Item> testItems = new();
        for (int i = 0; i < 3; i++)
            testItems.Add(CreateRandomItem());
        var ids = (await AddItemsAsync(testItems.ToArray()).ConfigureAwait(false)).ToList();
        var updateItem = CreateRandomItem();
        updateItem.Id = ids[2];
        await itemRepository.UpdateItemAsync(updateItem, CancellationToken).ConfigureAwait(false);
        var item = await itemRepository.GetItemAsync(ids[2], CancellationToken).ConfigureAwait(false);
        Assert.NotNull(item);
        Assert.Equal(ids[2], item.Id);
        Assert.True(ItemEqualsItem(updateItem, item));
    }

    [Fact]
    public async Task UpdateItemThrowsForUnknownTest()
    {
        List<TM.Item> testItems = new();
        for (int i = 0; i < 3; i++)
            testItems.Add(CreateRandomItem());
        _ = await AddItemsAsync(testItems.ToArray()).ConfigureAwait(false);
        var updateItem = CreateRandomItem();
        updateItem.Id = 50;
        await Assert.ThrowsAsync<ArgumentException>(async () =>
            await itemRepository.UpdateItemAsync(updateItem, CancellationToken).ConfigureAwait(false)
            ).ConfigureAwait(false);
    }

    [Fact]
    public async Task DeleteItemTest()
    {
        List<TM.Item> testItems = new();
        for (int i = 0; i < 3; i++)
            testItems.Add(CreateRandomItem());
        var ids = (await AddItemsAsync(testItems.ToArray()).ConfigureAwait(false)).ToList();
        await itemRepository.DeleteItemAsync(ids[1], CancellationToken).ConfigureAwait(false);
        await Assert.ThrowsAsync<ArgumentException>(async () =>
            await itemRepository.GetItemAsync(ids[1], CancellationToken).ConfigureAwait(false)
            ).ConfigureAwait(false);
    }

    [Fact]
    public async Task DeleteItemThrowsForUnknownTest()
    {
        List<TM.Item> testItems = new();
        for (int i = 0; i < 3; i++)
            testItems.Add(CreateRandomItem());
        _ = await AddItemsAsync(testItems.ToArray()).ConfigureAwait(false);
        await Assert.ThrowsAsync<ArgumentException>(async () =>
            await itemRepository.DeleteItemAsync(50, CancellationToken).ConfigureAwait(false)
            ).ConfigureAwait(false);
    }
}
