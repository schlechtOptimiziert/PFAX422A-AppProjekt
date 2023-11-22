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
        await cartRepository.AddItemToCartAsync(user.Id, itemIds.First(), CancellationToken).ConfigureAwait(false);

        var itemLinks = await cartRepository.GetCartItemLinksAsync(user.Id, CancellationToken).ConfigureAwait(false);
        Assert.NotEmpty(itemLinks);
    }

    [Fact]
    public async Task AddItemToCartMultipleTimes_IncreasesAmountTest()
    {
        var user = await CreateUserAsync().ConfigureAwait(false);
        var testItem = CreateRandomItem();
        var itemIds = await AddItemsAsync(testItem).ConfigureAwait(false);
        await AddItemsToCartAsync(user.Id, itemIds.First(), itemIds.First());

        var itemLinks = await cartRepository.GetCartItemLinksAsync(user.Id, CancellationToken).ConfigureAwait(false);
        Assert.NotEmpty(itemLinks);
        Assert.NotNull(itemLinks.First());
        Assert.Equal(2, itemLinks.First().Amount);
    }

    [Fact]
    public async Task AddUnknownItemToCartTest()
    {
        var user = await CreateUserAsync().ConfigureAwait(false);
        await Assert.ThrowsAsync<ArgumentException>(async () =>
            await cartRepository.AddItemToCartAsync(user.Id, 50, CancellationToken).ConfigureAwait(false)
            ).ConfigureAwait(false);

        var itemLinks = await cartRepository.GetCartItemLinksAsync(user.Id, CancellationToken).ConfigureAwait(false);
        Assert.Empty(itemLinks);
    }

    [Fact]
    public async Task AddItemToCartOfUnknownUserTest()
    {
        var testItem = CreateRandomItem();
        var itemIds = await AddItemsAsync(testItem).ConfigureAwait(false);
        await Assert.ThrowsAsync<ArgumentException>(async () =>
            await cartRepository.AddItemToCartAsync(Guid.NewGuid().ToString(), itemIds.First(), CancellationToken).ConfigureAwait(false)
            ).ConfigureAwait(false);
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
            Assert.NotNull(itemLinks[i].Item);
            Assert.True(ItemEqualsItem(itemLinks[i].Item, testItems[i]));
        }
    }

    [Fact]
    public async Task GetCartItemLinksFromUnknownUserTest()
    {
        await Assert.ThrowsAsync<ArgumentException>(async () =>
            await cartRepository.GetCartItemLinksAsync(Guid.NewGuid().ToString(), CancellationToken).ConfigureAwait(false)
            ).ConfigureAwait(false);
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

        await cartRepository.DeleteItemFromCartAsync(user.Id, itemIds.First(), CancellationToken).ConfigureAwait(false);

        itemLinks = await cartRepository.GetCartItemLinksAsync(user.Id, CancellationToken).ConfigureAwait(false);
        Assert.Empty(itemLinks);
    }

    [Fact]
    public async Task RemoveUnknownItemFromCartTest()
    {
        var user = await CreateUserAsync().ConfigureAwait(false);
        var testItem = CreateRandomItem();
        var itemIds = await AddItemsAsync(testItem).ConfigureAwait(false);
        await AddItemsToCartAsync(user.Id, itemIds.First()).ConfigureAwait(false);

        await Assert.ThrowsAsync<ArgumentException>(async () =>
            await cartRepository.DeleteItemFromCartAsync(user.Id, 50, CancellationToken).ConfigureAwait(false)
            ).ConfigureAwait(false);

        var itemLinks = await cartRepository.GetCartItemLinksAsync(user.Id, CancellationToken).ConfigureAwait(false);
        Assert.NotEmpty(itemLinks);
    }

    [Fact]
    public async Task RemoveItemFromCartFromUnknownUserTest()
    {
        var user = await CreateUserAsync().ConfigureAwait(false);
        var testItem = CreateRandomItem();
        var itemIds = await AddItemsAsync(testItem).ConfigureAwait(false);
        await AddItemsToCartAsync(user.Id, itemIds.First()).ConfigureAwait(false);

        await Assert.ThrowsAsync<ArgumentException>(async () =>
            await cartRepository.DeleteItemFromCartAsync(Guid.NewGuid().ToString(), itemIds.First(), CancellationToken).ConfigureAwait(false)
            ).ConfigureAwait(false);

        var itemLinks = await cartRepository.GetCartItemLinksAsync(user.Id, CancellationToken).ConfigureAwait(false);
        Assert.NotEmpty(itemLinks);
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

        await cartRepository.UpdateItemAmountAsync(user.Id, itemIds.First(), 5, CancellationToken).ConfigureAwait(false);

        itemLinks = await cartRepository.GetCartItemLinksAsync(user.Id, CancellationToken).ConfigureAwait(false);
        Assert.NotEmpty(itemLinks);
        Assert.Equal(5, itemLinks.First().Amount);
    }

    [Fact]
    public async Task UpdateUnknownItemAmountTest()
    {
        var user = await CreateUserAsync().ConfigureAwait(false);
        var testItem = CreateRandomItem();
        var itemIds = await AddItemsAsync(testItem).ConfigureAwait(false);
        await AddItemsToCartAsync(user.Id, itemIds.First()).ConfigureAwait(false);

        var itemLinks = await cartRepository.GetCartItemLinksAsync(user.Id, CancellationToken).ConfigureAwait(false);
        Assert.NotEmpty(itemLinks);

        await Assert.ThrowsAsync<ArgumentException>(async () =>
            await cartRepository.UpdateItemAmountAsync(user.Id, 50, 5, CancellationToken).ConfigureAwait(false)
            ).ConfigureAwait(false);

        var newItemLinks = await cartRepository.GetCartItemLinksAsync(user.Id, CancellationToken).ConfigureAwait(false);
        Assert.NotEmpty(itemLinks);
        Assert.Equal(itemLinks.First().Amount, newItemLinks.First().Amount);
    }

    [Fact]
    public async Task UpdateItemAmountForUnknownUserTest()
    {
        var user = await CreateUserAsync().ConfigureAwait(false);
        var testItem = CreateRandomItem();
        var itemIds = await AddItemsAsync(testItem).ConfigureAwait(false);
        await AddItemsToCartAsync(user.Id, itemIds.First()).ConfigureAwait(false);

        var itemLinks = await cartRepository.GetCartItemLinksAsync(user.Id, CancellationToken).ConfigureAwait(false);
        Assert.NotEmpty(itemLinks);

        await Assert.ThrowsAsync<ArgumentException>(async () =>
            await cartRepository.UpdateItemAmountAsync(Guid.NewGuid().ToString(), itemIds.First(), 5, CancellationToken).ConfigureAwait(false)
            ).ConfigureAwait(false);

        var newItemLinks = await cartRepository.GetCartItemLinksAsync(user.Id, CancellationToken).ConfigureAwait(false);
        Assert.NotEmpty(itemLinks);
        Assert.Equal(itemLinks.First().Amount, newItemLinks.First().Amount);
    }
}
