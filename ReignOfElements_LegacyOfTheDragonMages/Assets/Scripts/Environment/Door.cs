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
        if(collidingSpell == Type.None) // && Player hasKey
        {
            Destroy(gameObject);
        }
        else if (collidingSpell == Type.Normal && doorElement == Type.Normal)
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

    // On execution it assigns the door element type depending on the gameobject Name
    void SelectDoorType()
    {
        switch (gameObject.name)
        {
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
