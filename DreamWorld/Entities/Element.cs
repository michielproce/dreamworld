using Microsoft.Xna.Framework;

namespace DreamWorld.Entities
{
    public class Element : Entity
    {
        public Group Group { get; internal set; }

        public void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // Move the element's origin to the world's origin
            Body.Position -= Group.Body.Position;

            // Rotate the element (around the world's origin)
            Body.Orientation *= Matrix.CreateFromQuaternion(Group.RotationSinceLastUpdate);
            Body.Position = Vector3.Transform(Body.Position, Group.RotationSinceLastUpdate);

            // Move the element's origin back
            Body.Position += Group.Body.Position;
        }
    }
}