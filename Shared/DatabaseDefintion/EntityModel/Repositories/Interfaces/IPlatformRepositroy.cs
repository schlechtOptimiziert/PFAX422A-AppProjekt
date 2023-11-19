using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TM = TransferModel;

namespace DatabaseDefintion.EntityModel.Repositories.Interfaces;

public interface IPlatformRepositroy
{
    Task<long> AddPlatformAsync(TM.Platform platform, CancellationToken cancellationToken);
    Task<IEnumerable<TM.Platform>> GetPlatformsAsync(CancellationToken cancellationToken);
    Task<TM.Platform> GetPlatformAsync(long platformId, CancellationToken cancellationToken);
    Task<IEnumerable<TM.Item>> GetPlatformFilteredItemsAsync(long platformId, CancellationToken cancellationToken);
    Task UpdatePlatformAsync(TM.Platform platform, CancellationToken cancellationToken);
    Task DeletePlatformAsync(long platformId, CancellationToken cancellationToken);
}
