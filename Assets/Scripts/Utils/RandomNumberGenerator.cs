using System;

namespace SubjectGuide.Utils {
  public sealed class RandomNumberGenerator {
    private static Random s_random = new();
    public static double RandomDoubleValue(double min, double max) {
      return s_random.NextDouble() * (max - min) + min;
    }
  }
}