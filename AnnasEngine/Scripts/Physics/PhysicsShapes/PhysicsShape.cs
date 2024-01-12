using MagicPhysX.Toolkit;
using OpenTK.Mathematics;

namespace AnnasEngine.Scripts.Physics.PhysicsShapes;

public class PhysicsShape
{
    public ColliderType Type { get; }
    public Vector3 Scale { get; set; }

    public PhysicsShape(ColliderType type, Vector3 scale)
    {
        Type = type;
        Scale = scale;
    }
}
