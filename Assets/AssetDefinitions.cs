using Img = SixLabors.ImageSharp.Image;
using Scn = Aspose.ThreeD.Scene;
using AnimClip = Aspose.ThreeD.Animation.AnimationClip;
using SixLabors.ImageSharp;
using NAudio.Wave;
using shaderc;
using System.Linq;

namespace RetroForge.NET.Assets
{
    public enum AssetType
    {
        Audio,
        Image,
        Text,
        Shader,
        Scene,
        Anim2D,
        Anim3D,
        RAW,
    }
    namespace Asset
    {
        public interface IAsset
        {
            AssetType AssetType { get; }
            string Name { get; }

            public static virtual IAsset CreateFromSpan(Span<byte> data)
            {
                return null!;
            }
        }
        public struct Audio : IAsset
        {
            public readonly AssetType AssetType => AssetType.Audio;
            public readonly string Name => ToString()!;
            private AudioFileReader reader;

            public static Audio CreateFromFile(string path) => new() { reader = new AudioFileReader(path) };
        }

        public struct Image : IAsset
        {
            public readonly AssetType AssetType => AssetType.Image;
            public readonly string Name => ToString()!;
            private Img image;
            public static Image CreateFromFile(string path) => new() { image = Img.Load(path) };
        }

        public struct Text : IAsset
        {
            public readonly AssetType AssetType => AssetType.Text;
            public readonly string Name => ToString()!;
            public string _Text { get; private set; }
            public static Text CreateFromZippedFile(Stream file)
            {
                Span<byte> bytes = stackalloc byte[(int)file.Length];
                file.ReadExactly(bytes);
                string content = string.Join(' ', bytes);
                return new() { _Text = content };
            }
        }
        public struct Shader : IAsset
        {
            public readonly AssetType AssetType => AssetType.Shader;
            public readonly string Name => ToString()!;
            public int ID { get; private set; }
            private ReadOnlyMemory<byte> bytecode;
            

            public static Shader CreateFromSpan(Span<byte> data) => new() { bytecode = data.ToArray() };

            public static Shader CreateFromFile(string path) => CompileShader(path) ?? new Shader();

            private static Shader? CompileShader(string path)
            {
                var ShaderReadTask = File.ReadAllTextAsync(path);
                SourceLanguage lang = (SourceLanguage)2;
                string[] HLSL_EXTENSIONS = ["hlsl"];
                string[] GLSL_EXTENSIONS = ["frag", "vert"];
                if (HLSL_EXTENSIONS.Contains(Path.GetExtension(path))) lang = SourceLanguage.Hlsl;
                else if (GLSL_EXTENSIONS.Contains(Path.GetExtension(path))) lang = SourceLanguage.Glsl;
                if (HLSL_EXTENSIONS.Contains(Path.GetExtension(path)) 
                    && 
                    GLSL_EXTENSIONS.Contains(Path.GetExtension(path))
                   ) throw new Exception("could not determine source language");

                using Compiler compiler = new(new()
                {
                    SourceLanguage = lang,
                    Optimization = OptimizationLevel.Performance,
                });

                ShaderKind kind = path switch
                {
                    "vert" => ShaderKind.VertexShader,
                    "frag" => ShaderKind.FragmentShader,
                    "cl" => ShaderKind.ComputeShader,
                    "geo" => ShaderKind.GeometryShader,
                    "tessc" => ShaderKind.TessControlShader,
                    "tesse" => ShaderKind.TessEvaluationShader,
                    "spv" => ShaderKind.SpirvAssembly,
                    "raygen" => ShaderKind.RaygenShader,
                    "anyhit" => ShaderKind.AnyhitShader,
                    "closethit" => ShaderKind.ClosesthitShader,
                    "miss" => ShaderKind.MissShader,
                    "intersection" => ShaderKind.IntersectionShader,
                    "callable" => ShaderKind.CallableShader,
                    "task" => ShaderKind.TaskShader,
                    "mesh" => ShaderKind.MeshShader,
                    _ => ShaderKind.GlslInferFromSource
                };

                var res = compiler.Compile(path, kind, "main");
                    if (res.ErrorCount > 0)
                        throw new(res.ErrorMessage);
                    else 
                        unsafe { return CreateFromSpan(new Span<byte>((void*)res.CodePointer, (int)res.CodeLength));}
            }
        }
        public struct Scene : IAsset
        {
            public readonly AssetType AssetType => AssetType.Scene;
            public readonly string Name => ToString()!;
            Scn _scene;
            public static Scene CreateFromFile(string path) => new() { _scene = Scn.FromFile(path) };
        }
        public struct Anim2D : IAsset
        {
            public readonly AssetType AssetType => AssetType.Anim2D;
            public readonly string Name => ToString()!;
            private ImageFrame[] frames;
            private int frame_ptr;
            public static Anim2D CreateFromPath(string path) => new() { frames = [.. Img.Load(path).Frames], frame_ptr = 0 };
        }
        public struct Anim3D : IAsset
        {
            public readonly AssetType AssetType => AssetType.Anim3D;
            public readonly string Name => ToString()!;
            private List<AnimClip> _clip;

            public static Anim3D CreateFromScene(ref Scn scene) => new() { _clip = [.. scene.AnimationClips] };
            public static Anim3D CreateFromScene(Scn scene) => new() { _clip = [.. scene.AnimationClips] };
            public static Anim3D CreateFromFile(string path) => CreateFromScene(Scn.FromFile(path));
            public static Anim3D CreateFromFile(Stream File) => CreateFromScene(Scn.FromStream(File));
        }
        public struct Asset : IAsset
        {
            public readonly AssetType AssetType => AssetType.RAW;
            public readonly string Name => ToString()!;

            private ReadOnlyMemory<byte> data;
            public static Asset CreateFromSpan(Span<byte> data) => new() { data = data.ToArray() };
        }
    }
}