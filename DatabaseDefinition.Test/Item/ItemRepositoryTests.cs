using DatabaseDefinition.EntityModel.Repositories.Interfaces;
using Xunit;

namespace DatabaseDefintion.Test.Item
{
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
            var testItem0 = CreateRandomItem();
            var testItem1 = CreateRandomItem();
            var testItem2 = CreateRandomItem();
            _ = await AddItemsAsync(testItem0, testItem1, testItem2).ConfigureAwait(false);
            var items = await itemRepository.GetItemsAsync(CancellationToken).ConfigureAwait(false);
            Assert.NotNull(items);
            Assert.NotNull(items.ElementAt(0));
            Assert.True(ItemEqualsItem(testItem0, items.ElementAt(0)));
            Assert.NotNull(items.ElementAt(1));
            Assert.True(ItemEqualsItem(testItem1, items.ElementAt(1)));
            Assert.NotNull(items.ElementAt(2));
            Assert.True(ItemEqualsItem(testItem2, items.ElementAt(2)));
        }

        [Fact]
        public async Task GetItemTest()
        {
            var testItem0 = CreateRandomItem();
            var testItem1 = CreateRandomItem();
            var testItem2 = CreateRandomItem();
            var ids = await AddItemsAsync(testItem0, testItem1, testItem2).ConfigureAwait(false);
            var item = await itemRepository.GetItemAsync(ids.ElementAt(0), CancellationToken).ConfigureAwait(false);
            Assert.NotNull(item);
            Assert.Equal(ids.ElementAt(0), item.Id);
            Assert.True(ItemEqualsItem(testItem0, item));
        }

        [Fact]
        public async Task GetItemReturnsNullForUnknownTest()
        {

            var testItem0 = CreateRandomItem();
            var testItem1 = CreateRandomItem();
            var testItem2 = CreateRandomItem();
            _ = await AddItemsAsync(testItem0, testItem1, testItem2).ConfigureAwait(false);
            var item = await itemRepository.GetItemAsync(50, CancellationToken).ConfigureAwait(false);  //Should throw and not just return null
            Assert.Null(item);
        }

        [Fact]
        public async Task UpdateItemTest()
        {
            var testItem0 = CreateRandomItem();
            var testItem1 = CreateRandomItem();
            var testItem2 = CreateRandomItem();
            var ids = await AddItemsAsync(testItem0, testItem1, testItem2).ConfigureAwait(false);
            var updateItem = CreateRandomItem();
            updateItem.Id = ids.ElementAt(2);
            await itemRepository.UpdateItemAsync(updateItem, CancellationToken).ConfigureAwait(false);
            var item = await itemRepository.GetItemAsync(ids.ElementAt(2), CancellationToken).ConfigureAwait(false);
            Assert.NotNull(item);
            Assert.Equal(ids.ElementAt(2), item.Id);
            Assert.True(ItemEqualsItem(updateItem, item));
        }

        [Fact]
        public async Task UpdateItemThrowsForUnknownTest()
        {

            var testItem0 = CreateRandomItem();
            var testItem1 = CreateRandomItem();
            var testItem2 = CreateRandomItem();
            _ = await AddItemsAsync(testItem0, testItem1, testItem2).ConfigureAwait(false);
            var updateItem = CreateRandomItem();
            updateItem.Id = 50;
            await Assert.ThrowsAsync<ArgumentException>(async () =>
                await itemRepository.UpdateItemAsync(updateItem, CancellationToken).ConfigureAwait(false)
                ).ConfigureAwait(false);
        }

        [Fact]
        public async Task DeleteItemTest()
        {
            var testItem0 = CreateRandomItem();
            var testItem1 = CreateRandomItem();
            var testItem2 = CreateRandomItem();
            var ids = await AddItemsAsync(testItem0, testItem1, testItem2).ConfigureAwait(false);
            await itemRepository.DeleteItemAsync(ids.ElementAt(1), CancellationToken).ConfigureAwait(false);
            var item = await itemRepository.GetItemAsync(ids.ElementAt(1), CancellationToken).ConfigureAwait(false);  //Should throw and not just return null
            Assert.Null(item);
        }

        [Fact]
        public async Task DeleteItemThrowsForUnknownTest()
        {
            var testItem0 = CreateRandomItem();
            var testItem1 = CreateRandomItem();
            var testItem2 = CreateRandomItem();
            _ = await AddItemsAsync(testItem0, testItem1, testItem2).ConfigureAwait(false);
            await Assert.ThrowsAsync<ArgumentException>(async () =>
                await itemRepository.DeleteItemAsync(50, CancellationToken).ConfigureAwait(false)
                ).ConfigureAwait(false);
        }

    }
}
