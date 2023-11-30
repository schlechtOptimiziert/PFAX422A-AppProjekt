using DatabaseDefinition.EntityModel;
using DatabaseDefinition.EntityModel.Repositories;
using DatabaseDefintion.EntityModel;
using DatabaseDefintion.EntityModel.Database;
using DatabaseDefintion.EntityModel.Repositories;
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
    protected CartRepository CartRepository { get; }
    protected PlatformRepository PlatformRepository { get; }
    protected OrderRepository OrderRepository { get; }
    protected CancellationToken CancellationToken { get; } = CancellationToken.None;
    protected Random Random { get; }
    protected AppProjectDbContext DbContext { get; }

    public DatabaseDefinitionTestBase()
    {
        DbContext = new AppProjectDbContext(CreateInMemoryDbContextOptions<AppProjectDbContext>(), new OperationalStoreOptionsMigrations());
        ItemRepository = new(DbContext);
        ItemPictureRepository = new(DbContext);
        CartRepository = new(DbContext);
        PlatformRepository = new(DbContext);
        OrderRepository = new(DbContext);
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
        return new() { ItemId = itemId, FileName = $"{Guid.NewGuid()}.png" };
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
            && itemPicture1.FileName == itemPicture2.FileName;
    }

    public async Task AddItemsToCartAsync(string userId, params long[] itemsIds)
    {
        foreach (var itemId in itemsIds)
            await CartRepository.AddItemToCartAsync(userId, itemId, CancellationToken).ConfigureAwait(false);
    }

    public async Task<ApplicationUser> CreateUserAsync()
    {
        DbContext.Users.Add(new ApplicationUser());
        await DbContext.SaveChangesAsync(CancellationToken).ConfigureAwait(false);
        return DbContext.Users.LastOrDefault() ??
            throw new Exception("Couldn't find created User");
    }

    public async Task<IEnumerable<long>> AddOrdersAsync(params TM.Order[] orders)
    {
        var ids = new List<long>();
        foreach (var order in orders)
            ids.Add(await OrderRepository.AddOrderAsync(order, CancellationToken).ConfigureAwait(false));
        return ids;
    }

    public TM.Order CreateRandomOrder(string userId)
    {
        return new()
        {
            Date = DateTime.Now,
            UserId = userId,
            Name = $"TestName-{Guid.NewGuid()}",
            Street = $"TestSteet-{Guid.NewGuid()}",
            HouseNumber = $"TestStreetNumber-{Guid.NewGuid()}",
            Postcode = Random.Next(),
            City = $"TestCity-{Guid.NewGuid()}",
            Country = $"TestCountry-{Guid.NewGuid()}",
        };
    }

    public static bool OrderEqualsOrder(TM.Order order1, TM.Order order2)
    {
        var itemsEqualsitems = true;
        if (order1.Items != null && order2.Items != null)
        {
            var order1Items = order1.Items.ToList();
            var order2Items = order2.Items.ToList();
            for (int i = 0; i < order1Items.Count; i++)
                if (!ItemEqualsItem(order1Items[i], order2Items[i]))
                    itemsEqualsitems = false;
        }
        return itemsEqualsitems
            && order1.Date == order2.Date
            && order1.UserId == order2.UserId
            && order1.Name == order2.Name
            && order1.Street == order2.Street
            && order1.HouseNumber == order2.HouseNumber
            && order1.Postcode == order2.Postcode
            && order1.City == order2.City
            && order1.Country == order2.Country;
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
