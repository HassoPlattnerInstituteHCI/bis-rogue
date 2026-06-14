using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DualPantoToolkit;
using TMPro;
using SpeechIO;
public class RogueGameManager : MonoBehaviour
{
    public PlayerSimple player;

    public TMP_Text healthText; 

    [Multiline]
    [Tooltip("Text to be spoken when starting the game.")]
    public string introductionText;

    [Tooltip("When both AudioClip and introductionText are set, the AudioClip will be played.")]
    public AudioClip introductionClip;

    void Start()
    {
        player.OnHealthChanged += UpdateText;
        UpdateText(player.currentHealth);

        if (introductionClip != null)
        {
            SoundManager.Instance.Play(introductionClip);
        }
        else if (!string.IsNullOrEmpty(introductionText))
        {
            IntroductionSpeech();
        }
    }

    // TODO: 
    async void IntroductionSpeech()
    {
        SpeechOut speechOut = new SpeechOut();
        //await speechOut.Speak(introductionText);
    }

    public void LevelFinished()
    {
        UnityEditor.EditorApplication.isPlaying = false;
    }
    
    private void UpdateText(int current)
    {
        healthText.text = $"HP: {current}";
    }

    private void OnDestroy()
    {
        player.OnHealthChanged -= UpdateText;
    }
}
