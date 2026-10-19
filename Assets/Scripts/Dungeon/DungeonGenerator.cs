using System.Collections.Generic;
using System.Linq;

// Creates the rooms of a dungeon in three steps:
// split the space, collect the final areas, and place a room inside each area.
public class DungeonGenerator
{
    // All nodes created by the last space split
    private List<RoomNode> allNodesCollection = new();

    // Size of the whole dungeon
    private int dungeonWidth;
    private int dungeonLength;

    // Stores the size of the dungeon
    public DungeonGenerator(int dungeonWidth, int dungeonLength)
    {
        this.dungeonWidth = dungeonWidth;
        this.dungeonLength = dungeonLength;
    }

    // Returns the generated rooms
    public List<Node> CalculateDungeon(
        int maxIterations, int minRoomWidth, int minRoomLength, float roomBottomCornerModifier, float roomTopCornerModifier, int roomOffset, int corridorWidth)
    {
        // Split the dungeon area into smaller rectangles
        BinarySpacePartitioner bsp = new BinarySpacePartitioner(dungeonWidth, dungeonLength);
        allNodesCollection = bsp.PrepareNodesCollection(maxIterations, minRoomWidth, minRoomLength);

        // Take the final rectangles (the leaves of the tree)
        List<Node> roomSpaces = StructureHelper.TraverseGraphToExtractLowestLeaves(bsp.RootNode);

        // Place a room inside each of those rectangles
        RoomGenerator roomGenerator = new RoomGenerator(maxIterations, minRoomWidth, minRoomLength);
        List<RoomNode> roomList = roomGenerator.GenerateRoomsInGivenSpaces(roomSpaces, roomBottomCornerModifier, roomTopCornerModifier, roomOffset);

        CorridorsGenerator corridorGenerator = new CorridorsGenerator();
        var corridorList = corridorGenerator.CreateCorridors(allNodesCollection, corridorWidth);

        return new List<Node>(roomList).Concat(corridorList).ToList();
    }
}
