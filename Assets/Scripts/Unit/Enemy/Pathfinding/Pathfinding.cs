using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    private PathfindingGrid grid;
    private PathRequestManager requestManager;

    private void Awake()
    {
        requestManager = GetComponent<PathRequestManager>();
        grid = GetComponent<PathfindingGrid>();
    }

    public void StartFindPath(Vector2 startPosition, Vector2 targetPosition)
    {
        StartCoroutine(FindPath(startPosition, targetPosition));
    }

    private IEnumerator FindPath(Vector2 startPosition, Vector2 targetPosition)
    {
        Vector2[] wayPoints = new Vector2[0];
        bool pathSuccess = false;
        PathNode startNode = grid.NodeFromWorldPoint(startPosition);
        PathNode targetNode = grid.NodeFromWorldPoint(targetPosition);

        if (startNode.Walkable() && targetNode.Walkable())
        {
            Heap<PathNode> openSet = new Heap<PathNode>(grid.MaxSize());
            HashSet<PathNode> closedSet = new HashSet<PathNode>();

            openSet.Add(startNode);
            while (openSet.Count() > 0)
            {
                PathNode currentNode = openSet.RemoveFirst();
                closedSet.Add(currentNode);
                if (currentNode == targetNode)
                {
                    pathSuccess = true;
                    break;
                }
                foreach (PathNode neighbor in grid.GetNeighbors(currentNode))
                {
                    if ((!neighbor.Walkable()) || closedSet.Contains(neighbor))
                    {
                        continue;
                    }
                    int newMovementCostToNeighbor = currentNode.GCost() + GetDistance(currentNode, neighbor);
                    if ((newMovementCostToNeighbor < neighbor.GCost()) || (!openSet.Contains(neighbor)))
                    {
                        neighbor.SetGCost(newMovementCostToNeighbor);
                        neighbor.SetHCost(GetDistance(neighbor, targetNode));
                        neighbor.SetParent(currentNode);
                        if (!openSet.Contains(neighbor))
                        {
                            openSet.Add(neighbor);
                        }
                    }
                }
            }
        }
        
        yield return null;
        if (pathSuccess)
        {
            wayPoints = RetracePath(startNode, targetNode);
        }
        requestManager.FinishedProcessingPath(wayPoints, pathSuccess);
    }
    private Vector2[] RetracePath(PathNode startNode, PathNode endNode)
    {
        List<PathNode> path = new List<PathNode>();
        PathNode currentNode = endNode;
        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.GetParent();
        }
        Vector2[] waypoints = SimplyPath(path);
        Array.Reverse(waypoints);
        return waypoints;
    }
    private Vector2[] SimplyPath(List<PathNode> path)
    {
        List<Vector2> waypoints = new List<Vector2>();
        Vector2 directionOld = Vector2.zero;
        for(int i = 1; i < path.Count; i++)
        {
            Vector2 directionNew = new Vector2(path[i - 1].GridX() - path[i].GridX(), path[i - 1].GridY() - path[i].GridY());
            if (directionNew != directionOld)
            {
                waypoints.Add(path[i].WorldPosition());
            }
            directionOld = directionNew;
        }
        return waypoints.ToArray();
    }
    private int GetDistance(PathNode nodeA, PathNode nodeB)
    {
        int distanceX = Mathf.Abs(nodeA.GridX() - nodeB.GridX());
        int distanceY = Mathf.Abs(nodeA.GridY() - nodeB.GridY());

        if(distanceX > distanceY)
        {
            return 14 * distanceY + 10 * (distanceX - distanceY);
        }
        return 14 * distanceX + 10 * (distanceY - distanceX);
    }
}
