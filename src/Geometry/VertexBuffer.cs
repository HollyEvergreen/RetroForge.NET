using OpenTK.Graphics.OpenGL;
using RetroForge.NET.Geometry;

namespace RetroForge.NET
{
    public class MeshBuffer
    {
        public readonly int VertexBufferID;
        public readonly int ElementBufferID;
        List<Vertex> vertices;
        List<Triangle> faces;
        float[] glVerts => toGLVertArray(vertices);
        int[] indices => toGLIndices(faces);

        private int[] toGLIndices(List<Triangle> faces)
        {
            int[] _faces = new int[faces.Count * 3];
            int i = 0;
            faces.ForEach((face) =>
            {
                _faces[i] = face._1;
                _faces[i + 1] = face._2;
                _faces[i + 2] = face._3;
                i+=3;
            });
            return _faces;
        }
        public int index_count => faces.Count*3;

        int vertex_size => vertices.Count * 3 * sizeof(float);
        int index_size => faces.Count * 3 * sizeof(uint);
        double last_upload = 0;
        public MeshBuffer()
        {
            vertices ??= [];
            faces ??= [];
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

        static float[] toGLVertArray(List<Vertex> vertices)
        {
            float[] verts = new float[vertices.Count * 3];
            int i = 0;
            vertices.ForEach((vert) =>
            {
                verts[i] = vert.x;
                verts[i + 1] = vert.y;
                verts[i + 2] = vert.z;
                i+=3;
            });
            return verts;
        }

        public void AddVerts(Vertex[] new_verts) => vertices.AddRange(new_verts);

        public void AddFaces(Triangle[] new_faces) => faces.AddRange(new_faces);
    }
}