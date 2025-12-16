using Scn = Aspose.ThreeD.Scene;
using AnimClip = Aspose.ThreeD.Animation.AnimationClip;

namespace RetroForge.NET.Assets
{
    namespace Asset
    {
        public struct Anim3D : IAsset
        {
            public readonly AssetType AssetType => AssetType.Anim3D;
            public readonly string Name => ToString()!;
            private List<AnimClip> _clip;
            public readonly void Dispose() { }
            public static Anim3D CreateFromScene(ref Scn scene) => new() { _clip = [.. scene.AnimationClips] };
            public static Anim3D CreateFromScene(Scn scene) => new() { _clip = [.. scene.AnimationClips] };
            public static Anim3D CreateFromFile(string path) => CreateFromScene(Scn.FromFile(path));
            public static Anim3D CreateFromFile(Stream File) => CreateFromScene(Scn.FromStream(File));
        }
    }
}