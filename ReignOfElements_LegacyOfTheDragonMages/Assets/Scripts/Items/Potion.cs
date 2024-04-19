using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : Collectible
{
    Health playerHealth;
    Mana playerMana;

    private void Start()
    {
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
        playerMana = GameObject.FindGameObjectWithTag("Player").GetComponent<Mana>();
    }

    protected override void Give()
    {
        if(playerInventory != null)
        {
            playerInventory.CollectPotion(gameObject);
        }
    }

    public void Use(string potion)
    {
        if(potion == "Health Potion")
        {
            if (playerInventory != null)
            {
                playerInventory.health--;
                if(playerHealth != null)
                {
                    playerHealth.currentHealth += 10;
                }
            }
        }
        else if (potion == "Health Potion")
        {
            if (playerInventory != null)
            {
                playerInventory.health--;
                if (playerMana != null)
                {
                    playerMana.currentMana += 10;
                }
            }
        }
    }

    protected override void Destroy()
    {
        base.Destroy();
    }
}
