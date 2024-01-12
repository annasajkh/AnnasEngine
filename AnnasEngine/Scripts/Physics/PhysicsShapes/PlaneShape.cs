using MagicPhysX.Toolkit;
using OpenTK.Mathematics;

namespace AnnasEngine.Scripts.Physics.PhysicsShapes;

public class PlaneShape : PhysicsShape
{
    public PlaneShape(Vector3 scale) : base(ColliderType.Plane, scale)
    {

    }
}
