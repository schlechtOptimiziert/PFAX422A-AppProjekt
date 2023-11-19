using DatabaseDefinition.EntityModel.Repositories.Interfaces;
using DatabaseDefintion.Test;
using Xunit;
using TM = TransferModel;

namespace DatabaseDefinition.Test.Cart;

public class CartRepositoryTests : DatabaseDefinitionTestBase
{
    private readonly ICartRepository cartRepository;

    public CartRepositoryTests()
    {
        cartRepository = CartRepository;
    }

    [Fact]
    public async Task AddItemToCartTest()
    {
        var user = await CreateUserAsync().ConfigureAwait(false);
        var testItem = CreateRandomItem();
        var itemIds = await AddItemsAsync(testItem).ConfigureAwait(false);
        await cartRepository.AddItemToCart(user.Id, itemIds.First(), CancellationToken).ConfigureAwait(false);

        var itemLinks = await cartRepository.GetCartItemLinksAsync(user.Id, CancellationToken).ConfigureAwait(false);
        Assert.NotEmpty(itemLinks);
    }

    [Fact]
    public async Task GetCartItemLinksTest()
    {
        var user = await CreateUserAsync().ConfigureAwait(false);
        List<TM.Item> testItems = new();
        for (int i = 0; i < 3; i++)
            testItems.Add(CreateRandomItem());
        var ids = (await AddItemsAsync(testItems.ToArray()).ConfigureAwait(false)).ToList();
        await AddItemsToCartAsync(user.Id, ids.ToArray()).ConfigureAwait(false);

        var itemLinks = (await cartRepository.GetCartItemLinksAsync(user.Id, CancellationToken).ConfigureAwait(false)).ToList();
        Assert.NotEmpty(itemLinks);
        for (int i = 0; i < 3; i++)
        {
            Assert.NotNull(itemLinks[i]);
            Assert.Equal(itemLinks[i].ItemId, ids[i]);
            Assert.True(ItemEqualsItem(itemLinks[i].Item, testItems[i]));
        }
    }

    [Fact]
    public async Task RemoveItemFromCartTest()
    {
        var user = await CreateUserAsync().ConfigureAwait(false);
        var testItem = CreateRandomItem();
        var itemIds = await AddItemsAsync(testItem).ConfigureAwait(false);
        await AddItemsToCartAsync(user.Id, itemIds.First()).ConfigureAwait(false);

        var itemLinks = await cartRepository.GetCartItemLinksAsync(user.Id, CancellationToken).ConfigureAwait(false);
        Assert.NotEmpty(itemLinks);

        await cartRepository.DeleteItemFromCart(user.Id, itemIds.First(), CancellationToken).ConfigureAwait(false);

        itemLinks = await cartRepository.GetCartItemLinksAsync(user.Id, CancellationToken).ConfigureAwait(false);
        Assert.Empty(itemLinks);
    }

    [Fact]
    public async Task UpdateItemAmountTest()
    {
        var user = await CreateUserAsync().ConfigureAwait(false);
        var testItem = CreateRandomItem();
        var itemIds = await AddItemsAsync(testItem).ConfigureAwait(false);
        await AddItemsToCartAsync(user.Id, itemIds.First()).ConfigureAwait(false);

        var itemLinks = await cartRepository.GetCartItemLinksAsync(user.Id, CancellationToken).ConfigureAwait(false);
        Assert.NotEmpty(itemLinks);

        await cartRepository.UpdateItemAmount(user.Id, itemIds.First(), 5, CancellationToken).ConfigureAwait(false);

        itemLinks = await cartRepository.GetCartItemLinksAsync(user.Id, CancellationToken).ConfigureAwait(false);
        Assert.NotEmpty(itemLinks);
        Assert.True(itemLinks.First().Amount == 5);
    }
}
