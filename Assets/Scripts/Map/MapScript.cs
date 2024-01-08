using System.Threading.Tasks;
using SubjectGuide.Managers;
using SubjectGuide.Utils;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace SubjectGuide.Map {
  public class MapScript : MonoBehaviour {
    [SerializeField] private MapData _mapData;
    [SerializeField] private Transform _floor;
    [SerializeField] private Transform _obstaclesParent;

    public async Task<Task> Setup() {
      await GenerateMap();
      return Task.CompletedTask;
    }

    private void LoadMap() {

    }

    private async Task<Task> GenerateMap() {
      _floor.localScale = new(_mapData.MapDimensions.x / 10, 1, _mapData.MapDimensions.y / 10);

      var bounds = _floor.GetComponent<Collider>().bounds;
      var min = new Vector2(bounds.min.x, bounds.min.z) * _floor.localScale;
      var max = new Vector2(bounds.max.x, bounds.max.z) * _floor.localScale;

      for (short i = 0; i < _mapData.ObstaclesCount; i++) {
        var indexToSpawn = RandomNumberGenerator.RandomInt(0, _mapData.Obstacles.Count - 1);
        var obstacleToSpawn = _mapData.Obstacles[indexToSpawn];

        var randomPosition = RandomNumberGenerator.GetRandomPosition(min, max);
        randomPosition.y = 0;
        var randomRotation = RandomNumberGenerator.GetRandomRotationY();

        // wait for object to spawn, kinda nasty but adressables aren't awaitable in a proper way
        var go = await obstacleToSpawn.InstantiateAsync(randomPosition, randomRotation, _obstaclesParent).Task;
        go.AddComponent<MapObstacle>().ObstacleData.SetupAssetReferenceData(obstacleToSpawn, go.transform);
      }

      return Task.CompletedTask;
    }

    public MapData MapData => _mapData;
    public Transform ObstaclesParent => _obstaclesParent;
  }
}