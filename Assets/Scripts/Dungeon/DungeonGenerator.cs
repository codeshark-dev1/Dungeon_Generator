using System.Collections.Generic;

// Creates the rooms of a dungeon in three steps:
// split the space, collect the final areas, and place a room inside each area.
public class DungeonGenerator
{
    // All nodes created by the last space split
    private List<RoomNode> allSpaceNodes = new();

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
    public List<Node> CalculateRooms(int maxIterations, int minRoomWidth, int minRoomLength)
    {
        // Split the dungeon area into smaller rectangles
        BinarySpacePartitioner bsp = new BinarySpacePartitioner(dungeonWidth, dungeonLength);
        allSpaceNodes = bsp.PrepareNodesCollection(maxIterations, minRoomWidth, minRoomLength);

        // Take the final rectangles (the leaves of the tree)
        List<Node> roomSpaces = StructureHelper.TraverseGraphToExtractLowestLeaves(bsp.RootNode);

        // Place a room inside each of those rectangles
        RoomGenerator roomGenerator = new RoomGenerator(maxIterations, minRoomWidth, minRoomLength);
        List<RoomNode> roomList = roomGenerator.GenerateRoomsInGivenSpaces(roomSpaces);

        return new List<Node>(roomList);
    }
}
