//3D format demonstration is simple

//3D format demonstration is simple
using RetroForge.NET;

namespace RetroForge.NET.Geometry;

public struct Triangle(int _1, int _2, int _3)
{
    public int _1 = _1;
    public int _2 = _2;
    public int _3 = _3;
    public static implicit operator (int, int, int)(Triangle v)
    {
        return (v._1, v._2, v._3);
    }
    public static implicit operator Triangle((int, int, int) v)
    {
        return new(v.Item1, v.Item2, v.Item3);
    }
};

public struct MeshBlock(Vertex[] vertices, Triangle[] faces)
{
    public Vertex[] vertices = vertices;
    public Triangle[] faces = faces;
};

public struct GLMesh
{
    readonly List<MeshBlock> MeshBlocks = [];
    public GLMesh(){}

    public static GLMesh Quad
    {
        get => new GLMesh().AddBlock(
            new(
                [
                    (-0.5f, 0.5f, 0f),
                    (0.5f, 0.5f, 0f),
                    (0.5f, -0.5f, 0),
                    (-0.5f, -0.5f, 0)
                ],
                [
                    (3, 2, 0),
                    (0, 1, 2)
                ]
            )
        );
    }

    public readonly GLMesh AddBlock(MeshBlock block)
    {
        MeshBlocks.Add( block );

        return this;
    }

    public MeshBlock GetBlock(int i)
    {
        return MeshBlocks[i];
    }
    public void DebugTris()
    {
        int i = 0;
        var verts = MeshBlocks[0].vertices;
        foreach (var tri in MeshBlocks[0].faces)
        {
            i++;
            var vert = verts[tri._1];
            Logger.Log($"tri  {i}: {vert.x}:{vert.y}:{vert.z}");

            vert = verts[tri._2];
            Logger.Log($"tri {i}: {vert.x}:{vert.y}:{vert.z}");

            vert = verts[tri._3];
            Logger.Log($"tri {i}: {vert.x}:{vert.y}:{vert.z}");
        }
    }
}