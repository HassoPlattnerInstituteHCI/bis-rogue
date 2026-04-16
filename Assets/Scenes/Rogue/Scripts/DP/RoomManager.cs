using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using DualPantoToolkit;

public class RoomManager : MonoBehaviour
{

    [SerializeField]
    private GameObject foodPrefab;

    [SerializeField]
    private GameObject roomPrefab;

    [SerializeField]
    private GameObject enemyPrefab;

    [SerializeField]
    private GameObject finishPointPrefab;

    [SerializeField]
    [Range(0, 100)]
    public int enemySpawnProbability = 50; // percentage chance to spawn an enemy in a room

    private List<GameObject> rooms = new List<GameObject>();

    private Vector3 spawnPosition;

    public GameObject RoomObstacles;

    async void Start()
    {
        if (RoomObstacles == null)
        {
            Instantiate(RoomObstacles);
        }
        await Create();
    }

    public async Task Create()
    {
        await Task.Delay(1000); // wait a second to ensure all rooms are initialized
        FindAllRooms();
        Debug.Log("Number of rooms: " + rooms.Count);
       
        CreateRoomObsticles();
    }

    public void Reset()
    {
        foreach (Transform child in RoomObstacles.transform)
        {
            Destroy(child.gameObject);
        }
        rooms.Clear();
    }

    void CreateRoomObsticles()
    {
        
        if (rooms.Count != 0)
        {

            GameObject spawnRoom = null;
            foreach (GameObject room in rooms)
            {
                var roomComponent = room.GetComponent<Room>();
                if (roomComponent.isSpawnRoom)
                {
                    spawnRoom = room;
                    break;
                }
            }

            // find farthest room from spawn room to be finish room
            GameObject finishRoom = spawnRoom;
            spawnPosition = spawnRoom.transform.position;
            Vector3 farthestPos = spawnPosition;

            foreach (GameObject room in rooms)
            {
                var roomComponent = room.GetComponent<Room>();
                if (!roomComponent.isSpawnRoom)
                {
                    if (Vector3.Distance(room.transform.position, spawnPosition) > Vector3.Distance(farthestPos, spawnPosition))
                    {
                        farthestPos = room.transform.position;
                        finishRoom = room;
                    }
                }
            }

            int roomId = 1;
            foreach (GameObject room in rooms)
            {
                var roomScript = room.GetComponent<Room>();
                // add food to each room
                var foodGO = AddFoodToRoom(room);
                roomScript.gameObjectsInRoom.Add(foodGO);
                if (!roomScript.isSpawnRoom)  // <-- isSpawnRoom ist IMMER false!
                {
                    // add enemy based on probability
                    if (Random.Range(0, 100) < enemySpawnProbability)
                    {
                        var enemyGO = AddEnemyToRoom(room);
                        roomScript.gameObjectsInRoom.Add(enemyGO);
                    }
                    // add intro speech to non-spawn rooms
                    roomScript.AddIntroRoomSpeech(roomId);
                    roomId++;
                }
                if (room == finishRoom)
                {
                    // add finish point to finish room
                    Vector3 finishPointPos = new Vector3(finishRoom.transform.position.x, finishRoom.transform.position.y + 0.5f, finishRoom.transform.position.z);
                    Instantiate(finishPointPrefab, finishPointPos, Quaternion.identity, RoomObstacles.transform);
                    AddTextToSpawnRoom(spawnRoom, rooms.Count, roomId);
                }
                roomScript.UpdateRoomVisibility();
            }

        
        }
    }

    void AddTextToSpawnRoom(GameObject roomComponent, int roomCount, int finishRoomId)
    {
        // This is the spawn room
        string introductionText = "Hello Player! This is your starting room. Find food to heal and avoid enemies!. There are " + (rooms.Count - 1) + " rooms to explore. The finish room is room number " + finishRoomId + ". Good luck!";
        roomComponent.GetComponent<Room>().AddIntroRoomSpeech(0, introductionText);
        var roomSpeech = roomComponent.GetComponent<RoomSpeechOnEntry>();
        if (roomSpeech != null)
        {
            //after first visit, change the introduction text
            roomSpeech.triggerAfterSpeechEnd = true;
            UnityAction changeTextAction = () =>
            {
                roomComponent.GetComponent<Room>().ChangeIntroductionText("Start Room");
            };
            roomSpeech.onEnter.AddListener(changeTextAction);
        }
    }
    // add food item to the room at a random position
    GameObject AddFoodToRoom(GameObject room)
    {
        var (xPos, zPos, roomSize) = GetRandomPositionInRoom(room);
        Vector3 foodPosition = new Vector3(xPos, room.transform.position.y + 0.5f, zPos);

        return Instantiate(foodPrefab, foodPosition, Quaternion.identity, RoomObstacles.transform);
    }

    // add an enemy to the room at a random position
    GameObject AddEnemyToRoom(GameObject room)
    {
        var (xPos, zPos, roomSize) = GetRandomPositionInRoom(room);
        Vector3 enemyPosition = new Vector3(xPos, room.transform.position.y + 0.5f, zPos);

        var enemyGO = Instantiate(enemyPrefab, enemyPosition, Quaternion.identity, RoomObstacles.transform);
        var enemy = enemyGO.GetComponent<EnemyPanto>();
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

    // find all rooms by checking child objects with "Room" tag
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
    async Task MovePlayerToSpawn(Vector3 spawnPosition)
    {
        var handle = GameObject.Find("Panto").GetComponent<UpperHandle>();
        if(handle != null)
        {
            await handle.MoveToPosition(spawnPosition, 1f);
        }
    }
}
