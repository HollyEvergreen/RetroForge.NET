namespace RetroForge.NET
{
    public struct Vertex(float x, float y, float z)
    {
        public float x = x;
        public float y = y;
        public float z = z;

        public static implicit operator (float, float, float)(Vertex v)
        {
            return (v.x, v.y, v.z);
        }
        public static implicit operator Vertex((float, float, float) v)
        {
            return new(v.Item1, v.Item2, v.Item3);
        }
    }
}