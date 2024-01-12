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
      _navGrid.GridWorldSize = new(mapData.MapDimensions.x, mapData.MapDimensions.y);

      await _mapScript.Setup();
      await _navGrid.Init();
      await _subjectManager.Init();
      await _canvasController.CreateUIHooks(_subjectManager.Subjects);
    }

    public async Task<Task> LoadSave(MapSave save) {
      _subjectManager.ClearSubjects();
      _canvasController.ClearHooks();
      _mapScript.ClearMap();

      await _mapScript.LoadMap(save);
      await _subjectManager.LoadSubjectData(save.SubjectsInfo.ToArray());
      await _canvasController.CreateUIHooks(_subjectManager.Subjects);

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