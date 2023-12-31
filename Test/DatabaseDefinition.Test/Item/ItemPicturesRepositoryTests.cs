﻿using DatabaseDefinition.EntityModel.Repositories.Interfaces;
using Xunit;
using TM = TransferModel;

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
        List<TM.ItemPicture> testItemPictures = new();
        for (int i = 0; i < 3; i++)
            testItemPictures.Add(CreateRandomItemPictureForExistingItem(ids.First()));
        _ = await AddItemPicturesAsync(testItemPictures.ToArray()).ConfigureAwait(false);
        var itemPictures = (await itemPictureRepository.GetItemPicturesAsync(testItemPictures[0].ItemId, CancellationToken).ConfigureAwait(false)).ToList();
        Assert.NotNull(itemPictures);
        for (int i = 0; i < 3; i++)
        {
            Assert.NotNull(itemPictures[i]);
            Assert.True(ItemPictureEqualsItemPicture(testItemPictures[i], itemPictures[i]));
        }
    }

    [Fact]
    public async Task GetItemPicturesByItemIdTest()
    {
        List<TM.ItemPicture> testItemPictures = new();
        for (int i = 0; i < 3; i++)
            testItemPictures.Add(await CreateRandomItemPictureForNewItemAsync().ConfigureAwait(false));
        _ = await AddItemPicturesAsync(testItemPictures.ToArray()).ConfigureAwait(false);
        var itemPictures = (await itemPictureRepository.GetItemPicturesAsync(testItemPictures[0].ItemId, CancellationToken).ConfigureAwait(false)).ToList();
        Assert.NotNull(itemPictures);
        Assert.NotNull(itemPictures[0]);
        Assert.True(ItemPictureEqualsItemPicture(testItemPictures[0], itemPictures[0]));
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
        List<TM.ItemPicture> testItemPictures = new();
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
        List<TM.ItemPicture> testItemPictures = new();
        for (int i = 0; i < 3; i++)
            testItemPictures.Add(await CreateRandomItemPictureForNewItemAsync().ConfigureAwait(false));
        _ = await AddItemPicturesAsync(testItemPictures.ToArray()).ConfigureAwait(false);
        await Assert.ThrowsAsync<ArgumentException>(async () =>
            await itemPictureRepository.DeleteItemPictureAsync(50, CancellationToken).ConfigureAwait(false)
            ).ConfigureAwait(false);
    }

}