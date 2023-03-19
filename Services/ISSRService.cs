using BlazorPrerenderHelper.Models;

namespace BlazorPrerenderHelper.Services;

public interface ISSRService
{
    /// <summary>
    /// Return true if the current render context is server-side
    /// </summary>
    bool IsServer { get; }
    
    /// <summary>
    /// Return true if the current app instance is prerendered
    /// </summary>
    /// <returns></returns>
    Task<bool> IsSSR();

    /// <summary>
    /// Get hint from prerender context
    /// </summary>
    /// <param name="key"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    Task<HintResult<T>> GetHint<T>(string key) where T : class;

    /// <summary>
    /// Get all hints
    /// </summary>
    /// <returns></returns>
    Task<IReadOnlyDictionary<string, object>> GetAllHints();

    /// <summary>
    /// Set hint to client render context (only works in prerender context)
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    void SetHint<T>(string key, T value) where T : class;
}