using shaderc;
using OpenTK.Graphics.OpenGL;

namespace RetroForge.NET.Assets
{
    namespace Asset
    {
        public struct Shader : IAsset
        {
            public readonly AssetType AssetType => AssetType.Shader;
            public ShaderType Type { readonly get; private set; }
            public readonly string Name => ToString()!;
            public int ID { get; private set; }
            private ReadOnlyMemory<byte> bytecode;
            

            public readonly void Dispose() {
                GL.DeleteShader(ID);
            }

            public static Shader CreateFromSpan(Span<byte> data) => new() { bytecode = data.ToArray() };

            public static Shader CompileShader(string source, string path, SourceLanguage language, ShaderKind kind)
            {
                using Compiler compiler = new(new()
                {
                    SourceLanguage = language,
                    Optimization = OptimizationLevel.Zero,
                });

                var res = compiler.Compile(source, path, kind, "main");
                if (res.ErrorCount > 0)
                    throw new(res.ErrorMessage);
                else
                {
                    Span<byte> shadercode;
                    unsafe
                    {
                        shadercode = new Span<byte>((void*)res.CodePointer, (int)res.CodeLength);
                    }
                    var shader = CreateFromSpan(shadercode);
                    shader.Type = kind switch
                    {
                        ShaderKind.VertexShader => ShaderType.VertexShader,
                        ShaderKind.FragmentShader => ShaderType.FragmentShader,
                        ShaderKind.ComputeShader => ShaderType.ComputeShader,
                        ShaderKind.GeometryShader => ShaderType.GeometryShader,
                        _ => 0,
                    };

                    if (!Path.Exists(Path.GetDirectoryName(path)))
                    {
                        Logger.Warn($"{Path.GetDirectoryName(path)} does not exist. Creating now.");
                        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
                    }
                    Logger.Log($"Writing {shader.Type} to {path}");
                    File.WriteAllBytes(path, shadercode);
                    
                    return shader;
                }
            }
            public unsafe Shader Upload(int ProgramID)
            {
                ID = GL.CreateShader(Type);
                int[] shaders = [ID];
                GL.ShaderBinary(1, shaders, ShaderBinaryFormat.ShaderBinaryFormatSpirV, (nint)bytecode.Pin().Pointer, bytecode.Length);
                GL.SpecializeShader(ID, "main", 0, [], []);
                GL.GetShaderi(ID, ShaderParameterName.CompileStatus, out int success);
                if (success == 1)
                    GL.AttachShader(ProgramID, ID);
                else
                { 
                    GL.GetShaderInfoLog(ID, out string infoLog);
                    Logger.Error(infoLog);
                }
                    

                    return this;
            }
        }
    }
}