using System;
using Microsoft.Xna.Framework;

namespace DreamWorld.Util
{
    public class Curve3D
    {
        private Curve curveX;
        private Curve curveY;
        private Curve curveZ;

        private float length;

        public Curve3D(CurveLoopType loopType)
            : this(loopType, loopType)
        {
        }

        public Curve3D(CurveLoopType preLoopType, CurveLoopType postLoopType)
        {
            curveX = new Curve();
            curveY = new Curve();
            curveZ = new Curve();

            curveX.PostLoop = postLoopType;
            curveY.PostLoop = postLoopType;
            curveZ.PostLoop = postLoopType;

            curveX.PreLoop = preLoopType;
            curveY.PreLoop = preLoopType;
            curveZ.PreLoop = preLoopType;
        }

        public void AddPoint(Vector3 point)
        {
            AddPoint(point, length++);
        }

        public void AddPoint(Vector3 point, float time)
        {
            curveX.Keys.Add(new CurveKey(time, point.X));
            curveY.Keys.Add(new CurveKey(time, point.Y));
            curveZ.Keys.Add(new CurveKey(time, point.Z));
            if(time > length)
                length = time;
        }
       

        public Vector3 GetPointOnCurve(float time)
        {
            return new Vector3
                    {
                        X = curveX.Evaluate(time),
                        Y = curveY.Evaluate(time),
                        Z = curveZ.Evaluate(time)
                    };
        }        


        public void SetTangents()
        {
            CurveKey prev;
            CurveKey current;
            CurveKey next;
            int prevIndex;
            int nextIndex;
            for (int i = 0; i < curveX.Keys.Count; i++)
            {
                prevIndex = i - 1;
                if (prevIndex < 0) prevIndex = i;

                nextIndex = i + 1;
                if (nextIndex == curveX.Keys.Count) nextIndex = i;

                prev = curveX.Keys[prevIndex];
                next = curveX.Keys[nextIndex];
                current = curveX.Keys[i];
                SetCurveKeyTangent(ref prev, ref current, ref next);
                curveX.Keys[i] = current;
                prev = curveY.Keys[prevIndex];
                next = curveY.Keys[nextIndex];
                current = curveY.Keys[i];
                SetCurveKeyTangent(ref prev, ref current, ref next);
                curveY.Keys[i] = current;

                prev = curveZ.Keys[prevIndex];
                next = curveZ.Keys[nextIndex];
                current = curveZ.Keys[i];
                SetCurveKeyTangent(ref prev, ref current, ref next);
                curveZ.Keys[i] = current;
            }
        }

        private void SetCurveKeyTangent(ref CurveKey prev, ref CurveKey cur, ref CurveKey next)
        {
            float dt = next.Position - prev.Position;
            float dv = next.Value - prev.Value;
            if (Math.Abs(dv) < float.Epsilon)
            {
                cur.TangentIn = 0;
                cur.TangentOut = 0;
            }
            else
            {
                // The in and out tangents should be equal to the slope between the adjacent keys.
                cur.TangentIn = dv * (cur.Position - prev.Position) / dt;
                cur.TangentOut = dv * (next.Position - cur.Position) / dt;
            }
        }

    }
}
