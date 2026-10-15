using UnityEngine;

// Entry point of the dungeon generator.
// Attach this script to a GameObject in the scene and set the values in the Inspector.
public class DungeonCreator : MonoBehaviour
{
    // Total size of the dungeon, in grid units
    [SerializeField] private int DungeonWidth;
    [SerializeField] private int DungeonLength;

    // Smallest allowed size of a room, in grid units
    [SerializeField] private int MinRoomWidth;
    [SerializeField] private int MinRoomLength;

    // Maximum number of splits used to divide the dungeon
    [SerializeField] private int MaxIterations;
    // Width of the corridors between rooms (corridors are not implemented yet)
    [SerializeField] private int CorridorWidth;

    // Unity calls this once when the scene starts
    private void Start()
    {
        CreateDungeon();
    }

    // Builds the dungeon with the current settings.
    // The generated rooms are not drawn yet.
    private void CreateDungeon()
    {
        DungeonGenerator generator = new DungeonGenerator(DungeonWidth, DungeonLength);
        var listOfRooms = generator.CalculateRooms(MaxIterations, MinRoomWidth, MinRoomLength);
    }
}
