using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class RoomSpawnRandom : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject foodPrefab;

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
            var foodGameObject = AddFoodToRoom(room);
            roomComponent.gameObjectsInRoom.Add(foodGameObject);
            // add enemy based on probability
            if (Random.Range(0, 100) < enemySpawnProbability && !roomComponent.isSpawnRoom)
            {
                var enemyGameObject = AddEnemyToRoom(room);
                roomComponent.gameObjectsInRoom.Add(enemyGameObject);
            }
            roomComponent.UpdateRoomVisibility();
            // add intro speech to non-spawn rooms
        }
    }

    // add food item to the room at a random position

    // TODO: implement food spawning at a random position within the room bounds
    GameObject AddFoodToRoom(GameObject room)
    {
        var (xPos, zPos, roomSize) = GetRandomPositionInRoom(room);

        // TODO 1: implement food spawning at the random position (xPos, zPos) within the room
        // use 'room.transform.position.y + 0.5f' for the y position to make sure the food is above the floor
        // hint: use new Vector3(x,y,z) for the position
        Vector3 foodPosition; 

        // TODO 2: create the food prefab at the calculated position and return the created game object
        // hint: use Instantiate(prefab, position, rotation) to create the food game object in the scene
        // hint: use Quaternion.identity for the rotation if you don't want to rotate the food item
        // hint: Instantiate returns the created game object, so you can return it directly from this method 
        
        return null; // replace with the created food game object
    }

    // add an enemy to the room at a random position
    GameObject AddEnemyToRoom(GameObject room)
    {
        var (xPos, zPos, roomSize) = GetRandomPositionInRoom(room);
        Vector3 enemyPosition = new Vector3(xPos, room.transform.position.y + 0.5f, zPos);

        var enemyGO = Instantiate(enemyPrefab, enemyPosition, Quaternion.identity);
        var enemy = enemyGO.GetComponent<EnemyMovement>();
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