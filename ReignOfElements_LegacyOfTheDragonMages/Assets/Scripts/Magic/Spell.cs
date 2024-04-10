using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpellType;

public class Spell : MonoBehaviour
{

    [SerializeField] int damage;
    [SerializeField] float lifeTime;    
    [SerializeField] LayerMask damagingObjects;
    CircleCollider2D colliderRadius;
    public Type type;
    public int castSpeed;
    public Caster currentCaster;
    public enum Caster { Player, Enemy }

    private void Awake()
    {
        SelectSpellType();
        colliderRadius = GetComponent<CircleCollider2D>();
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

    void Dissolve()
    {
        Destroy(this.gameObject, lifeTime);
    }

    public void AreaDamage()
    {
        Collider2D[] objectsInRange = Physics2D.OverlapCircleAll(transform.position, colliderRadius.radius, damagingObjects);
        if (objectsInRange.Length > 0)
        {
            foreach (Collider2D obj in objectsInRange)
            {
                Health enemyHealth = obj.GetComponent<Health>();
                if (enemyHealth != null) 
                {
                    enemyHealth.Damage(damage, type);
                }
            }
        }        
    }
}
