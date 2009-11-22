using System.Collections.Generic;
using DreamWorld.Cameras;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DreamWorld.Helpers.Renderers
{
    public static class LineRenderer
    {
        private static List<Line> lines = new List<Line>();

        private static VertexPositionColor[] vertices = new VertexPositionColor[2];
        private static int[] indices = new int[]
            {
                0, 1,
            };

        static BasicEffect effect;
        static VertexDeclaration vertDecl;


        public static void AddLine(Line line)
        {
            lines.Add(line);
        }
        

        public static void RemoveLine(Line line)
        {
            lines.Remove(line);
        }
        

        public static void Reset()
        {
            lines.Clear();
        }


        public static void Render(Camera camera, GraphicsDevice device)
        {
            if (effect == null || vertDecl == null)
            {
                effect = new BasicEffect(device, null) {VertexColorEnabled = true, LightingEnabled = false};
                vertDecl = new VertexDeclaration(device, VertexPositionColor.VertexElements);
            }

            foreach (Line line in lines)
            {


                vertices[0].Position = line.A;
                vertices[0].Color = line.C;

                vertices[1].Position = line.B;
                vertices[1].Color = line.C;


                device.VertexDeclaration = vertDecl;

                effect.View = camera.View;
                effect.Projection = camera.Projection;

                effect.Begin();
                foreach (EffectPass pass in effect.CurrentTechnique.Passes)
                {
                    pass.Begin();


                    device.DrawUserIndexedPrimitives(
                        PrimitiveType.LineList,
                        vertices,
                        0,
                        2,
                        indices,
                        0,
                        indices.Length/2);

                    pass.End();
                }
                effect.End();
            }
        }

    }


    public class Line
    {
        public Vector3 A { get; private set; }
        public Vector3 B { get; private set; }
        public Color C { get; private set; }

        public Line(Vector3 a, Vector3 b)
            : this(a, b, Color.Blue)
        {
        }

        public Line(Vector3 a, Vector3 b, Color c)
        {
            A = a;
            B = b;
            C = c;
        }
    }
}
