using System;
using System.Collections.Generic;
using UnityEngine;

public class BinarySpacePartitioner
{
    private RoomNode rootNode;
    public RoomNode RootNode { get => rootNode; }

    public BinarySpacePartitioner(int dungeonWidth, int dungeonLength)
    {
        this.rootNode = new RoomNode(new Vector2Int(0, 0), new Vector2Int(dungeonWidth, dungeonLength), null, 0);

    }

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

            RoomNode currentNode = graph.Dequeue();
            if (currentNode.Width >= minRoomWidth * 2 || currentNode.Length >= minRoomLength * 2)
            {
                SplitSpace(currentNode, listToReturn, minRoomWidth, minRoomLength, graph);
            }
        }
    }

    private void SplitSpace(RoomNode currentNode, List<RoomNode> listToReturn, int minRoomWidth, int minRoomLength, Queue<RoomNode> graph)
    {
        Line line = GetLineDividingSpace(currentNode.BottomLeftAreaCorner, currentNode.TopRightAreaCorner, minRoomWidth, minRoomLength);

    }

    private Line GetLineDividingSpace(Vector2Int bottomLeftAreaCorner, Vector2Int topRightAreaCorner, int minRoomWidth, int minRoomLength)
    {
        throw new NotImplementedException();
    }
}