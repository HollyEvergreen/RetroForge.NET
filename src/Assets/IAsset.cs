namespace RetroForge.NET.Assets
{
    namespace Asset
    {
        public interface IAsset : IDisposable
        {
            AssetType AssetType { get; }
            string Name { get; }

            public static virtual IAsset CreateFromSpan(Span<byte> data)
            {
                return null!;
            }

        }
    }
}