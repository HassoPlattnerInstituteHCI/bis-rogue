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

    // TODO: implement tts for room introduction text
    async void IntroductionSpeech()
    {
        // TODO: create a new SpeechOut object.
        //var speechOut = ; //uncomment and complete this line
        
        // TODO: use the Speak method of the SpeechOut object to speak the introductionText.
        //await  ; // uncomment and complete this line to await the speech synthesis
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
