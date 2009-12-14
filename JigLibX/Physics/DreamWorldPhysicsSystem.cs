#region Using Statements
using System;
using System.Collections.Generic;
using JigLibX.Collision;
#endregion

namespace JigLibX.Physics
{
    public class DreamWorldPhysicsSystem 
    {
        public static DreamWorldPhysicsSystem CurrentPhysicsSystem;
        public CollisionSystem CollisionSystem;

        private List<Body> bodies = new List<Body>();
        private List<CollisionInfo> collisions = new List<CollisionInfo>();

        private float targetTime = 0.0f;

        private float collToll = 0.05f;

        public DreamWorldPhysicsSystem()
        {
            CurrentPhysicsSystem = this;
        }

        public void AddBody(Body body)
        {
            if (!bodies.Contains(body))
                bodies.Add(body);
            else
                System.Diagnostics.Debug.WriteLine("Warning: tried to add body to physics but it's already registered");

            if ((CollisionSystem != null) && (body.CollisionSkin != null))
                CollisionSystem.AddCollisionSkin(body.CollisionSkin);
        }

        public bool RemoveBody(Body body)
        {
            if ((CollisionSystem != null) && (body.CollisionSkin != null))
                CollisionSystem.RemoveCollisionSkin(body.CollisionSkin);

            if (!bodies.Contains(body))
                return false;

            bodies.Remove(body);
            return true;
        }

        private void UpdateAllPositions(float dt)
        {
            int numBodies = bodies.Count;
            for (int i = 0; i < numBodies; ++i)
                bodies[i].UpdatePositionWithAux(dt);
        }

        private void CopyAllCurrentStatesToOld()
        {
            int numBodies = bodies.Count;
            for (int i = 0; i < numBodies; ++i)
            {
                if (bodies[i].IsActive || bodies[i].VelChanged)
                    bodies[i].CopyCurrentStateToOld();
            }
        }

        Random rand = new Random();
        private void DetectAllCollisions(float dt)
        {
            if (CollisionSystem == null)
                return;

            int numBodies = bodies.Count;
            int numColls = collisions.Count;

            UpdateAllPositions(dt);

            for (int i = 0; i < numColls; ++i)
                CollisionInfo.FreeCollisionInfo(collisions[i]);

            collisions.Clear();

            for (int i = 0; i < numBodies; ++i)
            {
                if (bodies[i].CollisionSkin != null)
                    bodies[i].CollisionSkin.Collisions.Clear();
            }

            BasicCollisionFunctor collisionFunctor = new BasicCollisionFunctor(collisions);
            CollisionSystem.DetectAllCollisions(bodies, collisionFunctor, null, collToll);

            int index; CollisionInfo collInfo;
            for (int i = 1; i < collisions.Count; i++)
            {
                index = rand.Next(i + 1);
                collInfo = collisions[i];
                collisions[i] = collisions[index];
                collisions[index] = collInfo;
            }

        }

        public void Integrate(float dt)
        {
            targetTime += dt;

            CopyAllCurrentStatesToOld();

            DetectAllCollisions(dt);
        }
    }
}

