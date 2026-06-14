using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class PlayerWallSound : MonoBehaviour
{
    public float detectionRange = 0.5f;
    public float minPitch = 0.5f;
    public float maxPitch = 2f;
    public float volume = 1f;
    public AudioClip soundClip;
    
    private AudioSource audioSource;
    private readonly HashSet<Collider> roomTriggers = new HashSet<Collider>();
    private readonly HashSet<Collider> corridorTriggers = new HashSet<Collider>();

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.clip = soundClip;
        audioSource.loop = true;
        audioSource.playOnAwake = false;
        audioSource.volume = volume;
    }

    private void Update()
    {
        Collider activeArea = GetActiveRoomCollider();
        if (activeArea == null)
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }

            return;
        }

        
        float distanceToWall = GetMinDistanceToWallsXZ(activeArea);
        float normalizedDistance = Mathf.Clamp01(distanceToWall / detectionRange);
        
        // float pitch = Mathf.Lerp(minPitch, maxPitch, 1f - normalizedDistance);
        // pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
        // audioSource.pitch = pitch;
        
        if (distanceToWall < detectionRange && corridorTriggers.Count == 0)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        RegisterAreaTrigger(collision);
    }

    private void OnTriggerStay(Collider collision)
    {
        RegisterAreaTrigger(collision);
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.CompareTag("Room"))
        {
            roomTriggers.Remove(collision);
            return;
        }

        if (collision.CompareTag("Corridor"))
        {
            corridorTriggers.Remove(collision);
        }
    }

    private void RegisterAreaTrigger(Collider collision)
    {
        if (collision.CompareTag("Room"))
        {
            roomTriggers.Add(collision);
            return;
        }

        if (collision.CompareTag("Corridor"))
        {
            corridorTriggers.Add(collision);
        }
    }

    private Collider GetActiveRoomCollider()
    {
        if (roomTriggers.Count > 0)
        {
            foreach (Collider room in roomTriggers)
            {
                if (room != null)
                {
                    return room;
                }
            }
        }

        return null;
    }

    private float GetMinDistanceToWallsXZ(Collider roomCollider)
    {
        Vector3 playerPosition = transform.position;
        Bounds bounds = roomCollider.bounds;

        float minX = bounds.min.x;
        float maxX = bounds.max.x;
        float minZ = bounds.min.z;
        float maxZ = bounds.max.z;

        float distanceToMinX = Mathf.Abs(playerPosition.x - minX);
        float distanceToMaxX = Mathf.Abs(maxX - playerPosition.x);
        float distanceToMinZ = Mathf.Abs(playerPosition.z - minZ);
        float distanceToMaxZ = Mathf.Abs(maxZ - playerPosition.z);

        float minDistance = Mathf.Min(distanceToMinX, distanceToMaxX, distanceToMinZ, distanceToMaxZ);

        return minDistance;
    }
}