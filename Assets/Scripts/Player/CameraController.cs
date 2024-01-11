using UnityEngine;

namespace SubjectGuide.Player {
  public class CameraController : MonoBehaviour {
    [SerializeField] private Transform _camera;
    [SerializeField] private float _speed = 5.0f;

    private void Update() {
      var horizontal = Input.GetAxis("Horizontal");
      var vertical = Input.GetAxis("Vertical");

      var translation = new Vector3(horizontal, vertical, 0);

      _camera.Translate(translation * _speed * Time.deltaTime);
    }
  }
}