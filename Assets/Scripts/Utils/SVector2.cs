using System;
using Newtonsoft.Json;
using UnityEngine;

namespace SubjectGuide.Utils {
  [Serializable]
  public class SVector2 {
    public float X;
    public float Y;

    public SVector2(Vector2 vec2) {
      X = vec2.x;
      Y = vec2.y;
    }

    [JsonConstructor]
    public SVector2(float x, float y) {
      X = x;
      Y = y;
    }

    public static SVector2 FromVector2(Vector2 vec2) {
      return new SVector2(vec2);
    }

    public static Vector2 ToVector2(SVector2 sVec2) {
      return new Vector2(sVec2.X, sVec2.Y);
    }

    public static SVector2 Zero => new(0, 0);
    public static SVector2 One => new(1, 1);
    public static SVector2 OneX => new(1, 0);
    public static SVector2 OneY => new(0, 1);
  }
}