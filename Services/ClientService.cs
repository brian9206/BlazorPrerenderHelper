using System.Text.Json;
using BlazorPrerenderHelper.Models;
using Microsoft.JSInterop;

namespace BlazorPrerenderHelper.Services;

internal class ClientService : ISSRService
{
    private readonly IJSRuntime _jsRuntime;
    
    public ClientService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public bool IsServer => false;

    public async Task<bool> IsSSR()
    {
        return await _jsRuntime.InvokeAsync<int>("ssrInterop.isSSR") == 1;
    }

    public async Task<HintResult<T>> GetHint<T>(string key) where T : class
    {
        return await _jsRuntime.InvokeAsync<HintResult<T>>("ssrInterop.getHint", key);
    }

    public async Task<IReadOnlyDictionary<string, object>> GetAllHints()
    {
        return await _jsRuntime.InvokeAsync<IReadOnlyDictionary<string, object>>("ssrInterop.getHints");
    }

    public void SetHint<T>(string key, T value) where T : class
    {
        throw new InvalidOperationException("Client context does not support this operation");
    }
}