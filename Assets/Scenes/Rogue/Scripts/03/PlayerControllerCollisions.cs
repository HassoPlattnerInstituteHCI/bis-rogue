
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerCollisions : MonoBehaviour
{
    [Range(0.1f, 1f)]
    public float moveStep = 1f;

    public GameObject map;

    [Range(0.05f, 0.5f)]
    public float stepCooldown = 0.15f; // Time between steps when holding

    private float nextStepTime = 0f; // Time of the next allowed step

    void Start()
    {
        
    }
    void Update()
    {
        // Only move if cooldown has expired
        if (Time.time < nextStepTime)
            return;

        var movementVector = new Vector3();

        // GetKey instead of GetKeyDown: movement even when key is held
        if (Input.GetKey(KeyCode.UpArrow))
        {
            movementVector = new Vector3(0f, 0f, moveStep);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            movementVector = new Vector3(0f, 0f, -moveStep);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            movementVector = new Vector3(-moveStep, 0f, 0f);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            movementVector = new Vector3(moveStep, 0f, 0f);
        }
        if (movementVector != Vector3.zero)
        {

            Vector3 proposedPosition = transform.position + movementVector;
            if (CheckCollisions(proposedPosition))
            {
                transform.position = proposedPosition;
                nextStepTime = Time.time + stepCooldown; // Schedule next step
            }
        }
    }

    private bool CheckCollisions(Vector3 proposedPosition)
    {
        // Only move if target point is within the map colliders
        if (map != null)
        {
            var cols = map.GetComponentsInChildren<Collider>();
            foreach (var c in cols)
            {
                if (c == null || !c.enabled) continue;
                if (!c.bounds.Contains(proposedPosition)) continue; // quick pre-filter

                Vector3 closest = c.ClosestPoint(proposedPosition);
                if ((closest - proposedPosition).sqrMagnitude < 1e-6f)
                {
                    return true; // Collision detected

                }
            }

        }
        return false; // No collision, movement not allowed
    }


}
