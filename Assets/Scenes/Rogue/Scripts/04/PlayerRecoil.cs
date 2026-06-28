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
    private float recoilStrength = 3f;

    [SerializeField]
    [Range(1.0f, 10.0f)]
    private float recoilSpeed = 5f;

    private UpperHandle meHandle;

    void Start()
    {
        meHandle = GameObject.Find("Panto").GetComponent<UpperHandle>();
    }

    // checks for collision with enemy and applies recoil
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            var collisionPoint = collision.contacts[0].normal;
            ApplyRecoil(collisionPoint);
        }
    }

    // applies recoil to the player away from the collision point
    async void ApplyRecoil(Vector3 collisionPoint)
    {
        // TODO: Get the current position of the player from the meHandle
        Vector3 currentPosition = meHandle.GetPosition();

        // TODO: calculate the direction of the recoil (coming from the collision point to the current position)
        Vector3 recoilDirection = (collisionPoint.normalized - currentPosition.normalized).normalized;

        //strech the recoil direction by the recoil strength)
        Vector3 finalRecoilDirection = currentPosition + recoilDirection * recoilStrength;

        //TODO: Move the player to the new position using the meHandle.MoveToPosition method
        await meHandle.MoveToPosition(finalRecoilDirection, recoilSpeed);

    }

}
