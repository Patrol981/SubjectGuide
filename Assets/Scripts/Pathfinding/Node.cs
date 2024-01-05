using UnityEngine;

namespace SubjectGuide.Pathfinding {
  public class Node {
    private int _gridX;
    private int _gridY;
    private bool _isWall;
    private Vector3 _position;
    private Node _parent;
    private int _gCost; // cost of moving to next square
    private int _hCost; // distance to target from node

    public Node(bool isWall, Vector3 position, int gridX, int gridY) {
      _isWall = isWall;
      _position = position;
      _gridX = gridX;
      _gridY = gridY;
    }

    public int Cost {
      get {
        return _gCost + _hCost;
      }
    }
    public int HCost {
      get { return _hCost; }
      internal set { _hCost = value; }
    }
    public int GCost {
      get { return _gCost; }
      internal set { _gCost = value; }
    }

    public bool IsWall => _isWall;
    public Vector3 Position => _position;
    public Node Parent {
      get { return _parent; }
      internal set { _parent = value; }
    }
    public int GridX => _gridX;
    public int GridY => _gridY;
  }
}