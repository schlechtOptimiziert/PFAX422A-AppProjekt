using DatabaseDefinition.EntityModel.Repositories.Interfaces;
using Xunit;

namespace DatabaseDefintion.Test.Item;

public class ItemPicturesRepositoryTests : DatabaseDefinitionTestBase
{
    private readonly IItemPictureRepository itemPictureRepository;

    public ItemPicturesRepositoryTests()
    {
        itemPictureRepository = ItemPictureRepository;
    }

    [Fact]
    public async Task AddItemPictureTest()
    {
        var testItemPicture = await CreateRandomItemPictureForNewItemAsync().ConfigureAwait(false);
        _ = await itemPictureRepository.AddItemPictureAsync(testItemPicture, CancellationToken).ConfigureAwait(false);
        var itemPictures = await itemPictureRepository.GetItemPicturesAsync(testItemPicture.ItemId, CancellationToken).ConfigureAwait(false);
        Assert.NotNull(itemPictures);
        Assert.NotNull(itemPictures.First());
        Assert.True(ItemPictureEqualsItemPicture(testItemPicture, itemPictures.First()));
    }

    [Fact]
    public async Task AddMultipleItemPicturesTest()
    {
        var testItem0 = CreateRandomItem();
        var ids = await AddItemsAsync(testItem0).ConfigureAwait(false);
        var testItemPicture0 = CreateRandomItemPictureForExistingItem(ids.First());
        var testItemPicture1 = CreateRandomItemPictureForExistingItem(ids.First());
        var testItemPicture2 = CreateRandomItemPictureForExistingItem(ids.First());
        _ = await itemPictureRepository.AddItemPictureAsync(testItemPicture0, CancellationToken).ConfigureAwait(false);
        _ = await itemPictureRepository.AddItemPictureAsync(testItemPicture1, CancellationToken).ConfigureAwait(false);
        _ = await itemPictureRepository.AddItemPictureAsync(testItemPicture2, CancellationToken).ConfigureAwait(false);
        var itemPictures = await itemPictureRepository.GetItemPicturesAsync(ids.First(), CancellationToken).ConfigureAwait(false);
        Assert.NotNull(itemPictures);
        Assert.NotNull(itemPictures.ElementAt(0));
        Assert.True(ItemPictureEqualsItemPicture(testItemPicture0, itemPictures.ElementAt(0)));
        Assert.NotNull(itemPictures.ElementAt(1));
        Assert.True(ItemPictureEqualsItemPicture(testItemPicture1, itemPictures.ElementAt(1)));
        Assert.NotNull(itemPictures.ElementAt(2));
        Assert.True(ItemPictureEqualsItemPicture(testItemPicture2, itemPictures.ElementAt(2)));
    }

    [Fact]
    public async Task GetItemPicturesByItemIdTest()
    {
        var testItemPicture0 = await CreateRandomItemPictureForNewItemAsync().ConfigureAwait(false);
        var testItemPicture1 = await CreateRandomItemPictureForNewItemAsync().ConfigureAwait(false);
        var testItemPicture2 = await CreateRandomItemPictureForNewItemAsync().ConfigureAwait(false);
        _ = await AddItemPicturesAsync(testItemPicture0, testItemPicture1, testItemPicture2).ConfigureAwait(false);
        var itemPictures = await itemPictureRepository.GetItemPicturesAsync(testItemPicture0.ItemId, CancellationToken).ConfigureAwait(false);
        Assert.NotNull(itemPictures);
        Assert.NotNull(itemPictures.First());
        Assert.True(ItemPictureEqualsItemPicture(testItemPicture0, itemPictures.First()));
    }

    [Fact]
    public async Task GetMultipleItemPicturesByItemIdTest()
    {
        var testItem0 = CreateRandomItem();
        var ids = await AddItemsAsync(testItem0).ConfigureAwait(false);
        var testItemPicture0 = CreateRandomItemPictureForExistingItem(ids.First());
        var testItemPicture1 = CreateRandomItemPictureForExistingItem(ids.First());
        var testItemPicture2 = CreateRandomItemPictureForExistingItem(ids.First());
        _ = await AddItemPicturesAsync(testItemPicture0, testItemPicture1, testItemPicture2).ConfigureAwait(false);
        var itemPictures = await itemPictureRepository.GetItemPicturesAsync(ids.First(), CancellationToken).ConfigureAwait(false);
        Assert.NotNull(itemPictures);
        Assert.NotNull(itemPictures.ElementAt(0));
        Assert.True(ItemPictureEqualsItemPicture(testItemPicture0, itemPictures.ElementAt(0)));
        Assert.NotNull(itemPictures.ElementAt(1));
        Assert.True(ItemPictureEqualsItemPicture(testItemPicture1, itemPictures.ElementAt(1)));
        Assert.NotNull(itemPictures.ElementAt(2));
        Assert.True(ItemPictureEqualsItemPicture(testItemPicture2, itemPictures.ElementAt(2)));
    }

    [Fact]
    public async Task GetItemPictureThrowsForUnknownItemTest()
    {
        var testItemPicture = await CreateRandomItemPictureForNewItemAsync().ConfigureAwait(false);
        _ = await AddItemPicturesAsync(testItemPicture).ConfigureAwait(false);
        await Assert.ThrowsAsync<ArgumentException>(async () =>
            await itemPictureRepository.GetItemPicturesAsync(50, CancellationToken).ConfigureAwait(false)
            ).ConfigureAwait(false);
    }

    [Fact]
    public async Task DeleteItemPictureTest()
    {
        var testItemPicture0 = await CreateRandomItemPictureForNewItemAsync().ConfigureAwait(false);
        var testItemPicture1 = await CreateRandomItemPictureForNewItemAsync().ConfigureAwait(false);
        var testItemPicture2 = await CreateRandomItemPictureForNewItemAsync().ConfigureAwait(false);
        var ids = await AddItemPicturesAsync(testItemPicture0, testItemPicture1, testItemPicture2).ConfigureAwait(false);
        await itemPictureRepository.DeleteItemPictureAsync(ids.First(), CancellationToken).ConfigureAwait(false);
        var itemPictures = await itemPictureRepository.GetItemPicturesAsync(testItemPicture0.ItemId, CancellationToken).ConfigureAwait(false);
        Assert.Empty(itemPictures);
    }

    [Fact]
    public async Task DeleteItemThrowsForUnknownTest()
    {
        var testItemPicture0 = await CreateRandomItemPictureForNewItemAsync().ConfigureAwait(false);
        var testItemPicture1 = await CreateRandomItemPictureForNewItemAsync().ConfigureAwait(false);
        var testItemPicture2 = await CreateRandomItemPictureForNewItemAsync().ConfigureAwait(false);
        _ = await AddItemPicturesAsync(testItemPicture0, testItemPicture1, testItemPicture2).ConfigureAwait(false);
        await Assert.ThrowsAsync<ArgumentException>(async () =>
            await itemPictureRepository.DeleteItemPictureAsync(50, CancellationToken).ConfigureAwait(false)
            ).ConfigureAwait(false);
    }

}