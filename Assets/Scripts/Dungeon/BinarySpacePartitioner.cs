using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

// Divides a rectangle (the whole dungeon) into smaller rectangles using Binary Space Partitioning.
// Every split creates two child nodes, so the result is a tree.
public class BinarySpacePartitioner
{
    private RoomNode rootNode;
    public RoomNode RootNode { get => rootNode; }

    // Creates the root node, which covers the whole dungeon from (0, 0) to (dungeonWidth, dungeonLength)
    public BinarySpacePartitioner(int dungeonWidth, int dungeonLength)
    {
        this.rootNode = new RoomNode(new Vector2Int(0, 0), new Vector2Int(dungeonWidth, dungeonLength), null, 0);

    }

    // Splits the space until maxIterations is reached or no node is big enough to split.
    // Returns every node that was created, including the root.
    public List<RoomNode> PrepareNodesCollection(int maxIterations, int minRoomWidth, int minRoomLength)
    {
        Queue<RoomNode> graph = new();
        List<RoomNode> listToReturn = new();

        graph.Enqueue(this.rootNode);
        listToReturn.Add(this.rootNode);

        int iterations = 0;
        while (iterations < maxIterations && graph.Count > 0)
        {
            iterations++;

            // Take the oldest node first (breadth-first order)
            RoomNode currentNode = graph.Dequeue();
            // Only split nodes that are big enough to hold two rooms
            if (currentNode.Width >= minRoomWidth * 2 || currentNode.Length >= minRoomLength * 2)
            {
                SplitSpace(currentNode, listToReturn, minRoomWidth, minRoomLength, graph);
            }
        }

        return listToReturn;
    }

    // Splits currentNode in two along a line and adds the two new child nodes
    private void SplitSpace(RoomNode currentNode, List<RoomNode> listToReturn, int minRoomWidth, int minRoomLength, Queue<RoomNode> graph)
    {
        Line line = GetLineDividingSpace(currentNode.BottomLeftAreaCorner, currentNode.TopRightAreaCorner, minRoomWidth, minRoomLength);

        RoomNode node1, node2;
        // Horizontal line: node1 is the bottom part and node2 is the top part
        if (line.Orientation == Orientation.Horizontal){
            node1 = new RoomNode(currentNode.BottomLeftAreaCorner,
                new Vector2Int(currentNode.TopRightAreaCorner.x, line.Coordinates.y),
                currentNode,
                currentNode.TreeLayerIndex + 1);

            node2 = new RoomNode(new Vector2Int(currentNode.BottomLeftAreaCorner.x, line.Coordinates.y),
                currentNode.TopRightAreaCorner,
                currentNode,
                currentNode.TreeLayerIndex + 1);
        }
        // Vertical line: node1 is the left part and node2 is the right part
        else
        {
            node1 = new RoomNode(currentNode.BottomLeftAreaCorner,
                new Vector2Int(line.Coordinates.x, currentNode.TopRightAreaCorner.y),
                currentNode,
                currentNode.TreeLayerIndex + 1);

            node2 = new RoomNode(new Vector2Int(line.Coordinates.x, currentNode.BottomLeftAreaCorner.y),
                currentNode.TopRightAreaCorner,
                currentNode,
                currentNode.TreeLayerIndex + 1);
        }

        AddNewNodeToCollections(listToReturn, graph, node1);
        AddNewNodeToCollections(listToReturn, graph, node2);
    }

    // Stores the node in the full list and queues it so it can be split later
    private void AddNewNodeToCollections(List<RoomNode> listToReturn, Queue<RoomNode> graph, RoomNode node)
    {
        listToReturn.Add(node);
        graph.Enqueue(node);
    }

    // Chooses the direction of the split and the position of the line
    private Line GetLineDividingSpace(Vector2Int bottomLeftAreaCorner, Vector2Int topRightAreaCorner, int minRoomWidth, int minRoomLength)
    {
        Orientation orientation;
        bool lengthStatus = (topRightAreaCorner.y - bottomLeftAreaCorner.y) >= 2 * minRoomLength;
        bool widthStatus = (topRightAreaCorner.x - bottomLeftAreaCorner.x) >= 2 * minRoomWidth;

        // If both directions fit, pick one at random. Otherwise use the one that fits.
        if (lengthStatus && widthStatus)
            orientation = (Orientation)(Random.Range(0, 2));
        else if (widthStatus)
            orientation = Orientation.Vertical;
        else
            orientation = Orientation.Horizontal;

        return new Line(orientation, GetCoordinatesForOrientation(orientation, bottomLeftAreaCorner, topRightAreaCorner, minRoomWidth, minRoomLength));
    }

    // Picks a random position for the line, keeping at least the minimum room size on both sides
    private Vector2Int GetCoordinatesForOrientation(Orientation orientation, Vector2Int bottomLeftAreaCorner, Vector2Int topRightAreaCorner, int minRoomWidth, int minRoomLength)
    {
        Vector2Int coordinates = Vector2Int.zero;
        if (orientation == Orientation.Horizontal)
            coordinates = new Vector2Int(0, Random.Range(bottomLeftAreaCorner.y + minRoomLength, topRightAreaCorner.y - minRoomLength));
        else
            coordinates = new Vector2Int(Random.Range(bottomLeftAreaCorner.x + minRoomWidth, topRightAreaCorner.x - minRoomWidth), 0);

        return coordinates;
    }
}