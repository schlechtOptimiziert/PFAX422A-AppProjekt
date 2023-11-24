using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DatabaseDefinition.EntityModel.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using TM = TransferModel;

namespace MobileClient.Server.Controllers;

public class PlatformController : ControllerBase
{
    private readonly IPlatformRepositroy platformRepositroy;
    private readonly IItemRepository itemRepository;

    public PlatformController(IPlatformRepositroy platformRepositroy, IItemRepository itemRepository)
    {
        this.platformRepositroy = platformRepositroy;
        this.itemRepository = itemRepository;
    }

    [HttpPost]
    public Task<long> AddPlatformAsync([FromBody] TM.Platform platform, CancellationToken cancellationToken)
        => platformRepositroy.AddPlatformAsync(platform, cancellationToken);

    [HttpGet]
    public Task<IEnumerable<TM.Platform>> GetPlatformsAsync(CancellationToken cancellationToken)
        => platformRepositroy.GetPlatformsAsync(cancellationToken);

    [HttpGet("{platformId}")]
    public Task<TM.Platform> GetPlatformAsync(long platformId, CancellationToken cancellationToken)
        => platformRepositroy.GetPlatformAsync(platformId, cancellationToken);

    [HttpGet("{platformId}/Items")]
    public Task<IEnumerable<TM.Item>> GetPlatformFilteredItemsAsync(long platformId, CancellationToken cancellationToken)
        => itemRepository.GetPlatformFilteredItemsAsync(platformId, cancellationToken);

    [HttpPut]
    public Task UpdatePlatformAsync([FromBody] TM.Platform platform, CancellationToken cancellationToken)
        => platformRepositroy.UpdatePlatformAsync(platform, cancellationToken);

    [HttpDelete("{platformId}")]
    public Task DeletePlatformAsync(long platformId, CancellationToken cancellationToken)
        => platformRepositroy.DeletePlatformAsync(platformId, cancellationToken);
}
