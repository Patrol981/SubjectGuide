using System.Threading.Tasks;
using SubjectGuide.Map;
using SubjectGuide.Pathfinding;
using UnityEngine;

namespace SubjectGuide.Managers {
  public class GameManager : MonoBehaviour {
    [SerializeField] private NavGrid _navGrid;
    [SerializeField] private MapScript _mapScript;
    [SerializeField] private Transform _player;

    private void Start() {
      Setup();
    }

    private async void Setup() {
      var mapData = _mapScript.MapData;
      _navGrid.GridWorldSize = new(mapData.MapDimensions.x, mapData.MapDimensions.y);

      await _mapScript.Setup();
      await _navGrid.Init();
    }

    public NavGrid NavGrid => _navGrid;
    public Transform Player => _player;
  }
}