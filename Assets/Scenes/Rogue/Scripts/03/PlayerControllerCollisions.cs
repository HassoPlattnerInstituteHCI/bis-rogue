
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerCollisions : MonoBehaviour
{
    [SerializeField]
    [Range(0.1f, 1f)]
    public float moveStep = 1f;

    public GameObject map;

    [SerializeField]
    [Range(0.05f, 0.5f)]
    public float stepCooldown = 0.15f; // Zeit zwischen Schritten beim Halten

    private CharacterController controller;

    private float nextStepTime = 0f; // Zeitpunkt des nächsten erlaubten Schritts

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }
    void Update()
    {
        // Nur bewegen, wenn Cooldown abgelaufen ist
        if (Time.time < nextStepTime)
            return;

        var movementVector = new Vector3();

        // GetKey statt GetKeyDown: Bewegung auch wenn Taste gehalten wird
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
                nextStepTime = Time.time + stepCooldown; // Nächsten Schritt zeitlich planen
            }
        }
    }

    private bool CheckCollisions(Vector3 proposedPosition)
    {
        // Nur bewegen, wenn Zielpunkt innerhalb der Map-Collider liegt
        if (map != null)
        {
            var cols = map.GetComponentsInChildren<Collider>();
            foreach (var c in cols)
            {
                if (c == null || !c.enabled) continue;
                if (!c.bounds.Contains(proposedPosition)) continue; // schneller Vorfilter

                Vector3 closest = c.ClosestPoint(proposedPosition);
                if ((closest - proposedPosition).sqrMagnitude < 1e-6f)
                {
                    return true; // Kollision erkannt

                }
            }

        }
        return false; // Keine Kollision, Bewegung nicht erlaubt
    }


}
