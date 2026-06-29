using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using DualPantoToolkit;
using System;

public class PlayerRecoil : MonoBehaviour
{

    [SerializeField]
    [Range(0.0f, 10.0f)]
    private float recoilStrength = 1.5f;

    [SerializeField]
    [Range(1.0f, 10.0f)]
    private float recoilSpeed = 4f;

    private UpperHandle meHandle;

    void Start()
    {
        meHandle = GameObject.Find("Panto").GetComponent<UpperHandle>();
    }

    // checks for collision with enemy and applies recoil
    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Vector3 collisionPoint = collision.ClosestPoint(transform.position);
            ApplyRecoil(collisionPoint);
        }
    }

    // applies recoil to the player away from the collision point
    async void ApplyRecoil(Vector3 collisionPoint)
    {
        Vector3 currentPosition = Vector3.zero;
        // TODO: Get the current position of the player from the meHandle
        //currentPosition = 

        Vector3 recoilDirection = Vector3.zero;
        // TODO: calculate the direction of the recoil (coming from the collision point to the current position) and normalize it
        //recoilDirection = 

        //TODO: uncomment the line below to stretch the recoil direction by the recoil strength
        //Vector3 finalRecoilDirection = currentPosition - recoilDirection * recoilStrength;

        //TODO: Move the player to the new position using the meHandle.MoveToPosition method
        // use recoilSpeed to control the speed of the movement
        //await meHandle.

    }

}
