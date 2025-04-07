using System.Collections.Frozen;
using Microsoft.Extensions.Logging;
using Talkie.Sequences;

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

        var assetsExtensions = FrozenSequence.Wrap(".png", ".jpg");

        try
        {
            _assets = assetsPaths
                .Where(path => assetsExtensions.Contains(Path.GetExtension(path)))
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
