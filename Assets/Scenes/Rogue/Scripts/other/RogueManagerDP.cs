using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DualPantoToolkit;

public class RogueManagerDP : MonoBehaviour
{

    [SerializeField]
    [Range(0, 10)]
    public int playerHealth = 5;

    [SerializeField]
    [Range(0, 10)]
    public int playerAC = 3;

    [SerializeField]
    public Transform spawnPosition;

    

    private UpperHandle upperHandle;

    void Start()
    {
        
        upperHandle = GameObject.Find("Panto").GetComponent<UpperHandle>();
        Introduction();
    }

    void Introduction()
    {
        TransformPlayerToSpawn();
    }

    public void LevelFinished(){
        UnityEditor.EditorApplication.isPlaying = false;
    }

    async void TransformPlayerToSpawn()
    {
        if (upperHandle != null)
        {
            await upperHandle.MoveToPosition(spawnPosition.position, 1f);
        }
    }
    
}
