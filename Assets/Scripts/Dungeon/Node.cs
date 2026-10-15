using System.Collections.Generic;
using UnityEngine;

// Base class for every node of the space tree.
// A node knows its parent, its children and the corners of its area.
public abstract class Node
{
    // Nodes created when this node was split
    private List<Node> childrenNodeList;

    public List<Node> ChildrenNodeList { get => childrenNodeList; }

    // Marks whether this node was visited during a traversal (not used yet)
    public bool HasVisited { get; set; }
    // Corners of the rectangular area this node covers
    public Vector2Int BottomLeftAreaCorner { get; set; }
    public Vector2Int BottomRightAreaCorner { get; set; }
    public Vector2Int TopRightAreaCorner { get; set; }
    public Vector2Int TopLeftAreaCorner { get; set; }

    // The node this one was split from (null for the root)
    public Node Parent { get; set; }

    // How deep this node is in the tree (the root is 0)
    public int TreeLayerIndex { get; set; }

    // Creates the node and adds it as a child of the parent, if there is one
    public Node(Node parentNode)
    {
        childrenNodeList = new();
        this.Parent = parentNode;
        if (parentNode != null)
        {
            parentNode.AddChild(this);
        }
    }

    // Adds a child node
    public void AddChild(Node node)
    {
        childrenNodeList.Add(node);
    }

    // Removes a child node
    public void RemoveChild(Node node)
    {
        childrenNodeList.Remove(node);
    }
}