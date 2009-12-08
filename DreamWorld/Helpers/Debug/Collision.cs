using Microsoft.Xna.Framework;

namespace DreamWorld.Helpers.Debug
{
    class Collision
    {
        public static bool RayTriangleIntersect(Ray r,
                    Vector3[] t,
                    out float dist)
        {
            dist = 0;

            Vector3 edge1 = t[1] - t[0];
            Vector3 edge2 = t[2] - t[0];

            Vector3 tvec, pvec, qvec;
            float det, inv_det;

            pvec = Vector3.Cross(r.Direction, edge2);

            det = Vector3.Dot(edge1, pvec);

            if (det > -0.00001f)
                return false;

            inv_det = 1.0f / det;

            tvec = r.Position - t[0];

            float u = Vector3.Dot(tvec, pvec) * inv_det;
            if (u < -0.001f || u > 1.001f)
                return false;

            qvec = Vector3.Cross(tvec, edge1);

            float v = Vector3.Dot(r.Direction, qvec) * inv_det;
            if (v < -0.001f || u + v > 1.001f)
                return false;

            dist = Vector3.Dot(edge2, qvec) * inv_det;

            if (dist <= 0)
                return false;

            return true;
        }
    }
}
