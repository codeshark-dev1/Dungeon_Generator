using System;
using System.Collections.Generic;
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

    [SerializeField] private GameObject WallVertical;
    [SerializeField] private GameObject WallHorizontal;

    private List<Vector3Int> possibleDoorVerticalPositions;
    private List<Vector3Int> possibleDoorHorizontalPositions;
    private List<Vector3Int> possibleWallHorizontalPositions;
    private List<Vector3Int> possibleWallVerticalPositions;

    // Unity calls this once when the scene starts
    private void Start()
    {
        CreateDungeon();
    }

    // Builds the dungeon with the current settings.
    // The generated rooms are not drawn yet.
    public void CreateDungeon()
    {
        float creationTime = Time.time;
        Debug.Log("Starting dungeon creation...");

        DestroyAllChildren();

        DungeonGenerator generator = new DungeonGenerator(DungeonWidth, DungeonLength);
        var listOfRooms = generator.CalculateDungeon(MaxIterations, MinRoomWidth, MinRoomLength, RoomBottomCornerModifier, RoomTopCornerModifier, RoomOffset, CorridorWidth);

        GameObject wallParent = new GameObject("WallParent");
        wallParent.transform.parent = this.transform;

        possibleDoorVerticalPositions = new();
        possibleDoorHorizontalPositions = new();
        possibleWallHorizontalPositions = new();
        possibleWallVerticalPositions = new();

        for (int i = 0; i < listOfRooms.Count; i++)
            CreateMesh(listOfRooms[i].BottomLeftAreaCorner, listOfRooms[i].TopRightAreaCorner);

        CreateWalls(wallParent);

        Debug.Log($"Done in {Time.time - creationTime}s.");
    }

    private void CreateWalls(GameObject wallParent)
    {
        foreach (var wallPosition in possibleWallHorizontalPositions)
            CreateWall(wallParent, wallPosition, WallHorizontal);

        foreach (var wallPosition in possibleWallVerticalPositions)
            CreateWall(wallParent, wallPosition, WallVertical);
    }

    private void CreateWall(GameObject wallParent, Vector3Int wallPosition, GameObject wallPrefab)
    {
        Instantiate(wallPrefab, wallPosition, Quaternion.identity, wallParent.transform);
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

        GameObject dungeonFloor = new GameObject("Mesh" + bottomLeftCorner, typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider));

        dungeonFloor.transform.position = Vector3.zero;
        dungeonFloor.transform.localScale = Vector3.one;
        dungeonFloor.GetComponent<MeshFilter>().mesh = mesh;
        dungeonFloor.GetComponent<MeshCollider>().sharedMesh = mesh;
        dungeonFloor.GetComponent<MeshRenderer>().material = Material;
        dungeonFloor.transform.parent = this.transform;

        for (int row = (int)bottomLeftVert.x; row < (int)bottomRightVert.x; row++)
        {
            var wallPosition = new Vector3(row, 0, bottomLeftVert.z);
            AddWallPositionToList(wallPosition, possibleWallHorizontalPositions, possibleDoorHorizontalPositions);
        }

        for (int row = (int)topLeftVert.x; row < (int)topRightCorner.x; row++)
        {
            var wallPosition = new Vector3(row, 0, topRightVert.z);
            AddWallPositionToList(wallPosition, possibleWallHorizontalPositions, possibleDoorHorizontalPositions);
        }

        for (int col = (int)bottomLeftVert.z; col < (int)topLeftVert.z; col++)
        {
            var wallPosition = new Vector3(bottomLeftVert.x, 0, col);
            AddWallPositionToList(wallPosition, possibleWallVerticalPositions, possibleDoorVerticalPositions);
        }

        for (int col = (int)bottomRightVert.z; col < (int)topRightVert.z; col++)
        {
            var wallPosition = new Vector3(bottomRightVert.x, 0, col);
            AddWallPositionToList(wallPosition, possibleWallVerticalPositions, possibleDoorVerticalPositions);
        }
    }

    private void AddWallPositionToList(Vector3 wallPosition, List<Vector3Int> wallList, List<Vector3Int> doorList)
    {
        Vector3Int point = Vector3Int.CeilToInt(wallPosition);
        if (wallList.Contains(point))
        {
            doorList.Add(point);
            wallList.Remove(point);
        }
        else wallList.Add(point);
    }

    private void DestroyAllChildren()
    {
        while (transform.childCount != 0)
        {
            foreach (Transform item in this.transform)
            {
                DestroyImmediate(item.gameObject);
            }
        }
    }
}
