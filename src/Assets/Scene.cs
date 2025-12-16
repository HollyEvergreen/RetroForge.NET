using Scn = Aspose.ThreeD.Scene;

namespace RetroForge.NET.Assets
{
    namespace Asset
    {
        public struct Scene : IAsset
        {
            public readonly AssetType AssetType => AssetType.Scene;
            public readonly string Name => ToString()!;
            Scn _scene;
            public readonly void Dispose() { }
            public static Scene CreateFromFile(string path) => new() { _scene = Scn.FromFile(path) };
        }
    }
}