using System.Text.Json;
using BlazorPrerenderHelper.Models;
using MessagePack;

namespace BlazorPrerenderHelper.Services;

internal class ServerService : ISSRService, IPrerenderScriptGenerator
{
    private readonly PrerenderHelperOptions _options;
    private readonly Dictionary<string, object> _hints = new();

    public ServerService(PrerenderHelperOptions options)
    {
        _options = options;
    }

    public bool IsServer => true;
    
    public Task<bool> IsSSR()
    {
        return Task.FromResult(true);
    }

    public Task<HintResult<T>> GetHint<T>(string key, bool preserve) where T : class
    {
        if (_hints.ContainsKey(key))
            return Task.FromResult(new HintResult<T>());

        var result = new HintResult<T>()
        {
            IsFound = true,
            Result = (T?)_hints[key]
        };

        if (!preserve)
            _hints.Remove(key);

        return Task.FromResult(result);
    }

    public Task<IReadOnlyDictionary<string, object>> GetAllHints()
    {
        return Task.FromResult((IReadOnlyDictionary<string, object>)_hints.AsReadOnly());
    }

    public void SetHint<T>(string key, T value) where T : class
    {
        _hints[key] = value;
    }

    public string Generate()
    {
        var data = _hints.ToDictionary(kv => kv.Key, kv => Convert.ToBase64String(MessagePackSerializer.Serialize(kv.Value, _options.MessagePackSerializerOptions)));
        var json = JsonSerializer.Serialize(data);
        return "<script>var ssrHint=" + json + "</script>";
    }
}