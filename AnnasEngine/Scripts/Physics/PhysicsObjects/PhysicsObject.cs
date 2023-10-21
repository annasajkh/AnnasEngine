using AnnasEngine.Scripts.DataStructures.Containers;
using AnnasEngine.Scripts.Physics.PhysicsShapes;

namespace AnnasEngine.Scripts.Physics.PhysicsObjects
{
    public abstract class PhysicsObject : GameObjectComponent
    {
        public PhysicsShape PhysicsShape { get; }

        public PhysicsObject(PhysicsShape physicsShape)
        {
            PhysicsShape = physicsShape;
        }

        public abstract void BeforePhysicsUpdate();
        public abstract void AfterPhysicsUpdate();
    }
}
