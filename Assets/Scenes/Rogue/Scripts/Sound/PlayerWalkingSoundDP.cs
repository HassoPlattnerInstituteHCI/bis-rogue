using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DualPantoToolkit;
using System.ComponentModel;

public class PlayerWalkingSoundDP : MonoBehaviour
{

    public AudioClip walkSoundRoom;

    [SerializeField]
    [Range(0f, 1f)]
    private float volume = 1f;


    [SerializeField]
    [Range(500f, 22000f)]
    private float corridorLowPassCutoff = 1800f;

    [SerializeField]
    private AudioReverbPreset roomReverbPreset = AudioReverbPreset.Off;

    [SerializeField]
    private AudioReverbPreset corridorReverbPreset = AudioReverbPreset.Cave;
    
    [SerializeField]
    [Description("Sensitivity for detecting movement. Lower values make it more sensitive.")]
    [Range(0f, 1f)]
    private float movementDetection = 1f;
    
    private AudioSource audioSource;
    private AudioLowPassFilter lowPassFilter;
    private AudioReverbFilter reverbFilter;

    private readonly HashSet<Collider> roomTriggers = new HashSet<Collider>();
    private readonly HashSet<Collider> corridorTriggers = new HashSet<Collider>();

    private Vector3 lastPosition;

    void Awake()
    {
        this.audioSource = gameObject.AddComponent<AudioSource>();
        this.lowPassFilter = gameObject.AddComponent<AudioLowPassFilter>();
        this.reverbFilter = gameObject.AddComponent<AudioReverbFilter>();

        if (audioSource != null)
            audioSource.volume = volume;

        if (lowPassFilter != null)
            lowPassFilter.enabled = false;

        if (reverbFilter != null)
            reverbFilter.reverbPreset = roomReverbPreset;

        audioSource.clip = walkSoundRoom;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Room"))
        {
            roomTriggers.Add(other);
            return;
        }

        if (other.CompareTag("Corridor"))
        {
            corridorTriggers.Add(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Room"))
        {
            roomTriggers.Remove(other);
            return;
        }

        if (other.CompareTag("Corridor"))
        {
            corridorTriggers.Remove(other);
        }
    }

    private bool IsCorridorOnly()
    {
        bool inRoom = roomTriggers.Count > 0;
        bool inCorridor = corridorTriggers.Count > 0;

        return inCorridor && !inRoom;
    }

    private void UpdateEnvironmentEffects()
    {
        bool useCorridorFx = IsCorridorOnly();

        if (lowPassFilter != null)
        {
            lowPassFilter.enabled = useCorridorFx;
            lowPassFilter.cutoffFrequency = corridorLowPassCutoff;
        }

        if (reverbFilter != null)
        {
            reverbFilter.reverbPreset = useCorridorFx ? corridorReverbPreset : roomReverbPreset;
        }
    }

    void Update()
    {
        UpdateEnvironmentEffects();
        
        var handleDistance = UnityEngine.Vector3.Distance(this.transform.position, lastPosition);

        if (handleDistance > movementDetection)
        {
            PlayWalkSound(handleDistance);
            lastPosition = this.transform.position;
        }    
    }

    async void PlayWalkSound(float speed = 1)
    {
        if (walkSoundRoom == null || audioSource == null)
        {
            return;
        }

        if (audioSource.clip != walkSoundRoom)
        {
            audioSource.clip = walkSoundRoom;
        }

        audioSource.pitch = speed;
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }
}

