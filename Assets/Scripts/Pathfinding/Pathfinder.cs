using System.Collections.Generic;
using UnityEngine;

namespace SubjectGuide.Pathfinding {
  public class Pathfinder : MonoBehaviour {
    [SerializeField] private NavGrid _navGrid;

    private void Awake() {
      _navGrid = FindObjectOfType<NavGrid>();
    }

    internal void FindPath(Vector3 start, Vector3 end) {
      var startNode = _navGrid.NodeFromWorldPosition(start);
      var endNode = _navGrid.NodeFromWorldPosition(end);

      var openList = new List<Node>();
      var closedList = new HashSet<Node>();

      openList.Add(startNode);
      while (openList.Count > 0) {
        var currentNode = openList[0];
        for (short i = 1; i < openList.Count; i++) {
          if (openList[i].Cost < currentNode.Cost ||
             openList[i].Cost == currentNode.Cost &&
             openList[i].HCost < currentNode.HCost
          ) {
            currentNode = openList[i];
          }
        }
        openList.Remove(currentNode);
        closedList.Add(currentNode);

        if (currentNode == endNode) {
          GetFinalPath(startNode, endNode);
        }

        foreach (var neighbourNode in _navGrid.GetNeighbouringNodes(currentNode)) {
          if (!neighbourNode.IsWall || closedList.Contains(neighbourNode)) continue;
          var moveCost = currentNode.GCost + GetManhattenDistance(currentNode, neighbourNode);

          if (moveCost < neighbourNode.GCost || !openList.Contains(neighbourNode)) {
            neighbourNode.GCost = moveCost;
            neighbourNode.HCost = GetManhattenDistance(neighbourNode, endNode);
            neighbourNode.Parent = currentNode;

            if (!openList.Contains(neighbourNode)) {
              openList.Add(neighbourNode);
            }
          }
        }
      }
    }

    private void GetFinalPath(Node startNode, Node endNode) {
      var finalPath = new List<Node>();
      var currentNode = endNode;

      while (currentNode != startNode) {
        finalPath.Add(currentNode);
        currentNode = currentNode.Parent;
      }

      finalPath.Reverse();
      _navGrid.FinalPath = finalPath;
    }

    private int GetManhattenDistance(Node nodeA, Node nodeB) {
      var x = Mathf.Abs(nodeA.GridX - nodeB.GridX);
      var y = Mathf.Abs(nodeA.GridY - nodeB.GridY);

      return x + y;
    }
  }
}