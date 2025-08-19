using System.Collections.Frozen;
using Microsoft.Extensions.Logging;
using Falko.Talkie.Sequences;

namespace Fembina.Busquita.Storages.Assets;

public sealed class AssetProvider : IAssetProvider
{
    private readonly FrozenDictionary<string, string> _assets = FrozenDictionary<string, string>.Empty;

    public AssetProvider(ILogger<AssetProvider> logger)
    {
        var assetsDirectory = Path.Combine(AppContext.BaseDirectory, "Assets");

        IEnumerable<string> assetsPaths;

        try
        {
            assetsPaths = Directory.EnumerateFiles(assetsDirectory);
        }
        catch (DirectoryNotFoundException exception)
        {
            logger.LogWarning(exception, "Assets directory not found");
            return;
        }

        try
        {
            _assets = assetsPaths
                .Where(path => new ReadOnlySpan<string>([".png", ".jpg"]).Contains(Path.GetExtension(path)))
                .Select(path => new KeyValuePair<string, string>(Path.GetFileNameWithoutExtension(path).ToLowerInvariant(), path))
                .ToFrozenDictionary();
        }
        catch (IOException exception)
        {
            logger.LogWarning(exception, "Error while reading assets");
        }
    }

    public string GetAsset(string name) => _assets[name.ToLowerInvariant()];
}
