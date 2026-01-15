using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DualPantoToolkit;
using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

public class RoomData
{
    public int id;
    public float roomSizeX = 0f;
    public float roomSizeY = 0f;

    public int col;
    public int row;

    public bool connected = false;  // Whether room is connected to the dungeon network

    public RoomData(int id)
    {
        this.id = id;
    }
    public RoomData(int id, float roomSizeX, float roomSizeY, int col, int row)
    {
        this.id = id;
        this.roomSizeX = roomSizeX;
        this.roomSizeY = roomSizeY;
        this.col = col;
        this.row = row;
    }
}


public class GridRoomSpawner : MonoBehaviour
{


    // Insprector Settings ======================================================
    [Header("Grid Settings")]

    public GameObject plane;      // The world plane on which to place the grid

    // Grid dimensions - defines the number of cells for room placement
    public int rows = 3;
    public int columns = 3;


    // Room Inspector Settings
    [Header("Room Settings")]
    public GameObject roomPrefab;

    [Range(0.2f, 1.0f)]
    public float minRoomSize = 0.5f;
    [Range(0.2f, 1.0f)]
    public float maxRoomSize = 0.7f;

    [Range(0, 100)]
    public int probabilityOfRoomInCell = 80;

    [Header("Spawn Settings")]
    public bool spawnRoomInTopCenter = false; // Whether to force a spawn room in the top center cell

    // Corridor Inspector Settings
    [Header("Corridor Settings")]
    public GameObject corridorPrefab;

    [Range(0.01f, 0.20f)]
    public float corridorWidth = 0.05f;


    //======================================================

    private List<RoomData> rooms = new List<RoomData>();

    // Calculated cell dimensions based on plane size and grid dimensions
    private float cellWidth;
    private float cellHeight;

    public int[,] grid;  // 2D grid storing room IDs (0 = empty cell)

    void Awake()
    {
        Create();

    }

    public void Create()
    {
        CalculateGridFromPlane();
        grid = new int[rows, columns];
        //DrawGridWithIndices();
        CreateRooms();
        CreateCorridor();

        this.gameObject.AddComponent<PantoCompoundCollider>();
        this.gameObject.GetComponent<PantoCompoundCollider>().onLower = false;
    }

    void CalculateGridFromPlane()
    {
        if (plane == null)
        {
            Debug.LogError("Plane not assigned to GridManager.");
            return;
        }

        // Unity plane default size is 10x10, scaled by transform
        Vector3 worldScale = plane.transform.localScale;
        float totalWidth = 10f * worldScale.x;
        float totalHeight = 10f * worldScale.z;

        cellWidth = totalWidth / columns;
        cellHeight = totalHeight / rows;
    }

