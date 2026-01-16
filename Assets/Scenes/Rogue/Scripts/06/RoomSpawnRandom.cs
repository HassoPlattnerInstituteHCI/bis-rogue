using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class RoomSpawnRandom : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject foodPrefab;

    [SerializeField]
    [Range(0, 100)]
    public int enemySpawnProbability = 50;



    private List<GameObject> rooms = new List<GameObject>();

    void Start()
    {
        FindAllRooms();
        SpawnRandomly();
    }

    void Update()
    {

    }


    void FindAllRooms()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject child = transform.GetChild(i).gameObject;
            if (child.CompareTag("Room") && child.GetComponent<Room>() != null)
            {
                rooms.Add(child);
            }
        }
    }


    void SpawnRandomly()
    {
        // Implement random spawning logic here
        foreach (GameObject room in rooms)
        {
            var roomComponent = room.GetComponent<Room>();
            // add food to each room
            var gOF = AddFoodToRoom(room);
            roomComponent.gameObjectsInRoom.Add(gOF);
            // add enemy based on probability
            if (Random.Range(0, 100) < enemySpawnProbability && !roomComponent.isSpawnRoom)
            {
                var gOE = AddEnemyToRoom(room);
                roomComponent.gameObjectsInRoom.Add(gOE);
            }
            roomComponent.UpdateRoomVisibility();
            // add intro speech to non-spawn rooms
        }
    }

    // add food item to the room at a random position
    GameObject AddFoodToRoom(GameObject room)
    {
        var (xPos, zPos, roomSize) = GetRandomPositionInRoom(room);
        Vector3 foodPosition = new Vector3(xPos, room.transform.position.y + 0.5f, zPos);

        return Instantiate(foodPrefab, foodPosition, Quaternion.identity);
    }

    // add an enemy to the room at a random position
    GameObject AddEnemyToRoom(GameObject room)
    {
        var (xPos, zPos, roomSize) = GetRandomPositionInRoom(room);
        Vector3 enemyPosition = new Vector3(xPos, room.transform.position.y + 0.5f, zPos);

        var enemyGO = Instantiate(enemyPrefab, enemyPosition, Quaternion.identity);
        var enemy = enemyGO.GetComponent<SmartEnemy>();
        enemy.SetRoomBounds(new Vector2(room.transform.position.x, room.transform.position.z),
                new Vector2(roomSize.x, roomSize.z));
        return enemyGO;
    }

    // calculate a random position within the room bounds
    (float posX, float posZ, Vector3 roomSize) GetRandomPositionInRoom(GameObject room)
    {
        Vector3 roomSize = room.GetComponent<Renderer>().bounds.size;
        Vector3 roomPosition = room.transform.position;

        float xPos = Random.Range(roomPosition.x - roomSize.x / 2 + 0.5f, roomPosition.x + roomSize.x / 2 - 0.5f);
        float zPos = Random.Range(roomPosition.z - roomSize.z / 2 + 0.5f, roomPosition.z + roomSize.z / 2 - 0.5f);

        return (xPos, zPos, roomSize);
    }
}