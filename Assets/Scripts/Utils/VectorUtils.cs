using UnityEngine;

namespace SubjectGuide.Utils {
  public static class VectorUtils {
    public static Vector3 ToVector3(this Vector2 vec2) {
      return (Vector3)vec2;
    }
  }
}