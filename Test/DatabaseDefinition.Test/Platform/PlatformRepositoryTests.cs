using DatabaseDefinition.EntityModel.Repositories.Interfaces;
using DatabaseDefintion.Test;
using Xunit;
using TM = TransferModel;

namespace DatabaseDefinition.Test.Platform;

public class PlatformRepositoryTests : DatabaseDefinitionTestBase
{
    private readonly IPlatformRepositroy platformRepositroy;

    public PlatformRepositoryTests()
    {
        platformRepositroy = PlatformRepository;
    }

    [Fact]
    public async Task AddPlatformTest()
    {
        var testPlatform = new TM.Platform()
        {
            Name = "Test1",
        };
        var platformId = await platformRepositroy.AddPlatformAsync(testPlatform, CancellationToken).ConfigureAwait(false);

        var platfrom = await platformRepositroy.GetPlatformAsync(platformId, CancellationToken).ConfigureAwait(false);
        Assert.NotNull(platfrom);
    }

    [Fact]
    public async Task AddPlatformsTest()
    {
        var testPlatform = new TM.Platform()
        {
            Name = "Test1",
        };
        await platformRepositroy.AddPlatformAsync(testPlatform, CancellationToken).ConfigureAwait(false);
        testPlatform = new TM.Platform()
        {
            Name = "Test2",
        };
        await platformRepositroy.AddPlatformAsync(testPlatform, CancellationToken).ConfigureAwait(false);

        var platfroms = await platformRepositroy.GetPlatformsAsync(CancellationToken).ConfigureAwait(false);
        Assert.NotEmpty(platfroms);
        Assert.Equal(2, platfroms.Count());
    }

    [Fact]
    public async Task UpdatePlatformTest()
    {
        var testPlatform = new TM.Platform()
        {
            Name = "Test1",
        };
        var platformId = await platformRepositroy.AddPlatformAsync(testPlatform, CancellationToken).ConfigureAwait(false);
        testPlatform = await platformRepositroy.GetPlatformAsync(platformId, CancellationToken).ConfigureAwait(false);

        testPlatform.Name = "Test2";
        await platformRepositroy.UpdatePlatformAsync(testPlatform, CancellationToken).ConfigureAwait(false);

        var platfrom = await platformRepositroy.GetPlatformAsync(platformId, CancellationToken).ConfigureAwait(false);
        Assert.Equal("Test2", platfrom.Name);
    }

    [Fact]
    public async Task DeletePlatformTest()
    {
        var testPlatform = new TM.Platform()
        {
            Name = "Test1",
        };
        var platformId = await platformRepositroy.AddPlatformAsync(testPlatform, CancellationToken).ConfigureAwait(false);
        await platformRepositroy.DeletePlatformAsync(platformId, CancellationToken).ConfigureAwait(false);

        await Assert.ThrowsAsync<ArgumentException>(async () =>
            await platformRepositroy.GetPlatformAsync(platformId, CancellationToken).ConfigureAwait(false)
        ).ConfigureAwait(false);
    }

    [Fact]
    public async Task DeleteNonExistingPlatformTest()
    {
        await Assert.ThrowsAsync<ArgumentException>(async () =>
             await platformRepositroy.DeletePlatformAsync(50, CancellationToken).ConfigureAwait(false)
        ).ConfigureAwait(false);
    }
}
