using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{
    //TODO: - Father Class for spells
    //      - Enum states for selecting spell
    //      - Does Damage: Different Class manages the damage, make this override method
    //      - Open Doors: Think of tasks way to do it
    //      - Destroy Itself
    [SerializeField] int damage;
    [SerializeField] float lifeTime;
    [SerializeField] SpellType type;
    public int castSpeed;
    public enum SpellType {Normal, Fire, Water, Earth};

    private void Awake()
    {
        SelectSpellType();
    }

    private void Update()
    {        
        Dissolve();
    }   

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall"))
        {
            Destroy(this.gameObject);
        }
        else if (collision.CompareTag("Enemy"))
        {
            //TODO: Do damage
            Destroy(this.gameObject);
        }
        else if (collision.CompareTag("Door"))
        {
            //TODO: - Check if right spell state to open door
            //      - If right spell, Open door and destroy door
            Destroy(collision.gameObject);
            Destroy(this.gameObject);
        }
    }

    void SelectSpellType()
    {
        switch(gameObject.tag)
        {
            case "Normal":
                type = SpellType.Normal; 
                break;
            case "Fire":
                type = SpellType.Fire;
                break;
            case "Water":
                type = SpellType.Water;
                break;
            case "Earth":
                type = SpellType.Earth;
                break;
        }
    }

    void Dissolve()
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
