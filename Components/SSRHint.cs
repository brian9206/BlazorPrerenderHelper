using BlazorPrerenderHelper.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace BlazorPrerenderHelper.Components;

public class SSRHint : ComponentBase
{
    [Parameter]
    public required string Key { get; set; }
    
    [Parameter]
    public required object Value { get; set; }
    
    [Inject]
    private ISSRService? SSR { get; set; }
    
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        if (SSR?.IsServer != true)
            return;
        
        SSR.SetHint(Key, Value);
    }
}