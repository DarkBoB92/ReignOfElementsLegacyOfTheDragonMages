using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpellType;

public class Inventory : MonoBehaviour
{
    [Header("Key Check")]
    public bool hasKey;

    [Header("Power Gems")]
    public bool fire;
    public bool water;
    public bool earth;

    [Header("Potions")]
    public int health;
    public int mana;

    [Header("Player Variables")]
    Health playerHealth;
    Mana playerMana;

    private void Start()
    {
        playerHealth = GetComponent<Health>();
        playerMana = GetComponent<Mana>();
    }

    public void CollectKey()
    {
        hasKey = true;
    }

    // Sets the according variable true depending on the Type of the collected power gem
    public void CollectPowerGem(Type gemType)
    {
        switch(gemType)
        {
            case Type.Fire:
                fire = true;
                break;
            case Type.Water:
                water = true;
                break;
            case Type.Earth:
                earth = true;
                break;
        }
    }

    // Increases the value of the relative collected object tag
    public void CollectPotion(GameObject potion)
    {
        if (potion.CompareTag("HealthPotion"))
        {
            health++;
        }
        if (potion.CompareTag("ManaPotion"))
        {
            mana++;
        }
    }
    public void UsePotion(string potion)
    {
        if (potion == "HealthPotion")
        {
            if (playerHealth != null)
            {
                if (playerHealth.currentHealth < playerHealth.GetMaxHealth())
                {
                    health--;
                    playerHealth.currentHealth += 50;
                    if(playerHealth.currentHealth >= playerHealth.GetMaxHealth())
                    {
                        playerHealth.currentHealth = playerHealth.GetMaxHealth();
                    }
                }
                else
                {
                    Debug.Log("You're at Max Health");
                }
            }

        }
        else if (potion == "ManaPotion")
        {
            if (playerMana != null)
            {
                if (playerMana.currentMana < playerMana.GetMaxMana())
                {
                    mana--;
                    playerMana.currentMana += 50;
                    if (playerMana.currentMana >= playerMana.GetMaxMana())
                    {
                        playerMana.currentMana = playerMana.GetMaxMana();
                    }
                }
                else
                {
                    Debug.Log("You're at Max Mana");
                }
            }
        }
    }
}
