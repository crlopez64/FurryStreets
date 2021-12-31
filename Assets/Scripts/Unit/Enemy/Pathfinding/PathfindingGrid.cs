using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingGrid : MonoBehaviour
{
    private PathNode[,] grid;
    private float nodeDiameter;
    private int gridSizeX;
    private int gridSizeY;

    public bool displayGizmos;
    public LayerMask unwalkableMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    public Transform player;

    private void Awake()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector2(gridWorldSize.x, gridWorldSize.y));

        if (grid != null && displayGizmos)
        {
            foreach (PathNode node in grid)
            {
                Gizmos.color = node.Walkable() ? new Color(1, 1, 1, 0.4f) : new Color(1, 0, 0, 0.4f);
                Gizmos.DrawCube(node.WorldPosition(), Vector2.one * (nodeDiameter - 0.1f));
            }
        }
    }

    public int MaxSize()
    {
        return gridSizeX * gridSizeY;
    }
    public PathNode NodeFromWorldPoint(Vector2 worldPosition)
    {
        float percentX = (worldPosition.x / gridWorldSize.x) + 0.5f;
        float percentY = (worldPosition.y / gridWorldSize.y) + 0.5f;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);
        int x = Mathf.FloorToInt(Mathf.Clamp(gridSizeX * percentX, 0, gridSizeX - 1));
        int y = Mathf.FloorToInt(Mathf.Clamp(gridSizeY * percentY, 0, gridSizeY - 1));
        return grid[x, y];
    }
    public PathNode[,] Grid()
    {
        return grid;
    }
    public List<PathNode> GetNeighbors(PathNode node)
    {
        List<PathNode> neighbors = new List<PathNode>();
        for(int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if ((x == 0) && (y == 0))
                {
                    continue;
                }
                int checkX = node.GridX() + x;
                int checkY = node.GridY() + y;
                if ((checkX >= 0) && (checkX < gridSizeX) && (checkY >= 0) && (checkY < gridSizeY))
                {
                    neighbors.Add(grid[checkX, checkY]);
                }
            }
        }
        neighbors.TrimExcess();
        return neighbors;
    }

    private void CreateGrid()
    {
        grid = new PathNode[gridSizeX, gridSizeY];
        Vector3 worldBottomLeft = (transform.position) - Vector3.right * gridWorldSize.x / 2 - Vector3.up * gridWorldSize.y / 2;

        for(int x = 0; x < gridSizeX; x++)
        {
            for(int y = 0; y < gridSizeY; y++)
            {
                Vector2 worldPoint = (Vector2)(worldBottomLeft +
                    Vector3.right * (x * nodeDiameter + nodeRadius) +
                    Vector3.up * (y * nodeDiameter + nodeRadius));
                bool walkable = !(Physics2D.OverlapCircle(worldPoint, nodeRadius, unwalkableMask));
                grid[x, y] = new PathNode(walkable, worldPoint, x, y);
            }
        }
    }
}