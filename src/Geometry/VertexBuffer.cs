using OpenTK.Graphics.OpenGL;
using RetroForge.NET.Geometry;
using System.Numerics;

namespace RetroForge.NET
{
    public class MeshBuffer
    {
        public readonly int VertexBufferID;
        public readonly int ElementBufferID;
        List<Vertex> vertices;
        List<Triangle> faces;
        List<Vector2> texCoords;
        float[] glVerts => toGLVertArray(vertices, texCoords);
        int[] indices => toGLIndices(faces);

        public int index_count => faces.Count*3;

        int vertex_size => vertices.Count * 5 * sizeof(float);  // 5 floats per vertex (x,y,z,u,v)
        int index_size => faces.Count * 3 * sizeof(uint);
        double last_upload = 0;
        public MeshBuffer()
        {
            vertices ??= [];
            faces ??= [];
            texCoords ??= [];
            VertexBufferID = GL.GenBuffer();
            ElementBufferID = GL.GenBuffer();
        }

        public void Upload()
        {
            if (Engine.Now - last_upload < 100)
                Logger.Warn("Avoid overwriting vertex data more often than you have to");
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferID);
            GL.BufferData(BufferTarget.ArrayBuffer, vertex_size, glVerts, BufferUsage.StaticDraw);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferID);
            GL.BufferData(BufferTarget.ElementArrayBuffer, index_size, indices, BufferUsage.StaticDraw);

            last_upload = Engine.Now;
        }

        public void Delete()
        {
            GL.DeleteBuffer(VertexBufferID);
        }

        static float[] toGLVertArray(List<Vertex> vertices, List<Vector2> texCoords)
        {
            float[] verts = new float[vertices.Count * 5];
            for (int i = 0; i < vertices.Count; i++)
            {
                var vert = vertices[i];
                var texCoord = i < texCoords.Count ? texCoords[i] : new Vector2(0, 0);

                int idx = i * 5;  // Calculate base index
                verts[idx + 0] = vert.x;
                verts[idx + 1] = vert.y;
                verts[idx + 2] = vert.z;
                verts[idx + 3] = texCoord.X;
                verts[idx + 4] = texCoord.Y;
                Logger.Log($"Vertex {i}: pos({vert.x}, {vert.y}, {vert.z}) tex({texCoord.X}, {texCoord.Y})");
            }
            return verts;
        }
        private int[] toGLIndices(List<Triangle> faces)
        {
            int[] _faces = new int[faces.Count * 3];
            int i = 0;
            faces.ForEach((face) =>
            {
                _faces[i] = face._1;
                _faces[i + 1] = face._2;
                _faces[i + 2] = face._3;
                i += 3;
            });
            return _faces;
        }

        public void AddVerts(Vertex[] new_verts) => vertices.AddRange(new_verts);

        public void AddFaces(Triangle[] new_faces) => faces.AddRange(new_faces);
        public void AddTexCoords(Vector2[] new_texCoords) => texCoords.AddRange(new_texCoords);
    }
}