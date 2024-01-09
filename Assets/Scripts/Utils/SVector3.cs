using UnityEngine;

namespace SubjectGuide.Utils {
  public class SVector3 {
    public float X;
    public float Y;
    public float Z;

    public SVector3(Vector3 vec3) {
      X = vec3.x;
      Y = vec3.y;
      Z = vec3.z;
    }

    public SVector3(float x, float y, float z) {
      X = x;
      Y = y;
      Z = z;
    }

    public static SVector3 FromVector3(Vector3 vec3) {
      return new SVector3(vec3);
    }

    public static Vector3 ToVector3(SVector3 sVec3) {
      return new Vector3(sVec3.X, sVec3.Y, sVec3.Z);
    }

    public static SVector3 Zero => new(0, 0, 0);
    public static SVector3 One => new(1, 1, 1);
    public static SVector3 OneX => new(1, 0, 0);
    public static SVector3 OneY => new(0, 1, 0);
    public static SVector3 OneZ => new(0, 0, 1);
  }
}