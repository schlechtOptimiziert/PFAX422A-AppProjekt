using Microsoft.AspNetCore.Components;

namespace AppProject.SharedClientComponents.Components;

public partial class ConditionComponent : BaseComponent
{
    [Parameter]
    public RenderFragment Match { get; set; }

    [Parameter]
    public RenderFragment NotMatch { get; set; }

    [Parameter]
    public bool Condition { get; set; }
}
