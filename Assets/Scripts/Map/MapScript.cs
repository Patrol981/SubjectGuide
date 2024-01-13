using System.Threading.Tasks;
using SubjectGuide.Managers;
using SubjectGuide.SaveSystem;
using SubjectGuide.Utils;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace SubjectGuide.Map {
  public class MapScript : MonoBehaviour {
    [SerializeField] private GameManager _gameManager;

    [SerializeField] private MapData _mapData;
    [SerializeField] private Transform _floor;
    [SerializeField] private Transform _obstaclesParent;

    private void Start() {
      _gameManager = FindObjectOfType<GameManager>();
    }

    public async Task<Task> Setup() {
      await GenerateMap();
      return Task.CompletedTask;
    }

    public void ClearMap() {
      var obstacles = _obstaclesParent.GetComponentsInChildren<MapObstacle>();
      foreach (var obstacle in obstacles) {
        Destroy(obstacle.gameObject);
      }
    }

    internal async Task<Task> LoadMap(MapSave saveData) {
      _mapData.MapDimensions = saveData.MapDimensions;
      _floor.localScale = new(_mapData.MapDimensions.X / 10, 1, _mapData.MapDimensions.Y / 10);

      foreach (var mapObstacle in saveData.MapObstacles) {
        var pos = SVector3.ToVector3(mapObstacle.Position);
        var rot = SVector3.ToVector3(mapObstacle.Rotation);

        var go = await Addressables.InstantiateAsync(
         mapObstacle.AssetGuid,
         pos,
         Quaternion.Euler(rot),
         _obstaclesParent
        ).Task;
        go.AddComponent<MapObstacle>().ObstacleData.SetupAssetReferenceData(mapObstacle.AssetGuid);
      }
      return Task.CompletedTask;
    }

    private async Task<Task> GenerateMap() {
      _floor.localScale = new(_mapData.MapDimensions.X / 10, 1, _mapData.MapDimensions.Y / 10);

      var bounds = _floor.GetComponent<Collider>().bounds;
      var maxRange = _gameManager.MapScript.MapData.SubjectsMaxRange;
      var min = new Vector2(bounds.min.x, bounds.min.z) * _floor.localScale;
      var max = new Vector2(bounds.max.x, bounds.max.z) * _floor.localScale;

      var maxRng = _mapData.Obstacles.Count;

      for (short i = 0; i < _mapData.ObstaclesCount; i++) {
        var indexToSpawn = RandomNumberGenerator.RandomInt(0, maxRng - 1);
        var obstacleToSpawn = _mapData.Obstacles[indexToSpawn];

        Vector3 randomPosition;
        do {
          randomPosition = RandomNumberGenerator.GetRandomPosition(min, max).ToVector3();
        } while (IsInRadius(randomPosition, maxRange));

        randomPosition.z = randomPosition.y;
        randomPosition.y = 0;
        var randomRotation = RandomNumberGenerator.GetRandomRotationY();

        // wait for object to spawn, kinda nasty but adressables aren't awaitable in a proper way
        var go = await obstacleToSpawn.InstantiateAsync(randomPosition, randomRotation, _obstaclesParent).Task;
        go.AddComponent<MapObstacle>().ObstacleData.SetupAssetReferenceData(obstacleToSpawn);
      }

      return Task.CompletedTask;
    }

    private bool IsInRadius(Vector2 point, float radius) {
      Vector2 exclusionCenter = new(0f, 0f);
      float distance = Vector2.Distance(point, exclusionCenter);
      return distance < radius;
    }

    public MapData MapData => _mapData;
    public Transform ObstaclesParent => _obstaclesParent;
  }
}