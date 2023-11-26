using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TM = TransferModel;

namespace DatabaseDefinition.EntityModel.Repositories.Interfaces;

public interface IPlatformRepositroy
{
    Task<long> AddPlatformAsync(TM.Platform platform, CancellationToken cancellationToken);
    Task<IEnumerable<TM.Platform>> GetPlatformsAsync(CancellationToken cancellationToken);
    Task<TM.Platform> GetPlatformAsync(long platformId, CancellationToken cancellationToken);
    Task UpdatePlatformAsync(TM.Platform platform, CancellationToken cancellationToken);
    Task DeletePlatformAsync(long platformId, CancellationToken cancellationToken);
}
