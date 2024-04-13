using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpellType;

public class Ranged : Enemy
{
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
            if (gameObject.name == "Goblin")
            {
                StartCoroutine(Casting(damage, Type.Fire));
            }
            else if (gameObject.name == "Orc")
            {
                StartCoroutine(Casting(damage, Type.Water));
            }
        }
    }
    IEnumerator Casting(int damage, Type damageType)
    {
        while (attacking)
        {
            Cast(damage, damageType);
            yield return new WaitForSeconds(attackDelay);
        }
    }

    void Cast(int damage, Type damageType)
    {
        //playerHealth.Damage(damage, damageType);
    }

    //TODO: Useful for Caster Attack in override method
    //      Try to figure out propper aiming direction
    //firingCurrentTime += Time.deltaTime;
    //if (firingCurrentTime >= fireRate)
    //{
    //    weapon.FireWeapon();
    //    firingCurrentTime = 0;
    //}
    //if (currentTime >= timeDelay)
    //{
    //    state = State.Idle;
    //    firingCurrentTime = 0;
    //    currentTime = 0;
    //}
    // Needed while casting
    // castedSpell.GetComponent<Spell>().currentCaster = Spell.Caster.Enemy;
}
