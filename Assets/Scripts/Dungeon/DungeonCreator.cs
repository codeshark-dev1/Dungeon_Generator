using UnityEngine;

public class DungeonCreator : MonoBehaviour
{
    [SerializeField] private int DungeonWidth;
    [SerializeField] private int DungeonLength;

    [SerializeField] private int MinRoomWidth;
    [SerializeField] private int MinRoomLength;

    [SerializeField] private int MaxIterations;
    [SerializeField] private int CorridorWidth;

    private void Start()
    {
        CreateDungeon();
    }

    private void CreateDungeon()
    {
        DungeonGenerator generator = new DungeonGenerator(DungeonWidth, DungeonLength);
        var listOfRooms = generator.CalculateRooms(MaxIterations, MinRoomWidth, MinRoomLength);
    }
}
