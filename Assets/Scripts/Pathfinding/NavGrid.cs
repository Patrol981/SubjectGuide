using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace SubjectGuide.Pathfinding {
  public class NavGrid : MonoBehaviour {
    [SerializeField] private GridSettings _gridSettings;
    [SerializeField] private Transform _floor;
    [SerializeField] private LayerMask _wallMask;
    [SerializeField] private Vector2 _gridWorldSize;
    private float _nodeRadius;

    [SerializeField] Pathfinder _pathfinder;

    private Node[,] _nodes;
    private List<Node> _finalPath;

    private float _nodeDiameter;
    private int _gridSizeX;
    private int _gridSizeY;

    private bool _moving = false;
    private float _speed = 5.0f;

    private void Awake() {
      _pathfinder = FindObjectOfType<Pathfinder>();
    }

    public Task Init() {
      _nodeRadius = _gridSettings.NodeRadius;

      _nodeDiameter = _nodeRadius * 2;
      _gridSizeX = Mathf.RoundToInt(_gridWorldSize.x / _nodeDiameter);
      _gridSizeY = Mathf.RoundToInt(_gridWorldSize.y / _nodeDiameter);
      CreateNodes();
      return Task.CompletedTask;
    }

    public async void MoveActor(Transform actor, Vector3 destination) {
      if (!Validate()) return;
      if (actor == null) return;
      _moving = true;
      CalculatePath(actor.position, destination);
      await MoveObject(actor);
      _moving = false;
    }

    public async void MoveActors(Transform[] actors, Vector3 destination, bool waitForEachOther = false) {
      if (!Validate()) return;
      if (actors.Length < 1) return;
      _moving = true;

      CalculatePath(actors[0].position, destination);
      if (!waitForEachOther) {
        var tasks = new Task[actors.Length];
        for (short i = 0; i < actors.Length; i++) {
          tasks[i] = MoveObject(actors[i], i);
        }
        await Task.WhenAll(tasks);
      } else {
        for (short i = 0; i < actors.Length; i++) {
          await MoveObject(actors[i], i);
        }
      }

      _moving = false;
    }

    private bool Validate() {
      if (_finalPath != null && _finalPath.Count < 1) return false;
      if (_moving) return false;

      return true;
    }

    private void CalculatePath(Vector3 start, Vector3 end) {
      _pathfinder.FindPath(start, end);
    }

    private async Task MoveObject(Transform target, int index = 0) {
      for (short i = 0; i < _finalPath.Count - index; i++) {
        var targetNode = _finalPath[i];

        var lookDir = target.position - targetNode.Position;
        lookDir.y = 0;
        target.rotation = Quaternion.LookRotation(-lookDir);

        await MoveToPosition(target, targetNode.Position, _speed);
      }
    }

    private async Task MoveToPosition(Transform target, Vector3 destination, float speed) {
      while (Vector3.Distance(target.position, destination) > 0.1f) {
        target.position = Vector3.MoveTowards(target.position, destination, speed * Time.deltaTime);
        await Task.Yield();
      }
    }

    internal Node NodeFromWorldPosition(Vector3 worldPosition) {
      var xPoint = (worldPosition.x + _gridWorldSize.x / 2) / _gridWorldSize.x;
      var yPoint = (worldPosition.z + _gridWorldSize.y / 2) / _gridWorldSize.y;

      xPoint = Mathf.Clamp01(xPoint);
      yPoint = Mathf.Clamp01(yPoint);

      var x = Mathf.RoundToInt((_gridSizeX - 1) * xPoint);
      var y = Mathf.RoundToInt((_gridSizeY - 1) * yPoint);

      return _nodes[x, y];
    }

    internal ReadOnlySpan<Node> GetNeighbouringNodes(Node node) {
      var neighbouringNodes = new List<Node>();
      for (int x = -1; x <= 1; x++) {
        for (int y = -1; y <= 1; y++) {
          if (x == 0 && y == 0) {
            continue;
          }

          var checkX = node.GridX + x;
          var checkY = node.GridY + y;

          if (checkX >= 0 && checkX < _gridSizeX && checkY >= 0 && checkY < _gridSizeY) {
            neighbouringNodes.Add(_nodes[checkX, checkY]);
          }
        }
      }

      return neighbouringNodes.ToArray();
    }

    private void CreateNodes() {
      _nodes = new Node[_gridSizeX, _gridSizeY];
      var bottomLeft = _floor.position - Vector3.right * _gridWorldSize.x / 2 - Vector3.forward * _gridWorldSize.y / 2;
      for (short x = 0; x < _gridSizeX; x++) {
        for (short y = 0; y < _gridSizeY; y++) {
          var worldPoint = bottomLeft + Vector3.right * (x * _nodeDiameter + _nodeRadius) + Vector3.forward * (y * _nodeDiameter + _nodeRadius);
          var isWall = true;

          if (Physics.CheckSphere(worldPoint, _nodeRadius, _wallMask)) isWall = false;

          _nodes[x, y] = new Node(isWall, worldPoint, x, y);
        }
      }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos() {
      Gizmos.DrawWireCube(_floor.position, new(_gridWorldSize.x, 1, _gridWorldSize.y));
      if (_nodes != null) {
        foreach (var node in _nodes) {
          if (node.IsWall) Gizmos.color = Color.white;
          else Gizmos.color = Color.red;

          if (_finalPath != null) {
            if (_finalPath.Contains(node)) {
              Gizmos.color = Color.green;
            }
          }

          Gizmos.DrawCube(node.Position, Vector3.one * _nodeDiameter);
        }
      }
    }
#endif

    public List<Node> FinalPath {
      get { return _finalPath; }
      set { _finalPath = value; }
    }

    public Vector2 GridWorldSize {
      get { return _gridWorldSize; }
      set { _gridWorldSize = value; }
    }

    public bool Busy => _moving;
    public LayerMask WallMask => _wallMask;
  }
}