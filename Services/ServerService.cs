using System.Text.Json;
using System.Text.Json.Serialization;
using BlazorPrerenderHelper.Models;

namespace BlazorPrerenderHelper.Services;

internal class ServerService : ISSRService, IPrerenderScriptGenerator
{
    private readonly Dictionary<string, object> _hints = new();
    
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
        var json = JsonSerializer.Serialize(_hints, new JsonSerializerOptions()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        });

        return "<script>var ssrHint=" + json + "</script>";
    }
}