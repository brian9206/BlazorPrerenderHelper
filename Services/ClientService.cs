using BlazorPrerenderHelper.Models;
using MessagePack;
using Microsoft.JSInterop;

namespace BlazorPrerenderHelper.Services;

internal class ClientService : ISSRService
{
    private readonly IJSRuntime _jsRuntime;
    private readonly PrerenderHelperOptions _options;
    
    public ClientService(IJSRuntime jsRuntime, PrerenderHelperOptions options)
    {
        _jsRuntime = jsRuntime;
        _options = options;
    }

    public bool IsServer => false;

    public async Task<bool> IsSSR()
    {
        return await _jsRuntime.InvokeAsync<int>("ssrInterop.isSSR") == 1;
    }

    public async Task<HintResult<T>> GetHint<T>(string key, bool preserve) where T : class
    {
        var result = await _jsRuntime.InvokeAsync<MessagePackHintResult>("ssrInterop.get", key, preserve);

        if (result.IsFound)
        {
            return new HintResult<T>()
            {
                IsFound = true,
                Result = MessagePackSerializer.Deserialize<T>(Convert.FromBase64String(result.Result), _options.MessagePackSerializerOptions)
            };
        }

        return new HintResult<T>()
        {
            IsFound = false
        };
    }

    public async Task<IReadOnlyDictionary<string, object>> GetAllHints()
    {
        var result = await _jsRuntime.InvokeAsync<IReadOnlyDictionary<string, string>>("ssrInterop.getHints");
        return result.ToDictionary(kv => kv.Key, kv => MessagePackSerializer.Deserialize<object>(Convert.FromBase64String(kv.Value), _options.MessagePackSerializerOptions));
    }

    public void SetHint<T>(string key, T value) where T : class
    {
        throw new InvalidOperationException("Client context does not support this operation");
    }
}