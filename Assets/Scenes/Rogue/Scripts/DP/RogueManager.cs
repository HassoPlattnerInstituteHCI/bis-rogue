using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DualPantoToolkit;

public class RogueManager : MonoBehaviour
{

    [SerializeField]
    [Range(0, 10)]
    public int playerHealth = 5;

    [SerializeField]
    [Range(0, 10)]
    public int playerAC = 3;



    void Start()
    {
        
    }

    
    public void LevelFinished()
    {
        UnityEditor.EditorApplication.isPlaying = false;
    }
    public void PlayerHit()
    {
        playerHealth--;
        Debug.Log("Player Health: " + playerHealth);
        if (playerHealth <= 0)
        {
            Debug.Log("Player has died!");
            // Handle player death (e.g., end game, respawn, etc.)
        }
    }
}