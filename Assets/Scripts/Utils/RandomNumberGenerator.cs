using UnityEngine;

namespace SubjectGuide.Utils {
  public sealed class RandomNumberGenerator {
    private static readonly System.Random s_random = new();
    public static double RandomDoubleValue(double min, double max) {
      return s_random.NextDouble() * (max - min) + min;
    }

    public static int RandomInt(int min, int max) {
      return s_random.Next(min, max + 1);
    }

    public static Vector2 GetRandomPosition(Vector2 min, Vector2 max) {
      var randomX = (float)s_random.NextDouble() * (max.x - min.x) + min.x;
      var randomY = (float)s_random.NextDouble() * (max.y - min.y) + min.y;
      return new(randomX, randomY);
    }

    public static Quaternion GetRandomRotationY() {
      var rotY = (float)s_random.NextDouble() * (360 - 0) + 0;
      return Quaternion.Euler(0, rotY, 0);
    }
  }
}