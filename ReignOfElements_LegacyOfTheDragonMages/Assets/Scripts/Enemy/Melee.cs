using SpellType;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : Enemy
{
    //TODO: Give an Attack Range to melee enemy (big enough to fit animation)
    //      Do the override methods to attack
    
    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        if (currentState == State.Attack)
        {
            chasingTime += Time.deltaTime;
        }
        CheckInSightDistance();
        Move();
    }

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

    IEnumerator Damaging(int damage, Type damageType)
    {
        while (attacking)
        {
            playerHealth.Damage(damage, damageType);
            yield return new WaitForSeconds(attackDelay);
        }
    }
}
