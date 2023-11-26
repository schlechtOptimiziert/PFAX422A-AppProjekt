using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DatabaseDefinition.EntityModel.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using TM = TransferModel;

namespace AdminClient.Server.Controllers;

public class PlatformsController : ControllerBase
{
    private readonly IPlatformRepositroy platformRepositroy;

    public PlatformsController(IPlatformRepositroy platformRepositroy)
    {
        this.platformRepositroy = platformRepositroy;
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

    [HttpPut]
    public Task UpdatePlatformAsync([FromBody] TM.Platform platform, CancellationToken cancellationToken)
        => platformRepositroy.UpdatePlatformAsync(platform, cancellationToken);

    [HttpDelete("{platformId}")]
    public Task DeletePlatformAsync(long platformId, CancellationToken cancellationToken)
        => platformRepositroy.DeletePlatformAsync(platformId, cancellationToken);
}
