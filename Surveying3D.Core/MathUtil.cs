using System.Numerics;
using ObjLoader.Loader.Data.VertexData;

namespace Surveying3D.Core
{
    public static class MathUtil
    {
        public static float SarrusRule(in Vector3 v1, in Vector3 v2, in Vector3 v3)
        {
            var positive1 = v1.X * v2.Y * v3.Z;
            var positive2 = v2.X * v3.Y * v1.Z;
            var positive3 = v3.X * v1.Y * v2.Z;

            var negative1 = v1.X * v3.Y * v2.Z;
            var negative2 = v2.X * v1.Y * v3.Z;
            var negative3 = v3.X * v2.Y * v1.Z;

            return positive1 + positive2 + positive3 - negative1 - negative2 - negative3;
        }

        public static Vector3 AsVector3(this Vertex v)
        {
            return new Vector3(v.X, v.Y, v.Z);
        }
    }
}