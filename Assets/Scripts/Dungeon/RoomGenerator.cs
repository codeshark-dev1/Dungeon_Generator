using System.Collections.Generic;
using UnityEngine;

// Places one random room inside each space that the partitioner produced.
public class RoomGenerator
{
    // Settings for the rooms (maxIterations is stored but not used yet)
    private int maxIterations;
    private int minRoomWidth;
    private int minRoomLength;

    // Stores the settings
    public RoomGenerator(int maxIterations, int minRoomWidth, int minRoomLength)
    {
        this.maxIterations = maxIterations;
        this.minRoomWidth = minRoomWidth;
        this.minRoomLength = minRoomLength;
    }

    // Places a room inside each given space and returns the rooms.
    // Each room is a random rectangle that stays away from the edges of its space.
    public List<RoomNode> GenerateRoomsInGivenSpaces(List<Node> roomSpaces, float roomBottomCornerModifier, float roomTopCornerModifier, int roomOffset)
    {
        // The rooms that will be returned
        List<RoomNode> listToReturn = new();
        foreach (var space in roomSpaces)
        {
            // Random bottom-left corner in the lower-left part of the space
            Vector2Int newBottomLeftPoint = StructureHelper.GenerateBottomLeftCornerBetween(
                space.BottomLeftAreaCorner, space.TopRightAreaCorner, roomBottomCornerModifier, roomOffset
                );

            // Random top-right corner in the upper-right part of the space
            Vector2Int newTopRightPoint = StructureHelper.GenerateTopRightCornerBetween(
                space.BottomLeftAreaCorner, space.TopRightAreaCorner, roomTopCornerModifier, roomOffset
                );

            // Replace the space's corners with the room's corners, and update the other two
            space.BottomLeftAreaCorner = newBottomLeftPoint;
            space.TopRightAreaCorner = newTopRightPoint;
            space.BottomRightAreaCorner = new Vector2Int(newTopRightPoint.x, newBottomLeftPoint.y);
            space.TopLeftAreaCorner = new Vector2Int(newBottomLeftPoint.x , newTopRightPoint.y);

            // Every leaf was created as a RoomNode, so this cast is safe
            listToReturn.Add((RoomNode)space);
        }

        return listToReturn;
    }
}