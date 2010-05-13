using System;
using Microsoft.Xna.Framework;

namespace DreamWorld.Util
{
    public class Curve3D
    {
        private readonly Curve _curveX;
        private readonly Curve _curveY;
        private readonly Curve _curveZ;

        private float _length;
        private Vector3? _lastPoint;

        public Curve3D(CurveLoopType loopType)
            : this(loopType, loopType)
        {
        }

        public Curve3D(CurveLoopType preLoopType, CurveLoopType postLoopType)
        {
            _curveX = new Curve();
            _curveY = new Curve();
            _curveZ = new Curve();

            _curveX.PostLoop = postLoopType;
            _curveY.PostLoop = postLoopType;
            _curveZ.PostLoop = postLoopType;

            _curveX.PreLoop = preLoopType;
            _curveY.PreLoop = preLoopType;
            _curveZ.PreLoop = preLoopType;
        }

        public void AddPoint(Vector3 point)
        {
            if(_lastPoint.HasValue)
                _length += Vector3.Distance(point, (Vector3)_lastPoint);
            AddPoint(point, _length);
        }

        public void AddPoint(Vector3 point, float time)
        {
            _curveX.Keys.Add(new CurveKey(time, point.X));
            _curveY.Keys.Add(new CurveKey(time, point.Y));
            _curveZ.Keys.Add(new CurveKey(time, point.Z));    
            _lastPoint = point;
        }
       

        public Vector3 GetPointOnCurve(float time)
        {
            return new Vector3
                    {
                        X = _curveX.Evaluate(time),
                        Y = _curveY.Evaluate(time),
                        Z = _curveZ.Evaluate(time)
                    };
        }        


        public void SetTangents()
        {
            CurveKey prev;
            CurveKey current;
            CurveKey next;
            int prevIndex;
            int nextIndex;
            for (int i = 0; i < _curveX.Keys.Count; i++)
            {
                prevIndex = i - 1;
                if (prevIndex < 0) prevIndex = i;

                nextIndex = i + 1;
                if (nextIndex == _curveX.Keys.Count) nextIndex = i;

                prev = _curveX.Keys[prevIndex];
                next = _curveX.Keys[nextIndex];
                current = _curveX.Keys[i];
                SetCurveKeyTangent(ref prev, ref current, ref next);
                _curveX.Keys[i] = current;
                prev = _curveY.Keys[prevIndex];
                next = _curveY.Keys[nextIndex];
                current = _curveY.Keys[i];
                SetCurveKeyTangent(ref prev, ref current, ref next);
                _curveY.Keys[i] = current;

                prev = _curveZ.Keys[prevIndex];
                next = _curveZ.Keys[nextIndex];
                current = _curveZ.Keys[i];
                SetCurveKeyTangent(ref prev, ref current, ref next);
                _curveZ.Keys[i] = current;
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
