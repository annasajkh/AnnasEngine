using MagicPhysX.Toolkit;
using OpenTK.Mathematics;

namespace AnnasEngine.Scripts.Physics.PhysicsShapes
{
    public class BoxShape : PhysicsShape
    {
        public Vector3 HalfExtent { get; }

        public BoxShape(Vector3 halfExtent) : base(ColliderType.Box, Vector3.One)
        {
            HalfExtent = halfExtent;
        }
    }
}
