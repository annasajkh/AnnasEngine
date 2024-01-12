using OpenTK.Mathematics;

namespace AnnasEngine.Scripts.DataStructures;

public class Transform
{
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 scale;

    public Transform(Vector3 position, Quaternion rotation, Vector3 scale)
    {
        this.position = position;
        this.rotation = rotation;
        this.scale = scale;
    }

    public static Transform operator +(Transform transformA, Transform transformB)
    {
        return new Transform(position: transformA.position + transformB.position,
                             rotation: transformA.rotation + transformB.rotation,
                             scale: transformA.scale + transformB.scale);
    }

    public static Transform operator -(Transform transformA, Transform transformB)
    {
        return new Transform(position: transformA.position - transformB.position,
                             rotation: transformA.rotation - transformB.rotation,
                             scale: transformA.scale - transformB.scale);
    }

    public static Transform operator *(Transform transformA, Transform transformB)
    {
        return new Transform(position: transformA.position * transformB.position,
                             rotation: transformA.rotation * transformB.rotation,
                             scale: transformA.scale * transformB.scale);
    }
}
