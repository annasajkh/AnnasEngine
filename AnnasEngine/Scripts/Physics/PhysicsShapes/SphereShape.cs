using MagicPhysX.Toolkit;
using OpenTK.Mathematics;

namespace AnnasEngine.Scripts.Physics.PhysicsShapes;

public class SphereShape : PhysicsShape
{
    public float Radius { get; }

    public SphereShape(float radius) : base(ColliderType.Sphere, Vector3.One)
    {
        Radius = radius;
    }
}
