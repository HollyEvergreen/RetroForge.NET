namespace RetroForge.NET.Assets
{
    namespace Asset
    {
        public partial struct Config : IAsset
        {
            public readonly AssetType AssetType => AssetType.Config;
            public readonly string Name => "Config Asset";

            public static Config CreateFromFile(string path)
            {
                throw new NotImplementedException();
            }

            public readonly void Dispose(){}
        }
    }
}