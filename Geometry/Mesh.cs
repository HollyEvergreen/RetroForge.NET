//3D format demonstration is simple
struct Vertex {
    float x;
    float y;
    float z;
};
 
struct Triangle {
    int a;
    int b;
    int c;
};
 
struct MeshBlock {
    Vertex[] vertices;
    Triangle[] faces;
};
