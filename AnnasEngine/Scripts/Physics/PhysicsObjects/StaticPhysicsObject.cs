using AnnasEngine.Scripts.Physics.PhysicsShapes;
using AnnasEngine.Scripts.Utils.Extensions;
using MagicPhysX;
using MagicPhysX.Toolkit;
using Transform = AnnasEngine.Scripts.DataStructures.Transform;


namespace AnnasEngine.Scripts.Physics.PhysicsObjects;

public unsafe class StaticPhysicsObject : PhysicsObject
{
    public Rigidstatic Rigidstatic { get; }

    public StaticPhysicsObject(Transform transform, PhysicsShape physicsShape, PhysicsScene physicsScene, PxMaterial* pxMaterial)
        : base(physicsShape)
    {
        switch (physicsShape.Type)
        {
            case ColliderType.Box:
                BoxShape boxShape = (BoxShape)physicsShape;
                Rigidstatic = physicsScene.AddStaticBox(boxShape.HalfExtent.ToSystemVector3() * transform.scale.ToSystemVector3(), transform.position.ToSystemVector3(), transform.rotation.ToSystemQuaternion(), pxMaterial);
                break;

            case ColliderType.Sphere:
                SphereShape sphereShape = (SphereShape)physicsShape;

                Rigidstatic = physicsScene.AddStaticSphere(sphereShape.Radius * (transform.scale.X + transform.scale.Y + transform.scale.Z) / 3, transform.position.ToSystemVector3(), transform.rotation.ToSystemQuaternion(), pxMaterial);
                break;

            case ColliderType.Capsule:
                CapsuleShape capsuleShape = (CapsuleShape)physicsShape;

                Rigidstatic = physicsScene.AddStaticCapsule(capsuleShape.Radius * (transform.scale.X + transform.scale.Y + transform.scale.Z) / 3, capsuleShape.HalfHeight * (transform.scale.X + transform.scale.Y + transform.scale.Z) / 3, transform.position.ToSystemVector3(), transform.rotation.ToSystemQuaternion(), pxMaterial);
                break;

            case ColliderType.Plane:
                Rigidstatic = physicsScene.AddStaticPlane(0.0f, 1.0f, 0.0f, 0.0f, transform.position.ToSystemVector3(), transform.rotation.ToSystemQuaternion(), pxMaterial);
                break;

            default:
                throw new Exception();
        }
    }

    public override void BeforePhysicsUpdate()
    {

    }

    public override void AfterPhysicsUpdate()
    {

    }
}
