using DatabaseDefinition.EntityModel;
using DatabaseDefinition.EntityModel.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Storage;
using TM = TransferModel;

namespace DatabaseDefintion.Test;

/// <summary>
/// This class provides repositories with an in-memory db.
/// The in-memory db saves an update partially, if the commit failed.
/// The in-memory db also does not set db-generated values.
/// The in-memory db has other behaviors differing from a real db.
/// For these reasons, test cases may require a real local db for testing.
/// The huge benefit of the in-memory db is that is provided for testing very quick.
/// The local db enables true db logic testing, but comes with a high performance cost.
/// If we want to use those in the future we need to add a way to choose whether a test should use a in-memory db or a test sql db.
/// </summary>
public class DatabaseDefinitionTestBase
{
    protected ItemRepository ItemRepository { get; }
    protected ItemPictureRepository ItemPictureRepository { get; }
    protected CancellationToken CancellationToken { get; } = CancellationToken.None;
    protected Random Random { get; }

    public DatabaseDefinitionTestBase()
    {
        var dbContext = new AppProjectDbContext(CreateInMemoryDbContextOptions<AppProjectDbContext>());
        ItemRepository = new(dbContext);
        ItemPictureRepository = new(dbContext);
        Random = new Random();
    }

    public TM.Item CreateRandomItem()
    {
        return new() { Description = $"TestDescription-{Guid.NewGuid()}", Name = $"TestName-{Guid.NewGuid()}", Price = (decimal)(Random.Next(0, 1000000) + Random.NextDouble()) };
    }

    public async Task<IEnumerable<long>> AddItemsAsync(params TM.Item[] items)
    {
        var ids = new List<long>();
        foreach (var item in items)
            ids.Add(await ItemRepository.AddItemAsync(item, CancellationToken).ConfigureAwait(false));
        return ids;
    }

    public static bool ItemEqualsItem(TM.Item item1, TM.Item item2)
    {
        return item1.Name == item2.Name
            && item1.Description == item2.Description
            && item1.Price == item2.Price;
    }

    public async Task<TM.ItemPicture> CreateRandomItemPictureForNewItemAsync()
    {
        var item = CreateRandomItem();
        var ids = await AddItemsAsync(item).ConfigureAwait(false);
        return CreateRandomItemPictureForExistingItem(ids.First());
    }

    public TM.ItemPicture CreateRandomItemPictureForExistingItem(long itemId)
    {
        return new() { ItemId = itemId, Bytes = new byte[500], Description = $"TestDescription-{Guid.NewGuid()}", FileExtension = "png", Size = (decimal)(Random.Next(0, 1000000) + Random.NextDouble()) };
    }

    public async Task<IEnumerable<long>> AddItemPicturesAsync(params TM.ItemPicture[] itemsPictures)
    {
        var ids = new List<long>();
        foreach (var itemPicture in itemsPictures)
            ids.Add(await ItemPictureRepository.AddItemPictureAsync(itemPicture, CancellationToken).ConfigureAwait(false));
        return ids;
    }

    public static bool ItemPictureEqualsItemPicture(TM.ItemPicture itemPicture1, TM.ItemPicture itemPicture2)
    {
        return itemPicture1.ItemId == itemPicture2.ItemId
            && itemPicture1.Bytes.SequenceEqual(itemPicture2.Bytes)
            && itemPicture1.Description == itemPicture2.Description
            && itemPicture1.FileExtension == itemPicture2.FileExtension
            && itemPicture1.Size == itemPicture2.Size;
    }

    /// <summary>
    /// Creates <see cref="DbContextOptions"/> for the usage of an in-memory db.
    /// No manual cleanup required.
    /// </summary>
    private static DbContextOptions<TDbContext> CreateInMemoryDbContextOptions<TDbContext>()
        where TDbContext : DbContext
    {
        var dbName = CreateUniqueTestDbName();
        return new DbContextOptionsBuilder<TDbContext>()
            .ConfigureWarnings(x => x.Ignore(CoreEventId.ManyServiceProvidersCreatedWarning))
            .UseInMemoryDatabase(dbName, new InMemoryDatabaseRoot()).Options;
    }

    private static string CreateUniqueTestDbName() => $"TestDb_{Guid.NewGuid()}";
}
