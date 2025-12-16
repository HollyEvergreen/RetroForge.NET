using OpenTK.Graphics.OpenGL;
using RetroForge.NET.Geometry;
using System;
using System.Collections.Generic;
using System.Text;

namespace RetroForge.NET.Geometry
{
    public class VAO
    {
        int ID;
        List<MeshBuffer> MeshBuffers;
        List<GLMesh> meshes;

        public VAO()
        {
            ID = GL.GenVertexArray();
            MeshBuffers ??= [new MeshBuffer()];
            meshes ??= [];
        }

        public VAO AddMesh(GLMesh mesh)
        {
            meshes.Add(mesh);
            return this;
        }

        public VAO SetAttribPointer<_Type>(uint index, int size, nint offset = 0, bool normalised = false)
        {
            Type T = typeof(_Type);
            VertexAttribPointerType GL_T = T.Name switch
            {
                "Byte" => VertexAttribPointerType.Byte,
                "Int32" => VertexAttribPointerType.Int,
                "UInt32" => VertexAttribPointerType.UnsignedInt,
                "Single" => VertexAttribPointerType.Float,
                "Double" => VertexAttribPointerType.Double,
                _ => throw new NotSupportedException("unsupported type"),
            };
            int tsize = 0;
            unsafe { tsize = sizeof(_Type) * size; }
            GL.BindVertexArray(ID);
            GL.BindBuffer(BufferTarget.ArrayBuffer, MeshBuffers[0].VertexBufferID);
            GL.VertexAttribPointer(index, size, GL_T, false, tsize, offset);
            GL.EnableVertexAttribArray(index);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, MeshBuffers[0].ElementBufferID);
            GL.BindVertexArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            return this;
        }

        public VAO UseVB(int i, Action<MeshBuffer> action)
        {
            action(MeshBuffers[i]);
            return this;
        }

        public void Draw()
        {
            Engine.drawcalls++;
            GL.BindVertexArray(ID);
            GL.DrawElements(PrimitiveType.Triangles, MeshBuffers[0].index_count, DrawElementsType.UnsignedInt, 0);
        }

        public GLMesh GetMesh(int i)
        {
            return meshes[i];
        }
    }
}
