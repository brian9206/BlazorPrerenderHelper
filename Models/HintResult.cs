namespace BlazorPrerenderHelper.Models;

public record HintResult<T> where T : class
{
    public bool IsFound { get; set; }
    public T? Result { get; set; } = default;
}
