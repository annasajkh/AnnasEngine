using AnnasEngine.Scripts.DataStructures.GameObjects;
using AnnasEngine.Scripts.Physics.PhysicsShapes;
using AnnasEngine.Scripts.Utils.Extensions;
using MagicPhysX;
using MagicPhysX.Toolkit;
using MagicPhysX.Toolkit.Colliders;
using Transform = AnnasEngine.Scripts.DataStructures.Transform;

#pragma warning disable CS8618

namespace AnnasEngine.Scripts.Physics.PhysicsObjects
{
    public unsafe class DynamicPhysicsObject : PhysicsObject
    {
        public Rigidbody Rigidbody { get; }

        public DynamicPhysicsObject(Transform transform, float density, PhysicsShape physicsShape, PhysicsScene physicsScene, PxMaterial* pxMaterial)
            : base(physicsShape)
        {
            switch (physicsShape.Type)
            {
                case ColliderType.Box:
                    BoxShape boxShape = (BoxShape)physicsShape;

                    Rigidbody = physicsScene.AddDynamicBox(boxShape.HalfExtent.ToSystemVector3(), transform.position.ToSystemVector3(), transform.rotation.ToSystemQuaternion(), density, pxMaterial);
                    break;

                case ColliderType.Sphere:
                    SphereShape sphereShape = (SphereShape)physicsShape;

                    Rigidbody = physicsScene.AddDynamicSphere(sphereShape.Radius, transform.position.ToSystemVector3(), transform.rotation.ToSystemQuaternion(), density, pxMaterial);
                    break;

                case ColliderType.Capsule:
                    CapsuleShape capsuleShape = (CapsuleShape)physicsShape;

                    Rigidbody = physicsScene.AddDynamicCapsule(capsuleShape.Radius, capsuleShape.HalfHeight, transform.position.ToSystemVector3(), transform.rotation.ToSystemQuaternion(), density, pxMaterial);
                    break;

                case ColliderType.Plane:
                    throw new Exception("Plane shape can only be static");
            }
        }

        public override void BeforePhysicsUpdate()
        {
            PhysicsShape.Scale = ((GameObject3D)GetParent()).Transform.scale;

            Rigidbody.position = ((GameObject3D)GetParent()).Transform.position.ToSystemVector3();
            Rigidbody.rotation = ((GameObject3D)GetParent()).Transform.rotation.ToSystemQuaternion();


            switch (PhysicsShape.Type)
            {
                case ColliderType.Box:
                    BoxCollider boxCollider = (BoxCollider)Rigidbody.GetComponent<Collider>();
                    BoxShape boxShape = (BoxShape)PhysicsShape;

                    boxCollider.size = (boxShape.HalfExtent * boxShape.Scale).ToSystemVector3();

                    break;

                case ColliderType.Sphere:
                    SphereCollider sphereCollider = (SphereCollider)Rigidbody.GetComponent<Collider>();
                    SphereShape sphereShape = (SphereShape)PhysicsShape;

                    System.Numerics.Vector3 sphereRadius = (sphereShape.Radius * sphereShape.Scale).ToSystemVector3();

                    sphereCollider.radius = (sphereRadius.X + sphereRadius.Y + sphereRadius.Z) / 3;
                    break;

                case ColliderType.Capsule:
                    CapsuleCollider capsuleCollider = (CapsuleCollider)Rigidbody.GetComponent<Collider>();
                    CapsuleShape capsuleShape = (CapsuleShape)PhysicsShape;

                    System.Numerics.Vector3 capsuleRadius = (capsuleShape.Radius * capsuleShape.Scale).ToSystemVector3();
                    System.Numerics.Vector3 capsuleHeight = (capsuleShape.HalfHeight * capsuleShape.Scale).ToSystemVector3();

                    capsuleCollider.radius = (capsuleRadius.X + capsuleRadius.Y + capsuleRadius.Z) / 3;
                    capsuleCollider.height = (capsuleHeight.X + capsuleHeight.Y + capsuleHeight.Z) / 3;

                    break;

                case ColliderType.Plane:
                    throw new Exception("Plane shape can only be static");
            }
        }

        public override void AfterPhysicsUpdate()
        {
            ((GameObject3D)GetParent()).Transform.position = Rigidbody.position.ToOpenTKVector3();
            ((GameObject3D)GetParent()).Transform.rotation = Rigidbody.rotation.ToOpenTKQuaternion();
        }
    }
}
