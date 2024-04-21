using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using SpellType;

public class PlayerController : MonoBehaviour
{
    [Header("Player Movement Speed")]
    [SerializeField] int speed;

    [Header("Player Position")]
    Vector2 currentPos;
    Vector2 lastPos;

    [Header("Spell Variables")]
    [SerializeField] GameObject selectedSpell;
    [SerializeField] GameObject[] spell; // List of Spells: 0 = Normal, 1 = Fire, 2 = Water, 3 = Earth

    [Header("Player Element Affinity")]
    public Type transformation;

    [Header("Player Inventory")]
    Inventory inventory;

    [Header("Referenced Variables")]
    Rigidbody2D rb;
    Health health;
    Mana mana;
    Animator animator;

    // On Start() get references and set default Element Affinity and Used Spell
    void Awake()
    {
        transformation = Type.Normal;
        rb = GetComponent<Rigidbody2D>();
        health = GetComponent<Health>();
        mana = GetComponent<Mana>();
        inventory = GetComponent<Inventory>();
        animator = GetComponent<Animator>();
        selectedSpell = spell[0];
    }

    //--------------Input System---------------------

    // Checks if Player is wet or stunned and changes movement accordingly
    void OnMove(InputValue input)
    {
        Vector2 xyInput = input.Get<Vector2>();
        if(health.wet)
        {
            rb.velocity = xyInput * (speed/2);
        }
        else if(health.stunned)
        {
            rb.velocity *= 0;
        }
        else
        {
            rb.velocity = xyInput * speed;
            if (xyInput.x != 0 || xyInput.y != 0)
            {
                currentPos = rb.velocity;
                MovementAnimationCheck();
            }
            else if(xyInput.x == 0 || xyInput.y == 0)
            {
                animator.Play("Idle");
            }
        }
    }

    // Gets input from InputSystem. Checks if input magnitude value is greater than 0 to execute CastSpell(GameObject x, Vector2 y)
    void OnFire(InputValue input)
    {
        if (!health.stunned)
        {
            Vector2 xyInput = input.Get<Vector2>();
            if (xyInput.magnitude == 1)
            {
                if (mana.CheckManaAmount())
                {
                    CastSpell(selectedSpell, xyInput);
                    AttackAnimationCheck(xyInput);
                    mana.UseMana(transformation);
                }
                else
                {
                    Debug.Log("You don't have Mana");
                }
            }
            
        }
    }

    // Gets input from InputSystem. Executes ChangeSpell() on press.
    void OnTransform(InputValue input)
    {
        if (input.isPressed)
        {
            ChangeSpell();
        }
    }

    void OnUseHealthPotion(InputValue input)
    {
        if (input.isPressed)
        {
            if (inventory.health > 0)
            {
                inventory.UsePotion("HealthPotion");
            }
            else
            {
                Debug.Log("You don't have Health Potions");
            }
        }
    }

    void OnUseManaPotion(InputValue input)
    {
        if (input.isPressed)
        {
            if (inventory.mana > 0)
            {
                inventory.UsePotion("ManaPotion");
            }
            else
            {
                Debug.Log("You don't have Mana Potions");
            }
        }
    }

    //------------------Class Methods------------------------------

    // On execution Instantiate the gameobject passed by the parameter in the direction given by the parameter 
    void CastSpell(GameObject currentSpell, Vector2 directionToCast)
    {
        GameObject castedSpell = Instantiate(currentSpell, transform.position, Quaternion.identity);
        int castSpeed = castedSpell.GetComponent<Spell>().castSpeed;
        castedSpell.GetComponent<Spell>().currentCaster = Spell.Caster.Player;
        castedSpell.GetComponent<Rigidbody2D>().velocity = directionToCast * castSpeed;
    }

    // On execution it switches between the Type of elemental affinity assigning the new affinity and changing the current equipped Spell
    void ChangeSpell()
    {
        if (inventory.fire || inventory.water || inventory.earth)
        {
            switch (transformation)
            {
                case Type.Normal:
                    if (inventory.fire)
                    {
                        transformation = Type.Fire;
                        selectedSpell = spell[1];
                    }
                    else if (inventory.water)
                    {
                        transformation = Type.Water;
                        selectedSpell = spell[2];
                    }
                    else if (inventory.earth)
                    {
                        transformation = Type.Earth;
                        selectedSpell = spell[3];
                    }
                    break;
                case Type.Fire:
                    if (!inventory.water && !inventory.earth)
                    {
                        transformation = Type.Normal;
                        selectedSpell = spell[0];
                    }
                    else if (inventory.water) 
                    { 
                        transformation = Type.Water;
                        selectedSpell = spell[2];
                    }
                    else if (inventory.earth)
                    {
                        transformation = Type.Earth;
                        selectedSpell = spell[3];
                    }
                    break;
                case Type.Water:
                    if (!inventory.fire && !inventory.earth || !inventory.earth)
                    {
                        transformation = Type.Normal;
                        selectedSpell = spell[0];
                    }
                    else if (inventory.earth)
                    {
                        transformation = Type.Earth;
                        selectedSpell = spell[3];
                    }
                    break;
                case Type.Earth:
                    transformation = Type.Normal;
                    selectedSpell = spell[0];
                    break;
            }
        }
        else
        {
            // UI message that you don't have any power gem
            Debug.Log("UwU");
        }
    }

    void MovementAnimationCheck()
    {
        Vector2 deltaPosition = currentPos - lastPos;

        if (deltaPosition.magnitude > 0.01f)
        {
            if (Mathf.Abs(deltaPosition.x) > Mathf.Abs(deltaPosition.y))
            {
                if (deltaPosition.x > 0)
                {
                    animator.Play("WalkRight");
                }
                else if (deltaPosition.x < 0)
                {
                    animator.Play("WalkLeft");
                }
            }
            else
            {
                if (deltaPosition.y > 0)
                {
                    animator.Play("WalkUp");
                }
                else if (deltaPosition.y < 0)
                {
                    animator.Play("WalkDown");
                }
            }
        }
        lastPos = currentPos;
    }

    void AttackAnimationCheck(Vector2 directionToShoot)
    {
        if(directionToShoot.x > 0)
        {
            animator.Play("AttackRight");
        }
        else if (directionToShoot.x < 0)
        {
            animator.Play("AttackLeft");
        }
        else if (directionToShoot.y > 0)
        {
            animator.Play("AttackUp");
        }
        else if (directionToShoot.y < 0)
        {
            animator.Play("AttackDown");
        }
    }

}
