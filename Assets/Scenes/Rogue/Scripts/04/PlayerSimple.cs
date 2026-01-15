using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSimple : MonoBehaviour
{

    public int playerHealth = 5;
    
    
    void Start()
    {
        
    }
    void Update()
    {
        
    }
   

    // Handles collisions with enemies, food items, and finish point
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Player collided with: " + collision.gameObject.name);
        // Attack enemy if cooldown has elapsed
        if (collision.gameObject.CompareTag("Food"))
        {
            Heal();

            Destroy(collision.gameObject.transform.parent.gameObject);
        }
    }

    private void Heal()
    {
       // Implement healing logic here
       playerHealth++;
    }

}
