using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace SubjectGuide.Map {

  [CreateAssetMenu(fileName = "MapData", menuName = "MapData")]
  public class MapData : ScriptableObject {
    public Vector2 MapDimensions = new(30, 30);
    public List<AssetReference> Obstacles = new();
    [Range(1, 40)] public int ObstaclesCount = 1;
  }
}