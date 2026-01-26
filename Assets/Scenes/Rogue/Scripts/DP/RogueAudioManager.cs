using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RogueAudioManager : MonoBehaviour
{
    public AudioClip playerHitSound;

    public AudioClip enemyHitSound;

    public AudioClip enemyPresentSound;

    public AudioClip enemyDeathSound;

    public AudioClip playerDeathSound;

    public AudioClip itemPickupSound;

    public AudioClip levelCompleteSound;

    public float defaultVolume = 1f;

    public static RogueAudioManager instance { get; private set; }


    public static System.Action OnPlayerHitSoundRequested;
    public static System.Action OnEnemyHitSoundRequested;
    public static System.Action OnEnemyDeathSoundRequested;
    public static System.Action OnPlayerDeathSoundRequested;
    public static System.Action OnItemPickupSoundRequested;
    public static System.Action OnLevelCompleteSoundRequested;
    public static System.Action<float> OnEnemyPresentSoundRequested;



    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        audioSource = this.gameObject.AddComponent<AudioSource>();

        OnPlayerDeathSoundRequested += PlayPlayerDeathSound;
        OnPlayerHitSoundRequested += PlayPlayerHitSound;
        OnEnemyHitSoundRequested += PlayEnemyHitSound;
        OnEnemyDeathSoundRequested += PlayEnemyDeathSound;
        OnItemPickupSoundRequested += PlayItemPickupSound;
        OnEnemyPresentSoundRequested += PlayEnemyPresentSound;
        OnLevelCompleteSoundRequested += PlayLevelCompleteSound;

    }

    void OnDestroy()
    {
        OnPlayerDeathSoundRequested -= PlayPlayerDeathSound;
        OnPlayerHitSoundRequested -= PlayPlayerHitSound;
        OnEnemyHitSoundRequested -= PlayEnemyHitSound;
        OnEnemyDeathSoundRequested -= PlayEnemyDeathSound;
        OnItemPickupSoundRequested -= PlayItemPickupSound;
        OnEnemyPresentSoundRequested -= PlayEnemyPresentSound;
        OnLevelCompleteSoundRequested -= PlayLevelCompleteSound;
    }


    public void PlayPlayerHitSound()
    {
        if (playerHitSound != null)
        {
            audioSource.PlayOneShot(playerHitSound, 0.5f);
        }
    }
    public void PlayEnemyHitSound()
    {
        if (enemyHitSound != null)
        {
            audioSource.PlayOneShot(enemyHitSound);
        }
    }
    public void PlayEnemyDeathSound()
    {
        if (enemyDeathSound != null)
        {
            audioSource.PlayOneShot(enemyDeathSound);
        }
    }
    public void PlayPlayerDeathSound()
    {
        if (playerDeathSound != null)
        {
            audioSource.PlayOneShot(playerDeathSound);
        }
    }
    public void PlayItemPickupSound()
    {
        if (itemPickupSound != null)
        {
            audioSource.PlayOneShot(itemPickupSound);
        }
    }
    public void PlayEnemyPresentSound(float volume)
    {
        if (enemyPresentSound != null)
        {
            audioSource.PlayOneShot(enemyPresentSound, volume);
        }
    }
    public void PlayLevelCompleteSound()
    {
        if (levelCompleteSound != null)
        {
            audioSource.PlayOneShot(levelCompleteSound);
        }
    }
}