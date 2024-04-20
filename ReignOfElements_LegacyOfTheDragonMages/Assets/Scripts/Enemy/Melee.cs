using SpellType;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : Enemy
{
    [Header("Melee General Variables")]
    [SerializeField] int damage;

    // On Start() sets boolean letting the code know of its type
    private void Start()
    {
        melee = true;
        ranged = false;
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

    // This method is an override and that starts a Corotuine of damage depending on the gameobject name
    protected override void Attack()
    {
        if (!attacking)
        {
            attacking = true;
            if (gameObject.name == "Skeleton")
            {
                StartCoroutine(Damaging(damage, Type.None));
            }
            else if (gameObject.name == "Minotaur")
            {
                StartCoroutine(Damaging(damage, Type.Earth));
            }
        }
    }

    // This Coroutine just handles damage with using an atack delay
    IEnumerator Damaging(int damage, Type damageType)
    {        
        while (attacking)
        {
            if (playerHealth != null)
            {
                playerHealth.Damage(damage, damageType);
            }
            yield return new WaitForSeconds(attackDelay);
            if (player == null)
            {
                attacking = false;
            }
        }
    }
}
