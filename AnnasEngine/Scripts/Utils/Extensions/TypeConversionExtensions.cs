using OpenTK.Mathematics;

namespace AnnasEngine.Scripts.Utils.Extensions;

public static class TypeConversionExtensions
{
    // Why opentk implement their own quaternion instead of using the build in one
    // Now i have to do this stupid conversion
    public static Vector3 ToOpenTKVector3(this System.Numerics.Vector3 vector3)
    {
        return new Vector3(vector3.X, vector3.Y, vector3.Z);
    }

    public static Quaternion ToOpenTKQuaternion(this System.Numerics.Quaternion quaternion)
    {
        return new Quaternion(quaternion.X, quaternion.Y, quaternion.Z, quaternion.W);
    }

    public static System.Numerics.Vector3 ToSystemVector3(this Vector3 vector3)
    {
        return new System.Numerics.Vector3(vector3.X, vector3.Y, vector3.Z);
    }

    public static System.Numerics.Quaternion ToSystemQuaternion(this Quaternion quaternion)
    {
        return new System.Numerics.Quaternion(quaternion.X, quaternion.Y, quaternion.Z, quaternion.W);
    }
}
