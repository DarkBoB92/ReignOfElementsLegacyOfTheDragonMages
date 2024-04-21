using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpellType;

public class Spell : MonoBehaviour
{
    [Header("Spell Variables")]
    [SerializeField] int damage;
    [SerializeField] float lifeTime;      
    public Type type;
    public int castSpeed;

    [Header("Spell Caster")]
    public Caster currentCaster;

    [Header("Damageble Layers By AOE Spells")]
    [SerializeField] LayerMask damagingObjects;
    CircleCollider2D colliderRadius;

    [Header("Reference Variables")]
    Door collidingDoor;
    Altar collidingAltar;

    public enum Caster { Player, Enemy }

    // On Awake() execute SelectSpellType() and reference the Collider of the spell
    private void Awake()
    {
        SelectSpellType();
        colliderRadius = GetComponent<CircleCollider2D>();
    }

    // On Update execute Dissolve()
    private void Update()
    {        
        Dissolve();
    }   

    // On trigger enter with the collider it executes the Correct methods depending on the ollided object Tag
    private void OnTriggerEnter2D(Collider2D collision)
    {        
        if (collision.CompareTag("Wall"))
        {
            Destroy(this.gameObject);
        }
        else if (collision.CompareTag("Enemy") && currentCaster == Caster.Player)
        {
            if (type == Type.Fire || type == Type.Water)
            {
                if (colliderRadius != null)
                {
                    colliderRadius.radius *= 5;
                }
                AreaDamage();
                Destroy(this.gameObject);
            }
            else if (type == Type.Normal || type == Type.Earth)
            {
                collision.GetComponent<Health>().Damage(damage, type);
                Destroy(this.gameObject);
            }            
        }
        else if (collision.CompareTag("Player") && currentCaster == Caster.Enemy)
        {
            if (type == Type.Fire || type == Type.Water)
            {
                if (colliderRadius != null)
                {
                    colliderRadius.radius *= 5;
                }
                AreaDamage();
                Destroy(this.gameObject);
            }
        }
        else if (collision.CompareTag("Door") && currentCaster == Caster.Player)
        {            
            collidingDoor = collision.GetComponent<Door>();
            if (collidingDoor != null)
            {
                collidingDoor.OpenDoor(type);
            }
            Destroy(this.gameObject);
        }
        else if (collision.CompareTag("Altar"))
        {
            collidingAltar = collision.GetComponent<Altar>();
            if(collidingAltar != null)
            {
                collidingAltar.ActivateAltar(type);
            }
        }
    }

    // On execution it assigns the spell type depending on the gameobject Tag
    void SelectSpellType()
    {
        switch(gameObject.tag)
        {
            case "Normal":
                type = Type.Normal; 
                break;
            case "Fire":
                type = Type.Fire;
                break;
            case "Water":
                type = Type.Water;
                break;
            case "Earth":
                type = Type.Earth;
                break;
        }
    }

    // On execution it destroys the spell gameobject after its lifeTime.
    void Dissolve()
    {
        Destroy(this.gameObject, lifeTime);
    }

    // On execution it checks every collider that overlaps its collider and it damages every object that has the Health script attached
    public void AreaDamage()
    {
        Collider2D[] objectsInRange = Physics2D.OverlapCircleAll(transform.position, colliderRadius.radius, damagingObjects);
        if (objectsInRange.Length > 0)
        {
            foreach (Collider2D objects in objectsInRange)
            {
                Health health = objects.GetComponent<Health>();
                if (health != null) 
                {
                    health.Damage(damage, type);
                }
            }
        }        
    }
}
