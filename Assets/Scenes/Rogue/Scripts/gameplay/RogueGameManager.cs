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
    public string introductionClipName;

    void Start()
    {
        player.OnHealthChanged += UpdateText;
        UpdateText(player.currentHealth);

        if (!string.IsNullOrEmpty(introductionClipName))
        {
            SoundManager.Instance.Play(introductionClipName);
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
        var speechOut = new SpeechOut(); //uncomment and complete this line
        
        // TODO: use the Speak method of the SpeechOut object to speak the introductionText.
        await speechOut.Speak(introductionText); // uncomment and complete this line to await the speech synthesis
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
