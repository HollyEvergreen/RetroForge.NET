using NAudio.Wave;

namespace RetroForge.NET.Assets
{
    namespace Asset
    {
        public struct Audio : IAsset
        {
            public readonly AssetType AssetType => AssetType.Audio;
            public readonly string Name => ToString()!;
            private AudioFileReader reader;

            public static Audio CreateFromFile(string path) => new() { reader = new AudioFileReader(path) };

            public readonly void Dispose()
            {
                reader.Dispose();
                reader.Close();
            }
        }
    }
}