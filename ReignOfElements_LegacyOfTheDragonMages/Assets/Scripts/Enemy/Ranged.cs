using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ranged : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
