using UnityEngine;

public class PathNode : IHeapItem<PathNode>
{
    private PathNode parent;
    private readonly bool walkable;
    private int gCost;
    private int hCost;
    private readonly int gridX;
    private readonly int gridY;
    private int heapIndex;
    private Vector2 worldPosition;

    public PathNode(bool walkable, Vector2 worldPosition, int gridX, int gridY)
    {
        this.walkable = walkable;
        this.worldPosition = worldPosition;
        this.gridX = gridX;
        this.gridY = gridY;
    }

    public void SetParent(PathNode parent)
    {
        this.parent = parent;
    }
    public void SetGCost(int cost)
    {
        gCost = cost;
    }
    public void SetHCost(int cost)
    {
        hCost = cost;
    }
    public bool Walkable()
    {
        return walkable;
    }
    /// <summary>
    /// The distance from the Starting Node.
    /// </summary>
    /// <returns></returns>
    public int GCost()
    {
        return gCost;
    }
    /// <summary>
    /// Distance from the End Node.
    /// </summary>
    /// <returns></returns>
    public int HCost()
    {
        return hCost;
    }
    /// <summary>
    /// G cost and H cost added together.
    /// </summary>
    /// <returns></returns>
    public int FCost()
    {
        return gCost + hCost;
    }
    /// <summary>
    /// Return the X point in the grid.
    /// </summary>
    /// <returns></returns>
    public int GridX()
    {
        return gridX;
    }
    /// <summary>
    /// Return the Y point in the grid.
    /// </summary>
    /// <returns></returns>
    public int GridY()
    {
        return gridY;
    }
    public int HeapIndex
    {
        get
        {
            return heapIndex;
        }
        set
        {
            heapIndex = value;
        }
    }
    public int CompareTo(PathNode nodeToCompare)
    {
        int compare = FCost().CompareTo(nodeToCompare.FCost());
        if (compare == 0)
        {
            compare = HCost().CompareTo(nodeToCompare.HCost());
        }
        return -compare;
    }
    /// <summary>
    /// Get the world position the Unit can reference.
    /// </summary>
    /// <returns></returns>
    public Vector2 WorldPosition()
    {
        return worldPosition;
    }
    /// <summary>
    /// Get the parent node to this node.
    /// </summary>
    /// <returns></returns>
    public PathNode GetParent()
    {
        return parent;
    }
}
