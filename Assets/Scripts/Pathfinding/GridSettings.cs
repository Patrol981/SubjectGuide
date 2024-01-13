using UnityEngine;

namespace SubjectGuide.Pathfinding {
  [CreateAssetMenu(fileName = "GridSettings", menuName = "GridSettings")]
  public class GridSettings : ScriptableObject {
    [Range(0.1f, 1.0f)] public float NodeRadius = 0.5f;
  }
}