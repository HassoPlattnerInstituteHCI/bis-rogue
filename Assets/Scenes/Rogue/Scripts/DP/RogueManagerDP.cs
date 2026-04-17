using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DualPantoToolkit;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEditor;

public class RogueManagerDP : MonoBehaviour
{

    private UpperHandle upperHandle;
    private LowerHandle lowerHandle;


    private UnityEngine.Vector3 spawnPositionUpperHandle = new UnityEngine.Vector3(0, 0, 0);
    private UnityEngine.Vector3 spawnPositionLowerHandle = new UnityEngine.Vector3(0, 0, 0);

    private List<PantoCollider> pantoColliders = new List<PantoCollider>();

    void Start()
    {

        upperHandle = GameObject.Find("Panto").GetComponent<UpperHandle>();
        lowerHandle = GameObject.Find("Panto").GetComponent<LowerHandle>();



        Callibrate();
    }

    void Callibrate()
    {
        upperHandle.GetPosition();
        lowerHandle.GetPosition();
    }

    public async Task LevelFinished()
    {
        EditorApplication.isPlaying = false;

        // FindPantoColliders();
        // DisableCollider();
        // await Task.Delay(2000);
        // await TransformPlayerToSpawn();
        // await PlayerReachedSpawn();
        // EnableCollider();
    }

    public async Task PlayerReachedSpawn()
    {
        EnableCollider();
        var gridManager = GameObject.FindObjectOfType<GridRoomSpawner>();
        if (gridManager != null)
        {
            gridManager.Reset();
            gridManager.Create();
        }

        var roomManager = GameObject.FindObjectOfType<RoomManager>();
        if (roomManager != null)
        {
            roomManager.Reset();
            await roomManager.Create();
        }
    }

    async Task TransformPlayerToSpawn()
    {
        if (upperHandle != null && lowerHandle != null)
        {
            await upperHandle.MoveToPosition(spawnPositionUpperHandle, 100f);
            await lowerHandle.MoveToPosition(spawnPositionLowerHandle, 100f);
        }
        
    }

    private void FindPantoColliders()
    {
        pantoColliders.Clear();
        PantoCollider[] colliders = GameObject.FindObjectsOfType<PantoCollider>();
        foreach (PantoCollider collider in colliders)
        {
            pantoColliders.Add(collider);
        }
    }

    private void EnableCollider()
    {
        foreach (PantoCollider collider in pantoColliders)
        {
            collider.Enable();
        }
    }

    private async Task DisableCollider()
    {
        foreach (PantoCollider collider in pantoColliders)
        {
            collider.Disable();
            await Task.Delay(10);
        }
    }

}
