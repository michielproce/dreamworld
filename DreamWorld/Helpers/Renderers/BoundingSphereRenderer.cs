using System;
using System.Collections.Generic;
using DreamWorld.Cameras;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DreamWorld.Helpers.Renderers
{

    public static class BoundingSphereRenderer
    {
        private class DrawableBoundingSphere
        {
            public Color Color;
            public BoundingSphere BoundingSphere;

            public DrawableBoundingSphere(BoundingSphere boundingSphere, Color color)
            {
                BoundingSphere = boundingSphere;
                Color = color;
            }
        }

        static VertexBuffer vertBuffer;
        static VertexDeclaration vertDecl;
        static BasicEffect effect;
        static int SphereResolution;

        private static GraphicsDevice GraphicsDevice;
        private static Camera Camera;

        private static List<DrawableBoundingSphere> BoundingSpheres = new List<DrawableBoundingSphere>();

        private static void Initialize(int sphereResolution)
        {
            SphereResolution = sphereResolution;

            vertDecl = new VertexDeclaration(GraphicsDevice, VertexPositionColor.VertexElements);
            effect = new BasicEffect(GraphicsDevice, null) {LightingEnabled = false, VertexColorEnabled = false};

            VertexPositionColor[] verts = new VertexPositionColor[sphereResolution * 3];

            int index = 0;

            //create the loop on the XY plane first
            for (; index < sphereResolution; index++)
            {
                float angle = (MathHelper.TwoPi / (sphereResolution - 1)) * index;
                verts[index] = new VertexPositionColor(
                    new Vector3((float)Math.Cos(angle), (float)Math.Sin(angle), 0f),
                    Color.White);
            }

            //next on the XZ plane
            for (; index < sphereResolution *2; index++)
            {
                float angle = (MathHelper.TwoPi / (sphereResolution - 1)) * (index - sphereResolution);
                verts[index] = new VertexPositionColor(
                    new Vector3((float)Math.Cos(angle), 0f, (float)Math.Sin(angle)),
                    Color.White);
            }

            //finally on the YZ plane
            for (; index < sphereResolution *3; index++)
            {
                float angle = (MathHelper.TwoPi / (sphereResolution - 1)) * (index - 2*sphereResolution);
                verts[index] = new VertexPositionColor(
                    new Vector3(0f, (float)Math.Cos(angle), (float)Math.Sin(angle)),
                    Color.White);
            }

            vertBuffer = new VertexBuffer(
                GraphicsDevice,
                verts.Length * VertexPositionColor.SizeInBytes,
                BufferUsage.None);
            vertBuffer.SetData(verts);
        }

        private static void RenderSphere(BoundingSphere sphere, Color color)
        {
            Matrix view = Camera.View;
            Matrix projection = Camera.Projection;


            GraphicsDevice.VertexDeclaration = vertDecl;
            GraphicsDevice.Vertices[0].SetSource(
                vertBuffer,
                0,
                VertexPositionColor.SizeInBytes);

            effect.World =
                Matrix.CreateScale(sphere.Radius) *
                Matrix.CreateTranslation(sphere.Center);
            effect.View = view;
            effect.Projection = projection;
            effect.DiffuseColor = color.ToVector3();

            effect.Begin();
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Begin();

                //render each circle individually
                GraphicsDevice.DrawPrimitives(
                    PrimitiveType.LineStrip,
                    0,
                    SphereResolution);
                GraphicsDevice.DrawPrimitives(
                    PrimitiveType.LineStrip,
                    SphereResolution,
                    SphereResolution-1);
                GraphicsDevice.DrawPrimitives(
                    PrimitiveType.LineStrip,
                    (SphereResolution * 2),
                    SphereResolution-1);

                pass.End();
            }
            effect.End();
        }


        public static void AddSphere(BoundingSphere sphere, Matrix world, Color color)
        {
            BoundingSpheres.Add(new DrawableBoundingSphere(sphere.Transform(world), color));
        }
        
        public static void Render(Camera camera, GraphicsDevice device)
        {
            GraphicsDevice = device;
            Camera = camera;

            if (vertBuffer == null)
                Initialize(25);

            foreach (DrawableBoundingSphere boundingSphere in BoundingSpheres)
            {
                RenderSphere(boundingSphere.BoundingSphere, boundingSphere.Color);
            }

            BoundingSpheres = new List<DrawableBoundingSphere>();
        }
    }
}