using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DreamWorld.Helpers.Debug
{
    public class TriangleFinder
    {
        public static List<Vector3[]> find(ModelMesh mesh, Matrix world)
        {
            List<Vector3[]> triangles = new List<Vector3[]>();

            foreach (ModelMeshPart part in mesh.MeshParts)
            {
                if (mesh.IndexBuffer.IndexElementSize == IndexElementSize.SixteenBits)
                {
                    short[] indices = new short[part.PrimitiveCount * 3];
                    mesh.IndexBuffer.GetData<short>(part.StartIndex * sizeof(short),
                                indices, 0, part.PrimitiveCount * 3);
                    Vector3[] vertices = new Vector3[part.NumVertices];
                    mesh.VertexBuffer.GetData<Vector3>(
                                part.BaseVertex * part.VertexStride,
                                vertices,
                                0,
                                part.NumVertices,
                                part.VertexStride);

                    for (int x = 0; x < part.PrimitiveCount; x++)
                    {
                        Vector3 a = vertices[indices[x * 3 + 0]];
                        Vector3 b = vertices[indices[x * 3 + 1]];
                        Vector3 c = vertices[indices[x * 3 + 2]];

                        a = Vector3.Transform(a, world);
                        b = Vector3.Transform(b, world);
                        c = Vector3.Transform(c, world);

                        triangles.Add(new Vector3[] { a, b, c });

                    }
                }

                else
                {
                    // TODO (32bit)

                }

            }
            return triangles;
        }
    }
}
