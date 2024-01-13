using System.Threading.Tasks;
using SubjectGuide.Map;
using SubjectGuide.Pathfinding;
using SubjectGuide.SaveSystem;
using SubjectGuide.UI;
using UnityEngine;

namespace SubjectGuide.Managers {
  public class GameManager : MonoBehaviour {
    [SerializeField] private NavGrid _navGrid;
    [SerializeField] private MapScript _mapScript;
    [SerializeField] private SaveScript _saveScript;
    [SerializeField] private SubjectManager _subjectManager;
    [SerializeField] private CanvasController _canvasController;

    private void Start() {
      Setup();
    }

    private async void Setup() {
      var mapData = _mapScript.MapData;
      _navGrid.GridWorldSize = new(mapData.MapDimensions.X, mapData.MapDimensions.Y);

      await _mapScript.Setup();
      await _navGrid.Init();
      await _subjectManager.Init();
      await _canvasController.CreateUIHooks(_subjectManager.Subjects);
    }

    public async Task<Task> LoadSave(MapSave save) {
      // before we can start load save file, we need to remove all the previous assets from the game
      // it would be faster if some of them would be reused, but it's cleaner this way
      _subjectManager.ClearSubjects();
      _canvasController.ClearHooks();
      _mapScript.ClearMap();

      // in order to make sure all the systems will load in sequence
      // I did change some standard void calls into awaitable Task,
      // while still being syncronous
      await _mapScript.LoadMap(save);
      await _subjectManager.LoadSubjectData(save.SubjectsInfo.ToArray());
      await _canvasController.CreateUIHooks(_subjectManager.Subjects);

      // after all systems are reloaded, now's the time for a* to reload itself
      _navGrid.GridWorldSize = new(save.MapDimensions.X, save.MapDimensions.Y);
      await _navGrid.Init();
      return Task.CompletedTask;
    }

    public NavGrid NavGrid => _navGrid;
    public MapScript MapScript => _mapScript;
    public SaveScript SaveScript => _saveScript;
    public SubjectManager SubjectManager => _subjectManager;
    public CanvasController CanvasController => _canvasController;
    public Transform Player => _subjectManager.Guide.Transform;
  }
}