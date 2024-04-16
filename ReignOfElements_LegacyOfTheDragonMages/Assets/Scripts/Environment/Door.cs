using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpellType;

public class Door : MonoBehaviour
{
    [Header("Door Variables")]
    [SerializeField] Type doorElement;

    // On Start() it sets the door element type
    void Start()
    {
        SelectDoorType();
    }
    
    // On execution it checks if the colliding Spell matches the element of the door to Open
    public void OpenDoor(Type collidingSpell)
    {
        if (collidingSpell == Type.Normal && doorElement == Type.Normal)
        {
            Destroy(gameObject);
        }
        else if (collidingSpell == Type.Fire && doorElement == Type.Water)
        {
            Destroy(gameObject);
        }
        else if (collidingSpell == Type.Water && doorElement == Type.Earth)
        {
            Destroy(gameObject);
        }
        else if (collidingSpell == Type.Earth && doorElement == Type.Fire)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (doorElement == Type.None)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                Inventory playerInventory = collision.gameObject.GetComponent<Inventory>();
                if (playerInventory != null)
                {
                    if(playerInventory.hasKey)
                    {
                        Destroy(gameObject);
                        playerInventory.hasKey = false;
                    }
                    else
                    {
                        // UI message you don't have the key
                        Debug.Log("UwU");
                    }
                }
                
            }
        }
    }

    // On execution it assigns the door element type depending on the gameobject Name
    void SelectDoorType()
    {
        switch (gameObject.name)
        {
            case "WoodDoor":
                doorElement = Type.None;
                break;
            case "Door":
                doorElement = Type.Normal;
                break;
            case "FireDoor":
                doorElement = Type.Fire;
                break;
            case "WaterDoor":
                doorElement = Type.Water;
                break;
            case "EarthDoor":
                doorElement = Type.Earth;
                break;
        }
    }
}
