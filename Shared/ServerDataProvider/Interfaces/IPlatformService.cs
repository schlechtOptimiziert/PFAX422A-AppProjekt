using TM = TransferModel;

namespace ServerDataProvider.Interfaces;

public interface IPlatformService
{
    public Task<long> AddPlatformAsync(TM.Platform platform, CancellationToken cancellationToken);
    public Task<IEnumerable<TM.Platform>> GetPlatformsAsync(CancellationToken cancellationToken);
    public Task<TM.Platform> GetPlatformAsync(long platformId, CancellationToken cancellationToken);
    public Task<IEnumerable<TM.Item>> GetPlatformFilteredItemsAsync(long platformId, CancellationToken cancellationToken);
    public Task UpdatePlatformAsync(TM.Platform platform, CancellationToken cancellationToken);
    public Task DeletePlatformAsync(long platformId, CancellationToken cancellationToken);
}
