using DatabaseDefinition.EntityModel.Repositories.Interfaces;
using TransferModel;
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
        List<ItemPicture> testItemPictures = new();
        for (int i = 0; i < 3; i++)
            testItemPictures.Add(CreateRandomItemPictureForExistingItem(ids.First()));
        _ = await AddItemPicturesAsync(testItemPictures.ToArray()).ConfigureAwait(false);
        var itemPictures = await itemPictureRepository.GetItemPicturesAsync(ids.First(), CancellationToken).ConfigureAwait(false);
        Assert.NotNull(itemPictures);
        for (int i = 0; i < 3; i++)
        {
            Assert.NotNull(itemPictures.ElementAt(i));
            Assert.True(ItemPictureEqualsItemPicture(testItemPictures[i], itemPictures.ElementAt(i)));
        }
    }

    [Fact]
    public async Task GetItemPicturesByItemIdTest()
    {
        List<ItemPicture> testItemPictures = new();
        for (int i = 0; i < 3; i++)
            testItemPictures.Add(await CreateRandomItemPictureForNewItemAsync().ConfigureAwait(false));
        _ = await AddItemPicturesAsync(testItemPictures.ToArray()).ConfigureAwait(false);
        var itemPictures = await itemPictureRepository.GetItemPicturesAsync(testItemPictures[0].ItemId, CancellationToken).ConfigureAwait(false);
        Assert.NotNull(itemPictures);
        Assert.NotNull(itemPictures.First());
        Assert.True(ItemPictureEqualsItemPicture(testItemPictures[0], itemPictures.First()));
    }

    [Fact]
    public async Task GetMultipleItemPicturesByItemIdTest()
    {
        var testItem0 = CreateRandomItem();
        var ids = await AddItemsAsync(testItem0).ConfigureAwait(false);
        List<ItemPicture> testItemPictures = new();
        for (int i = 0; i < 3; i++)
            testItemPictures.Add(CreateRandomItemPictureForExistingItem(ids.First()));
        _ = await AddItemPicturesAsync(testItemPictures.ToArray()).ConfigureAwait(false);
        var itemPictures = await itemPictureRepository.GetItemPicturesAsync(ids.First(), CancellationToken).ConfigureAwait(false);
        Assert.NotNull(itemPictures);
        for (int i = 0; i < 3; i++)
        {
            Assert.NotNull(itemPictures.ElementAt(i));
            Assert.True(ItemPictureEqualsItemPicture(testItemPictures[i], itemPictures.ElementAt(i)));
        }
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
        List<ItemPicture> testItemPictures = new();
        for (int i = 0; i < 3; i++)
            testItemPictures.Add(await CreateRandomItemPictureForNewItemAsync().ConfigureAwait(false));
        var ids = await AddItemPicturesAsync(testItemPictures.ToArray()).ConfigureAwait(false);
        await itemPictureRepository.DeleteItemPictureAsync(ids.First(), CancellationToken).ConfigureAwait(false);
        var itemPictures = await itemPictureRepository.GetItemPicturesAsync(testItemPictures[0].ItemId, CancellationToken).ConfigureAwait(false);
        Assert.Empty(itemPictures);
    }

    [Fact]
    public async Task DeleteItemThrowsForUnknownTest()
    {
        List<ItemPicture> testItemPictures = new();
        for (int i = 0; i < 3; i++)
            testItemPictures.Add(await CreateRandomItemPictureForNewItemAsync().ConfigureAwait(false));
        _ = await AddItemPicturesAsync(testItemPictures.ToArray()).ConfigureAwait(false);
        await Assert.ThrowsAsync<ArgumentException>(async () =>
            await itemPictureRepository.DeleteItemPictureAsync(50, CancellationToken).ConfigureAwait(false)
            ).ConfigureAwait(false);
    }

}