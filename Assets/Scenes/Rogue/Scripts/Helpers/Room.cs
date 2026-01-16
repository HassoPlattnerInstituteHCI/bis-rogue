using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField]
    public bool isSpawnRoom = false;

    public int countEnemies = 0;


    // Dark room property
    [Header("Dark Room Settings")]
    public bool isDarkRoom = false;

    public List<GameObject> gameObjectsInRoom = new List<GameObject>();

    public Material materialNormalRoom;
    public Material materialDarkRoom;
    
    
    void Start()
    {
        // Load normal material
        if (materialNormalRoom != null)
        {
            this.GetComponent<Renderer>().material = materialNormalRoom;
        }
        // Set initial room visibility
        UpdateRoomVisibility();
    }

    void OnValidate()
    {
        UpdateRoomVisibility();
    }

    // Update Room Visibility
    public void UpdateRoomVisibility()
    {
        foreach (GameObject obj in gameObjectsInRoom)
        {
            Renderer rend = obj.GetComponent<Renderer>();
            if (rend != null)
            {
                if (isDarkRoom)
                {
                    rend.enabled = false;
                }
                else
                {
                    rend.enabled = true;
                }
            }
        }
        // Update room material
        Renderer roomRend = this.GetComponent<Renderer>();
        if (roomRend != null)
        {
            if (isDarkRoom)
            {
                if(materialDarkRoom != null)
                {
                    roomRend.material = materialDarkRoom;
                }
            }
            else
            {
                roomRend.material = materialNormalRoom;
            }
        }

    }



    // add class <RoomSpeechOnEntry> to this room and set introduction text
    public void AddIntroRoomSpeech(int roomId, string introductionText = null)
    {
        var roomSpeech = this.gameObject.AddComponent<RoomSpeechOnEntry>();
        roomSpeech.introductionText = introductionText ?? "You have entered room number " + roomId + ".";
    }

    // change the introduction text of the RoomSpeechOnEntry component
    public void ChangeIntroductionText(string newText)
    {
        var roomSpeech = this.gameObject.GetComponent<RoomSpeechOnEntry>();
        if (roomSpeech != null)
        {
            roomSpeech.introductionText = newText;
        }
    }


}
