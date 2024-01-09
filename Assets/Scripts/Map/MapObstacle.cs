using UnityEngine.AddressableAssets;
using UnityEngine;
using System;
using SubjectGuide.Utils;

namespace SubjectGuide.Map {
  [Serializable]
  public class MapObstacleData {
    public string AssetGuid = String.Empty;
    // use custom vector3 to optimize save file, since unity vec3 and quat are redundant,
    // therefore takes more space
    public SVector3 Position = SVector3.Zero;
    public SVector3 Rotation = SVector3.Zero;

    public MapObstacleData() { }

    public MapObstacleData(string assetGuid, SVector3 position, SVector3 rotation) {
      AssetGuid = assetGuid;
      Position = position;
      Rotation = rotation;
    }

    public void SetupAssetReferenceData(AssetReference assetReference) {
      AssetGuid = assetReference.AssetGUID;
    }
  }

  public class MapObstacle : MonoBehaviour {
    public MapObstacleData ObstacleData { get; private set; } = new();
  }
}