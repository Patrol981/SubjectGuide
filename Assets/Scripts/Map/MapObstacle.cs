using UnityEngine.AddressableAssets;
using UnityEngine;
using System;

namespace SubjectGuide.Map {
  public class MapObstacleData {
    public string AssetGuid { get; private set; } = String.Empty;
    public Vector3 Position { get; private set; } = Vector3.zero;
    public Quaternion Rotation { get; private set; } = Quaternion.identity;

    public MapObstacleData() { }

    public MapObstacleData(string assetGuid, Vector3 position, Quaternion rotation) {
      AssetGuid = assetGuid;
      Position = position;
      Rotation = rotation;
    }

    public void SetupAssetReferenceData(AssetReference assetReference, Transform spawnedGameObject) {
      AssetGuid = assetReference.AssetGUID;
      Position = spawnedGameObject.position;
      Rotation = spawnedGameObject.localRotation;
    }
  }

  public class MapObstacle : MonoBehaviour {
    public MapObstacleData ObstacleData { get; private set; } = new();
  }
}