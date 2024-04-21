using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Collectible : MonoBehaviour
{
    protected Inventory playerInventory;

    // When it collides with the Player it gives something to the player and destroys the collectible
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerInventory = collision.gameObject.GetComponent<Inventory>();
            if (playerInventory != null)
            {
                Give();
                Destroy();
            }
        }
    }

    // This method is overridble because every single collectible will give different things
    protected abstract void Give();
    // This method is overridble because every single collectible will have a different way of disappearing
    protected abstract void Destroy();
}
