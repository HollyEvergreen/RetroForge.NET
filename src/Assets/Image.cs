using StbImageSharp;

namespace RetroForge.NET.Assets
{
    namespace Asset
    {
        public struct Image : IAsset
        {
            public readonly AssetType AssetType => AssetType.Image;
            public readonly string Name => ToString()!;
            private ImageResult image;
            public static Image CreateFromFile(string path) => new() { image = ImageResult.FromStream(File.OpenRead(path)) };

            public readonly void Dispose() => Logger.Warn($"{this} does not need to be disposed");
        }
    }
}