using SubjectGuide.Managers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SubjectGuide.Player {
  public class MouseController : MonoBehaviour {
    private static Vector3 _mousePosition = Vector3.zero;
    private static Vector3 _worldPoint = Vector3.zero;

    private Transform _camera;
    private GameManager _gameManager;

    private void Awake() {
      _camera = Camera.main.transform;
      _gameManager = FindObjectOfType<GameManager>();
    }

    private void Update() {
      RayMouse();
      if (Input.GetMouseButtonDown(0) && !MouseOverUI()) {
        _gameManager.NavGrid.MoveActor(_gameManager.Player, _worldPoint);
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

    public static Vector3 WorldPoint { get { return _worldPoint; } }
    public static Vector3 MousePoint { get { return _mousePosition; } }
  }
}