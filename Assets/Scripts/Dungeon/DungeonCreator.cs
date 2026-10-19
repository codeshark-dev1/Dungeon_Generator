using System;
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
    [SerializeField] private Material Material;

    [Range(0.0f, 0.3f)]
    [SerializeField] private float RoomBottomCornerModifier;

    [Range(0.7f, 1.0f)]
    [SerializeField] private float RoomTopCornerModifier;

    [Range(0, 2)]
    [SerializeField] private int RoomOffset;

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
        var listOfRooms = generator.CalculateDungeon(MaxIterations, MinRoomWidth, MinRoomLength, RoomBottomCornerModifier, RoomTopCornerModifier, RoomOffset, CorridorWidth);
        for (int i = 0; i < listOfRooms.Count; i++)
        {
            CreateMesh(listOfRooms[i].BottomLeftAreaCorner, listOfRooms[i].TopRightAreaCorner);
        }
    }

    private void CreateMesh(Vector2 bottomLeftCorner, Vector2 topRightCorner)
    {
        Vector3 bottomLeftVert = new Vector3(bottomLeftCorner.x, 0, bottomLeftCorner.y);
        Vector3 bottomRightVert = new Vector3(topRightCorner.x, 0, bottomLeftCorner.y);
        Vector3 topLeftVert = new Vector3(bottomLeftCorner.x, 0, topRightCorner.y);
        Vector3 topRightVert = new Vector3(topRightCorner.x, 0, topRightCorner.y);

        Vector3[] vertices = new Vector3[]
        {
            topLeftVert,
            topRightVert,
            bottomLeftVert,
            bottomRightVert
        };

        Vector2[] uvs = new Vector2[vertices.Length];
        for (int i = 0; i < uvs.Length; i++)
        {
            uvs[i] = new Vector2(vertices[i].x, vertices[i].z);
        }

        int[] triangles = new int[]
        {
            0,
            1,
            2,
            2,
            1,
            3
        };

        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = triangles;

        GameObject dungeonFloor = new GameObject("Mesh" + bottomLeftCorner, typeof(MeshFilter), typeof(MeshRenderer));

        dungeonFloor.transform.position = Vector3.zero;
        dungeonFloor.transform.localScale = Vector3.one;
        dungeonFloor.GetComponent<MeshFilter>().mesh = mesh;
        dungeonFloor.GetComponent<MeshRenderer>().material = Material;

    }
}
