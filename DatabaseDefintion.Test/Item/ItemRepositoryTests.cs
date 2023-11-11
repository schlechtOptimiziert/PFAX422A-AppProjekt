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
            var itemId = await itemRepository.AddItemAsync(new TransferModel.Item { Description = "Test", Name = "Test", Price = 3 }, CancellationToken).ConfigureAwait(false);
            var item = await itemRepository.GetItemAsync(itemId, CancellationToken).ConfigureAwait(false);
            Assert.NotNull(item);
        }
    }
}
