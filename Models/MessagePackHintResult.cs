namespace BlazorPrerenderHelper.Models;

public record MessagePackHintResult
{
    public bool IsFound { get; set; }
    public string Result { get; set; } = string.Empty;
}