    public void CreateRooms()
    {
        int currentRoomId = 1;

        // Optionally force a spawn room in the first row, center column
        if (spawnRoomInTopCenter)
        {
            int spawnCol = columns / 2;
            int spawnRow = 0;
            if (spawnCol >= 0 && spawnCol < columns && grid[spawnRow, spawnCol] == 0)
            {
                CreateRoomAt(spawnRow, spawnCol, currentRoomId, true);
                currentRoomId++;
            }
        }

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                if (grid[row, col] == 0)
                {
                    if (UnityEngine.Random.Range(0, 100) < probabilityOfRoomInCell)
                    {
                        CreateRoomAt(row, col, currentRoomId, currentRoomId == 1);
                        currentRoomId++;
                    }
                }
            }
        }
    }

    private void CreateRoomAt(int row, int col, int roomId, bool isSpawnRoom)
    {
        float roomSizeX = UnityEngine.Random.Range(minRoomSize, maxRoomSize) * cellWidth;
        float roomSizeY = UnityEngine.Random.Range(minRoomSize, maxRoomSize) * cellHeight;

        var roomData = new RoomData(roomId, roomSizeX, roomSizeY, col, row);
        AddRoomToMap(roomData, isSpawnRoom);
        rooms.Add(roomData);
        grid[row, col] = roomId;
    }
    private void AddRoomToMap(RoomData roomData, bool isSpawnRoom = false)
    {
        Vector3 pos = RoomWorldCenterPos(roomData.col, roomData.row);

        var room = Instantiate(roomPrefab, pos, Quaternion.identity, this.gameObject.transform);

        // Scale the room to its calculated dimensions
        room.transform.localScale = new Vector3(roomData.roomSizeX, 0f, roomData.roomSizeY);

        var roomComponent = room.GetComponent<Room>();
        if (roomComponent != null)
        {
            roomComponent.isSpawnRoom = isSpawnRoom;
        }
    }

    private Vector3 RoomWorldCenterPos(RoomData roomData)
    {
        return RoomWorldCenterPos(roomData.col, roomData.row);
    }

    private Vector3 RoomWorldCenterPos(int col, int row)
    {
        Vector3 worldScale = plane.transform.localScale;
        float totalWidth = 10f * worldScale.x;
        float totalHeight = 10f * worldScale.z;

        // Calculate bottom-left corner of the plane
        Vector3 planeZero = plane.transform.position - new Vector3(totalWidth * 0.5f, 0f, totalHeight * 0.5f);

        return planeZero + new Vector3((col + 0.5f) * cellWidth, 0f, (rows - 1 - row + 0.5f) * cellHeight);
    }

    private void CreateCorridor()
    {
        if (rooms.Count == 0)
            return;

        // Start with the first room as connected
        rooms[0].connected = true;

        // Connect all other rooms with already connected rooms
        for (int i = 1; i < rooms.Count; i++)
        {
            RoomData room = rooms[i];

            // Find the nearest already connected neighbor (search radius grows recursively)
            int? neighborRoomId = FindConnectedNeighbor(room.id, room.col, room.row, 1);
            if (!neighborRoomId.HasValue)
            {
                Debug.LogWarning("Room could not be connected to the network");
                continue;
            }

            ConnectNeighbor(room, neighborRoomId.Value);
            room.connected = true;
        }
    }

    private int? FindConnectedNeighbor(int roomId, int startCol, int startRow, int maxSearchRadius)
    {
        if (maxSearchRadius > Math.Max(columns, rows))
        {
            Debug.LogError("room could not be connected");
            return null;
        }

        // Search in a square area around the starting position
        for (int col = startCol - maxSearchRadius; col <= startCol + maxSearchRadius; col++)
        {
            for (int row = startRow - maxSearchRadius; row <= startRow + maxSearchRadius; row++)
            {
                if (col >= 0 && row >= 0 && col < columns && row < rows)
                {
                    var cell = grid[row, col];
                    if (cell > 0 && cell != roomId)  // Found a room that's not the current one
                    {
                        var neighbor = rooms.FirstOrDefault(r => r.id == cell && r.connected);
                        if (neighbor != null)
                            return neighbor.id;
                    }
                }
            }
        }
        // If no connected neighbors found at this radius, expand the search
        return FindConnectedNeighbor(roomId, startCol, startRow, maxSearchRadius + 1);
    }

    private void ConnectNeighbor(RoomData room, int roomIdNeighbor)
    {
        var neighborRoom = rooms.FirstOrDefault(x => x.id == roomIdNeighbor);
        if (neighborRoom == null)
            return;

        // Calculate the offset to determine corridor shape
        int offsetCol = neighborRoom.col - room.col;
        int offsetRow = neighborRoom.row - room.row;

        // Same row or column: simple, straight corridor
        if (offsetCol == 0 || offsetRow == 0)
        {
            Vector3 start = RoomWorldCenterPos(room);
            Vector3 end = RoomWorldCenterPos(neighborRoom);
            CreateCorridorSegment(start, end);
        }
        else
        {
            // Offset cells: corridor in two segments (L-shape)
            // First segment horizontal, second vertical
            Vector3 a = RoomWorldCenterPos(room);
            Vector3 b = RoomWorldCenterPos(neighborRoom.col, room.row);   // Same row, target column
            Vector3 c = RoomWorldCenterPos(neighborRoom);

            // Extend each segment along its own direction to create proper overlap at corner
            float overlapExtension = cellWidth * corridorWidth * 0.5f;
            Vector3 dirHorizontal = (b - a).normalized;
            Vector3 dirVertical = (c - b).normalized;

            CreateCorridorSegment(a, b + dirHorizontal * overlapExtension);
            CreateCorridorSegment(b - dirVertical * overlapExtension, c);
        }
    }

    private void CreateCorridorSegment(Vector3 start, Vector3 end)
    {
        if (corridorPrefab == null)
            return;

        Vector3 dir = end - start;
        float length = dir.magnitude;
        if (length <= 0.001f)  // Skip if points are too close
            return;

        // Position corridor at midpoint between start and end
        Vector3 pos = start + dir * 0.5f;
        Quaternion rot = Quaternion.LookRotation(dir.normalized, Vector3.up);

        var corridor = Instantiate(corridorPrefab, pos, rot, this.gameObject.transform);

        // We assume the prefab is aligned along its local Z-axis
        Vector3 scale = corridor.transform.localScale;
        scale.z = length;
        // Adapt corridor width to cell size (scalable via corridorWidth)
        scale.x = cellWidth * corridorWidth;
        corridor.transform.localScale = scale;
    }



    public Vector3 GetWorldPosition(int x, int y)
    {
        // Determine the size of the plane in world space
        Vector3 worldScale = plane.transform.localScale;
        float totalWidth = 10f * worldScale.x;
        float totalHeight = 10f * worldScale.z;

        // Align cell centers to the center of the plane
        float originX = plane.transform.position.x - totalWidth / 2f + cellWidth / 2f;
        float originZ = plane.transform.position.z - totalHeight / 2f + cellHeight / 2f;

        return new Vector3(originX + x * cellWidth, 0, originZ + y * cellHeight);
    }
}
