using UnityEngine;

// A node of the space tree that represents a rectangular area of the dungeon.
public class RoomNode : Node
{
    // Width and length of the area, in grid units
    public int Width { get => (int)(TopRightAreaCorner.x - BottomLeftAreaCorner.x); }
    public int Length { get => (int)(TopRightAreaCorner.y - BottomLeftAreaCorner.y); }

    // Creates a node for the area between the two corners.
    // The other two corners are calculated from these.
    public RoomNode(Vector2Int bottomLeftAreaCorner, Vector2Int topRightAreaCorner, Node parentNode, int index) : base(parentNode)
    {
        this.BottomLeftAreaCorner = bottomLeftAreaCorner;
        this.TopRightAreaCorner = topRightAreaCorner;
        this.BottomRightAreaCorner = new Vector2Int(topRightAreaCorner.x, bottomLeftAreaCorner.y);
        this.TopLeftAreaCorner = new Vector2Int(bottomLeftAreaCorner.x, topRightAreaCorner.y);
        this.TreeLayerIndex = index;
    }
}