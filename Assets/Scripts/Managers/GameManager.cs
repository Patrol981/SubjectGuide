using SubjectGuide.Pathfinding;
using UnityEngine;

namespace SubjectGuide.Managers {
  public class GameManager : MonoBehaviour {
    [SerializeField] private NavGrid _navGrid;
    [SerializeField] private Transform _player;

    public NavGrid NavGrid => _navGrid;
    public Transform Player => _player;
  }
}