using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpellType;

public class Ranged : Enemy
{
    [SerializeField] GameObject spell;

    private void Start()
    {
        melee = false;
        ranged = true;
    }

    void Update()
    {
        currentTime += Time.deltaTime;
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

    protected override void Attack()
    {
        if (!attacking)
        {
            attacking = true;
            StartCoroutine(Casting(spell));                       
        }
    }
    IEnumerator Casting(GameObject castingSpell)
    {
        while (attacking)
        {
            Cast(castingSpell);
            yield return new WaitForSeconds(attackDelay);
        }
    }

    void Cast(GameObject castSpell)
    {
        Debug.Log("Casting from Enemy");
        Vector2 directionToCast = player.transform.position - transform.position;
        GameObject castedSpell = Instantiate(castSpell, transform.position, Quaternion.identity);
        int castSpeed = castedSpell.GetComponent<Spell>().castSpeed;
        castedSpell.GetComponent<Spell>().currentCaster = Spell.Caster.Enemy;
        castedSpell.GetComponent<Rigidbody2D>().velocity = directionToCast.normalized * castSpeed;
    }    
}
