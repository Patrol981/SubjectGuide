using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System.Linq;

namespace SubjectGuide.Pathfinding {
  public class NavGrid : MonoBehaviour {
    [SerializeField] private Transform _floor;
    [SerializeField] private LayerMask _wallMask;
    [SerializeField] private Vector2 _gridWorldSize;
    [SerializeField] private float _nodeRadius;
    [SerializeField] private float _distance;

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
      _nodeDiameter = _nodeRadius * 2;
      _gridSizeX = Mathf.RoundToInt(_gridWorldSize.x / _nodeDiameter);
      _gridSizeY = Mathf.RoundToInt(_gridWorldSize.y / _nodeDiameter);
      CreateNodes();
      return Task.CompletedTask;
    }

    public async void MoveActor(Transform actor, Vector3 destination) {
      if (actor == null) return;
      if (_moving) return;
      _moving = true;
      CalculatePath(actor.position, destination);
      // actor.rotation = Quaternion.Euler(lookDir.x, 0, lookDir.z);
      await MoveObject(actor);
      _moving = false;
    }

    private void CalculatePath(Vector3 start, Vector3 end) {
      _pathfinder.FindPath(start, end);
    }

    private async Task MoveObject(Transform target) {
      for (short i = 0; i < _finalPath.Count; i++) {
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
      var xCheck = 0;
      var yCheck = 0;
      {
        // Right Side
        xCheck = node.GridX + 1;
        yCheck = node.GridY;
        if (xCheck >= 0 && xCheck < _gridSizeX) {
          if (yCheck >= 0 && yCheck < _gridSizeY) {
            neighbouringNodes.Add(_nodes[xCheck, yCheck]);
          }
        }
      }

      {
        // Left Side
        xCheck = node.GridX - 1;
        yCheck = node.GridY;
        if (xCheck >= 0 && xCheck < _gridSizeX) {
          if (yCheck >= 0 && yCheck < _gridSizeY) {
            neighbouringNodes.Add(_nodes[xCheck, yCheck]);
          }
        }
      }

      {
        // Top Side
        xCheck = node.GridX;
        yCheck = node.GridY + 1;
        if (xCheck >= 0 && xCheck < _gridSizeX) {
          if (yCheck >= 0 && yCheck < _gridSizeY) {
            neighbouringNodes.Add(_nodes[xCheck, yCheck]);
          }
        }
      }

      {
        // Bottom Side
        xCheck = node.GridX;
        yCheck = node.GridY - 1;
        if (xCheck >= 0 && xCheck < _gridSizeX) {
          if (yCheck >= 0 && yCheck < _gridSizeY) {
            neighbouringNodes.Add(_nodes[xCheck, yCheck]);
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

    // Debug Draw
#if DEBUG
    private void OnDrawGizmos() {
      Gizmos.DrawWireCube(_floor.position, new(_gridWorldSize.x, 1, _gridWorldSize.y));
      if (_nodes != null) {
        foreach (var node in _nodes) {
          if (node.IsWall) Gizmos.color = Color.white;
          else Gizmos.color = Color.yellow;

          if (_finalPath != null) {
            if (_finalPath.Contains(node)) {
              Gizmos.color = Color.red;
            }
          }

          Gizmos.DrawCube(node.Position, Vector3.one * (_nodeDiameter - _distance));
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
  }
}