using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartEnemy : MonoBehaviour
{
    [SerializeField]
    [Range(0.0f, 10.0f)]
    public float speed = 5f;


    private GameObject player;

    private Vector3 lastPlayerPosition = Vector3.zero;


    private Vector2 roomCenter;
    private Vector2 roomSize;

    private bool playerInRange = false;


    //Room mesurements
    private float roomMaxX;
    private float roomMinX;
    private float roomMaxZ;
    private float roomMinZ;
    private float roomTolerance = 0.1f;

    void Start()
    {
        player = GameObject.Find("Player");
        if (player == null)
        {
            Debug.LogWarning("Player not found in scene.");
        }
    }

    void Update()
    {
        MoveEnemy();
    }

    void OnValidate()
    {
        UpdateRoomBounds();
    }

    // calculate room bounds based on center and size
    public void SetRoomBounds(Vector2 center, Vector2 size)
    {
        roomCenter = center;
        roomSize = size;
        UpdateRoomBounds();
    }

    // update min/max bounds based on center and size of the room
    private void UpdateRoomBounds()
    {
        // Only compute if we have a sensible room size
        if (roomSize.x <= 0f || roomSize.y <= 0f)
            return;

        float halfX = roomSize.x * 0.5f;
        float halfZ = roomSize.y * 0.5f;

        roomMaxX = roomCenter.x + halfX;
        roomMinX = roomCenter.x - halfX;
        roomMaxZ = roomCenter.y + halfZ;
        roomMinZ = roomCenter.y - halfZ;
    }

    // move enemy towards player, if player is in range
    private void MoveEnemy()
    {

        if (!CheckPlayerInRange())
        {
            return; // Player is out of range
        }

        if (player.transform.position == lastPlayerPosition)
        {
            return; // Player is not moving
        }

        lastPlayerPosition = player.transform.position;

        Vector3 direction = (lastPlayerPosition - this.transform.position).normalized;
        float dt = Time.deltaTime;

        Vector3 nextPos = this.transform.position + direction * speed * dt;
        this.transform.position = new Vector3(nextPos.x, 0, nextPos.z);
    }

    // check if player is within room bounds plus tolerance
    bool CheckPlayerInRange()
    {

        Vector3 playerPosLocal = player.transform.position;

        if (playerPosLocal.x >= roomMinX - roomTolerance && playerPosLocal.x <= roomMaxX + roomTolerance &&
            playerPosLocal.z >= roomMinZ - roomTolerance && playerPosLocal.z <= roomMaxZ + roomTolerance)
        {
            return true;
        }
        return false;
    }
}


