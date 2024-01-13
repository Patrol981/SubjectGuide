using SubjectGuide.Globals;
using SubjectGuide.Managers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SubjectGuide.Player {
  public class MouseController : MonoBehaviour {
    private static Vector3 _mousePosition = Vector3.zero;
    private static Vector3 _worldPoint = Vector3.zero;
    [SerializeField] private LayerMask _playerMask;

    private Transform _camera;
    private GameManager _gameManager;

    private void Awake() {
      _camera = Camera.main.transform;
      _gameManager = FindObjectOfType<GameManager>();
    }

    private void Update() {
      RayMouse();
      if (Input.GetMouseButtonDown(1) && !MouseOverUI()) {
        if (MouseOverWall()) { Debug.Log("mouse over wall"); return; }
        _gameManager.NavGrid.MoveActors(_gameManager.SubjectManager.GatherSubjects(), _worldPoint);
      }
      if (Input.GetMouseButtonDown(0) && !MouseOverUI()) {
        var ray = _camera.GetComponent<Camera>().ScreenPointToRay(_mousePosition);
        var result = Physics.Raycast(ray.origin, ray.direction, out var hit, _playerMask);
        if (!result) return;
        hit.transform.gameObject.TryGetComponent<ISubject>(out var subject);
        if (subject == null) return;
        _gameManager.CanvasController.ClickButton(subject.SubjectId.ToString());
        _gameManager.SubjectManager.SetGuide(subject);
      }
    }

    private void RayMouse() {
      _mousePosition = Input.mousePosition;
      var ray = _camera.GetComponent<Camera>().ScreenPointToRay(_mousePosition);
      var result = Physics.Raycast(ray.origin, ray.direction, out var hit);
      if (result) {
        _worldPoint = hit.point;
      }
    }

    public static bool MouseOverUI() {
      return EventSystem.current.IsPointerOverGameObject();
    }

    public bool MouseOverWall() {
      var ray = _camera.GetComponent<Camera>().ScreenPointToRay(_mousePosition);
      var layer = LayerMask.NameToLayer(GameConstants.NavWall);
      Physics.Raycast(ray.origin, ray.direction, out var hit);
      if (hit.transform.gameObject.layer == layer) return true;
      return false;
    }

    public static Vector3 WorldPoint { get { return _worldPoint; } }
    public static Vector3 MousePoint { get { return _mousePosition; } }
  }
}