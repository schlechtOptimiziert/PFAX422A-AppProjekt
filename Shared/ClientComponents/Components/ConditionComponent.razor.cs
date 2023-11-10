using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientComponents.Components;

public partial class ConditionComponent : BaseComponent
{
    [Parameter]
    public RenderFragment Match { get; set; }

    [Parameter]
    public RenderFragment NotMatch { get; set; }

    [Parameter]
    public bool Condition { get; set; }
}
