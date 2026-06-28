using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using DualPantoToolkit;

public class EnemyMovement : MonoBehaviour
{
    [Range(0.0f, 10.0f)]
    public float speed = 5f;

    public float movementThreshold = 0.03f; // Minimum distance to move before updating position

    private GameObject player;

    private Vector3 lastPlayerPosition = Vector3.zero;

    private bool playerInRange = false;

    //Room mesurements
    private Vector2 roomCenter;
    private Vector2 roomSize;

    private float roomMaxX = 1000f;
    private float roomMinX = -1000f;
    private float roomMaxZ = 1000f;
    private float roomMinZ = -1000f;
    private float roomTolerance = 0.1f;


    private UpperHandle meHandle;

    private LowerHandle lowerHandle;

    void Start()
    {
        player = GameObject.Find("Player");
        if (player == null)
        {
            Debug.LogWarning("Player not found in scene.");
        }

        // Get the UpperHandle and LowerHandle components from the Panto GameObject
        meHandle = GameObject.Find("Panto").GetComponent<UpperHandle>();
        lowerHandle = GameObject.Find("Panto").GetComponent<LowerHandle>();
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
    private async void MoveEnemy()
    {
        if (meHandle == null) return;

        if (!CheckPlayerInRange())
        {
            return; // Player is out of range
        }
        else
        {
            // TODO: Switch to the LowerHandle before moving the enemy, for more information, see: https://github.com/HassoPlattnerInstituteHCI/unity-dualpanto-toolkit/blob/develop/Documentation/documentation.md 
            // use: this.gameObject and set speed > 50f
            //await lowerHandle.
        }

        Vector3 currentPlayerPosition = Vector3.zero; 
        // TODO: Get the current position of the player from the meHandle
        // for more information, see: https://github.com/HassoPlattnerInstituteHCI/unity-dualpanto-toolkit/blob/develop/Documentation/documentation.md
        
        //currentPlayerPosition = 

        if (Vector3.Distance(currentPlayerPosition, lastPlayerPosition) > movementThreshold)
        {
            lastPlayerPosition = currentPlayerPosition;

            Vector3 direction = (currentPlayerPosition - transform.position).normalized;
            float dt = Time.deltaTime;

            Vector3 nextPos = transform.position + direction * speed * dt;
            if (nextPos.x < roomMinX || nextPos.x > roomMaxX || nextPos.z < roomMinZ || nextPos.z > roomMaxZ)
            {
                // Out of bounds, do not move
                return;
            }
            transform.position = nextPos;
        }

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


