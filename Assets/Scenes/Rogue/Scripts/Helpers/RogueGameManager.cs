using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DualPantoToolkit;
using TMPro;
public class RogueGameManager : MonoBehaviour
{
    public PlayerSimple player;

    public TMP_Text healthText; 

    void Start()
    {
        player.OnHealthChanged += UpdateText;
        UpdateText(player.currentHealth);
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
