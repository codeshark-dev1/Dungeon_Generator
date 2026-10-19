using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

// Helper methods for reading the space tree and picking room corners.
public static class StructureHelper
{
    // Returns all leaf nodes (nodes without children) below the given parent,
    // using a breadth-first traversal of the graph.
    public static List<Node> TraverseGraphToExtractLowestLeaves(Node parentNode)
    {
        Queue<Node> nodesToCheck = new();
        List<Node> listToReturn = new();

        if (parentNode.ChildrenNodeList.Count == 0)
            return new List<Node>() { parentNode };

        // Start the traversal with the parent's direct children
        foreach (var child in parentNode.ChildrenNodeList)
            nodesToCheck.Enqueue(child);

        // Process nodes level by level until none are left
        while (nodesToCheck.Count > 0)
        {
            var currentNode = nodesToCheck.Dequeue();

            // No children: this is a leaf, so keep it
            if (currentNode.ChildrenNodeList.Count == 0) listToReturn.Add(currentNode);
            else
            {
                // Otherwise queue its children to be checked later
                foreach (var child in currentNode.ChildrenNodeList)
                    nodesToCheck.Enqueue(child);
            }
        }

        return listToReturn;
    }

    // Returns a random bottom-left corner inside the area.
    // The point is taken from the lower-left part of the area; pointModifier (0-1) sets how far it may go,
    // and offset keeps it away from the edges.
    public static Vector2Int GenerateBottomLeftCornerBetween(Vector2Int boundaryLeftPoint, Vector2Int boundaryRightPoint, float pointModifier, int offset)
    {
        int minX = boundaryLeftPoint.x + offset;
        int maxX = boundaryRightPoint.x - offset;
        int minY = boundaryLeftPoint.y + offset;
        int maxY = boundaryRightPoint.y - offset;

        return new Vector2Int(
            Random.Range(minX, (int)(minX + (maxX - minX) * pointModifier)),
            Random.Range(minY, (int)(minY + (maxY - minY) * pointModifier)));
    }

    // Returns a random top-right corner inside the area.
    // The point is taken from the upper-right part of the area; pointModifier (0-1) sets where that part starts,
    // and offset keeps it away from the edges.
    public static Vector2Int GenerateTopRightCornerBetween(Vector2Int boundaryLeftPoint, Vector2Int boundaryRightPoint, float pointModifier, int offset)
    {
        int minX = boundaryLeftPoint.x + offset;
        int maxX = boundaryRightPoint.x - offset;
        int minY = boundaryLeftPoint.y + offset;
        int maxY = boundaryRightPoint.y - offset;

        return new Vector2Int(
            Random.Range((int)(minX + (maxX - minX) * pointModifier), maxX),
            Random.Range((int)(minY + (maxY - minY) * pointModifier), maxY)
            );
    }

    public static Vector2Int CalculateMiddlePoint(Vector2Int v1, Vector2Int v2)
    {
        Vector2 sum = v1 + v2;
        Vector2 tempVector = sum / 2;
        return new Vector2Int((int)tempVector.x, (int)tempVector.y);
    }
}

public enum RelativePosition
{
    Up,
    Down,
    Left,
    Right
};