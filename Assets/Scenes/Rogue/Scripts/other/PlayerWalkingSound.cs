using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DualPantoToolkit;
using System.ComponentModel;

public class PlayerWalkingSound : MonoBehaviour
{

    public AudioClip walkSound;
    public bool onMeHandle = true;
    public bool onItHandle = false;

    [SerializeField]
    [Range(0f, 1f)]
    private float volume = 1f;

    [SerializeField]
    [Range(0f, 20f)]
    private float pitch = 1f;

    [SerializeField]
    [Description("Sensitivity for detecting movement. Lower values make it more sensitive.")]
    [Range(0f, 1f)]
    private float movementDetection = 1f;

    private Vector3 lastPositionItHandle;
    private Vector3 lastPositionMeHandle;

    private AudioSource audioSource;

    void Awake()
    {
        this.audioSource = gameObject.AddComponent<AudioSource>();
        if (audioSource != null)
            audioSource.volume = volume;
        audioSource.clip = walkSound;
    }
    void Update()
    {
        if (onMeHandle)
        {
            Vector3 handlePos = GameObject.Find("Panto").GetComponent<UpperHandle>().GetPosition();
            var handleDistance = UnityEngine.Vector3.Distance(handlePos, lastPositionMeHandle);

            if (handleDistance > movementDetection)
            {
                lastPositionMeHandle = handlePos;
                PlayWalkSound(handleDistance * pitch);
            }
        }
        if (onItHandle)
        {
            Vector3 handlePos = GameObject.Find("Panto").GetComponent<LowerHandle>().GetPosition();
            var handleDistance = UnityEngine.Vector3.Distance(handlePos, lastPositionItHandle);

            if (handleDistance > movementDetection)
            {
                lastPositionItHandle = handlePos;
                PlayWalkSound(handleDistance * pitch);
            }
        }
    }

    async void PlayWalkSound(float speed = 1)
    {
        audioSource.pitch = speed;
        if (audioSource != null && walkSound != null && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }
}


