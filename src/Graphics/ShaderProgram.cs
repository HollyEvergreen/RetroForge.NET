using OpenTK.Graphics.OpenGL;
using RetroForge.NET.Assets.Asset;
using shaderc;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Text;

namespace RetroForge.NET.Graphics
{
	public class ShaderProgram : IDisposable
	{
		public int ProgramID;
		public List<ShaderInfo> shaderInfos;
        private bool disposedValue;

        public ShaderProgram(Shader[] Shaders)
		{
			shaderInfos ??= [];
			ProgramID = GL.CreateProgram();
            foreach (var shader in Shaders)
            {
				shader.Upload(ProgramID);
				shaderInfos.Add(new() { ID = shader.ID, shader_type = shader.Type});
            }
			GL.LinkProgram(ProgramID);
			int success;
			unsafe
			{
				GL.GetProgramiv(ProgramID, ProgramProperty.LinkStatus, &success);
			}
            if (success == 0)
            {
                GL.GetProgramInfoLog(ProgramID, out string infoLog);
				Logger.Log(infoLog);
            }
			foreach (var shader in Shaders)
			{
				GL.DeleteShader(shader.ID);
            }
			try
			{
				Engine.Instance.Window.Unload += Dispose;
			}
			catch (NullReferenceException ex)
			{
				Logger.Error(ex);
			}
        }
		public ShaderProgram ScopedUse(Action action)
		{
			Use();
				action();
			End();
			return this;
		}
		public ShaderProgram Use()
		{
			GL.UseProgram(ProgramID);
			return this;
		}
		public ShaderProgram End()
		{
			GL.UseProgram(0);
			return this;
		}

		public void SetUniform(int loc)
		{
			Logger.Error("ShaderProgram.SetUniform is not implemented");
		}
        public virtual void Dispose()
        {
            if (!disposedValue)
            {
                shaderInfos.Clear();
				GL.DeleteProgram(ProgramID);
                disposedValue = true;
				GC.SuppressFinalize(this);
            }
        }
		~ShaderProgram()
		{
			if (!disposedValue)
			{
				Logger.Error("GPU RESOURCE LEAK");
			}
		}
        public uint GetAttribLocation(string attribName)
        {
            return (uint)GL.GetAttribLocation(ProgramID, attribName);
			
        }
		public unsafe List<string> GetAttribs()
		{
			List<string> strs = [];
			int _params = 0;
			GL.GetProgramiv(ProgramID, ProgramProperty.ActiveAttributes, &_params);
			for (uint i = 0; i < _params; i++)
			{
				int strlen;
				int AttributeSize;
				AttributeType attributeType;
				byte* name = (byte*)Marshal.AllocHGlobal(128);
                GL.GetActiveAttrib(ProgramID, i, 128, &strlen, &AttributeSize, &attributeType, name);
				string _name = "null";
				if (name != (void*)0)
				{
					_name = Marshal.PtrToStringAuto((nint)name, strlen)!;
				}

				Logger.Log($"base shader {ProgramID}: attribute name <{_name}>[{strlen}] -> pos:{i} => Attr Type:{attributeType} => Attr Size:{AttributeSize}");
				strs.Add(_name);
			}

            return strs;
		}
    }
	public struct ShaderInfo
	{
		public int ID;
		public ShaderType shader_type;
	}
}