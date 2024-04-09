using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{
    //TODO: - Does Damage: Different Class manages the damage, make this override method

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
            //TODO: Do damage get enemy health component pass to damage to give to the other methods
            Damage();
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

    void Damage()
    {
        switch(type)
        {
            case SpellType.Normal:
                NormalDamage(damage);
                break;
            case SpellType.Fire:
                FireDamage(damage);
                break;
            case SpellType.Water:
                WaterDamage(damage);
                break;
            case SpellType.Earth:
                EarthDamage(damage);
                break;
        }
    }

    void NormalDamage(int damageAmount) 
    {
        //TODO: does simple single damage
    }
    void FireDamage(int damageAmount)
    {
        //TODO: does less AOE damage but over time because burned
    }
    void WaterDamage(int damageAmount)
    {
        //TODO: AOE damage and slows down
    }
    void EarthDamage(int damageAmount)
    {
        //TODO: slow but high single damage
    }


    void Dissolve()
    {
        Destroy(this.gameObject, lifeTime);
    }
}
