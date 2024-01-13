using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.AddressableAssets;
using SubjectGuide.Utils;

namespace SubjectGuide.Map {

  [CreateAssetMenu(fileName = "MapData", menuName = "MapData")]
  public class MapData : ScriptableObject {
    public SVector2 MapDimensions = new(30, 30);
    public List<AssetReference> Obstacles = new();
    [Range(1, 40)] public int ObstaclesCount = 1;
    [Range(4, 7)] public int SubjectsMaxRange = 4;
  }
}