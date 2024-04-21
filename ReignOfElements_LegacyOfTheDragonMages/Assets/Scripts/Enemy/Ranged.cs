using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpellType;

public class Ranged : Enemy
{
    [Header("Ranged General Variables")]
    [SerializeField] GameObject spell;

    // On Start() sets boolean letting the code know of its type
    private void Start()
    {
        melee = false;
        ranged = true;
    }

    // This Update just copies the Father class
    void Update()
    {
        patrolTime += Time.deltaTime;
        if (currentState == State.Attack)
        {
            chasingTime += Time.deltaTime;
        }
        if (player != null)
        {
            CheckInSightDistance();
            Move();
        }
    }    

    // Attack method just starts a coroutine to cast and set attack to true
    protected override void Attack()
    {
        if (!attacking)
        {
            attacking = true;
            StartCoroutine(Casting(spell));                       
        }
    }

    // This Coroutine just handles casting with using an atack delay
    IEnumerator Casting(GameObject castingSpell)
    {
        while (attacking)
        {
            Cast(castingSpell);
            yield return new WaitForSeconds(attackDelay);
        }
    }

    // This Method instantiate a spell object in the direction of the player
    void Cast(GameObject castSpell)
    {        
        if (player != null)
        {
            Vector2 directionToCast = player.transform.position - transform.position;
            GameObject castedSpell = Instantiate(castSpell, transform.position, Quaternion.identity);
            int castSpeed = castedSpell.GetComponent<Spell>().castSpeed;
            castedSpell.GetComponent<Spell>().currentCaster = Spell.Caster.Enemy;
            castedSpell.GetComponent<Rigidbody2D>().velocity = directionToCast.normalized * castSpeed;
        }
    }    
}
