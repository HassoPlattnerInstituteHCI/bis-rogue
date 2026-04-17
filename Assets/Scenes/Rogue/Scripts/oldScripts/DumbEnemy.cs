using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DumbEnemy : MonoBehaviour
{
    [SerializeField]
    [Range(0.0f, 10.0f)]
    public float speed = 5f;

    [SerializeField]
    public GameObject player;

    private Vector3 lastPlayerPosition = Vector3.zero;

    void Start()
    {
        
    }

    void Update()
    {
        MoveEnemy();
    }
    

    // move enemy towards player
    private void MoveEnemy()
    {
        if (player.transform.position == lastPlayerPosition)
        {
            return; // Player is not moving
        }

        lastPlayerPosition = player.transform.position;

        Vector3 direction = (lastPlayerPosition - this.transform.position).normalized;
        float dt = Time.deltaTime;

        Vector3 nextPos = this.transform.position + direction * speed * dt;
        this.transform.position = new Vector3(nextPos.x, 0, nextPos.z);
    }
}


