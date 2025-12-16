namespace RetroForge.NET.Assets
{
    namespace Asset
    {
        public struct Anim2D : IAsset
        {
            public readonly AssetType AssetType => AssetType.Anim2D;
            public readonly string Name => ToString()!;
            private Image[] frames;
            private int frame_ptr;
            public readonly void Dispose() {
                foreach (var frame in frames)
                {
                    frame.Dispose();
                }
            }
            public static Anim2D CreateFromPath(string path)
            {
                Logger.Error($"Anim2D is not implemented");
                return new() { };
            }
        }
    }
}