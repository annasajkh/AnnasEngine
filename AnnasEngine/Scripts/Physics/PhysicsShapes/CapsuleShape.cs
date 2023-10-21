using MagicPhysX.Toolkit;
using OpenTK.Mathematics;
using AnnasEngine.Scripts.Physics.PhysicsShapes;

namespace AnnasEngine.Scripts.Physics.PhysicsShapes
{
    public class CapsuleShape : PhysicsShape
    {
        public float Radius { get; }
        public float HalfHeight { get; }


        public CapsuleShape(float radius, float halfHeight) : base(ColliderType.Capsule, Vector3.One)
        {
            Radius = radius;
            HalfHeight = halfHeight;
        }
    }
}
