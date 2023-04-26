using MessagePack;
using MessagePack.Resolvers;

namespace BlazorPrerenderHelper.Models;

public class PrerenderHelperOptions
{
    public MessagePackSerializerOptions MessagePackSerializerOptions { get; set; } = ContractlessStandardResolver.Options.WithCompression(MessagePackCompression.Lz4BlockArray);
}