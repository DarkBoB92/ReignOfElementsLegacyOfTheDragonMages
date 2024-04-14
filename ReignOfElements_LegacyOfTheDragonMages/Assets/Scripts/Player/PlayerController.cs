using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using SpellType;

public class PlayerController : MonoBehaviour
{
    [Header("Player Movement Speed")]
    [SerializeField] int speed;

    [Header("Spell Variables")]
    [SerializeField] GameObject selectedSpell;
    [SerializeField] GameObject[] spell; // List of Spells: 0 = Normal, 1 = Fire, 2 = Water, 3 = Earth

    [Header("Player Element Affinity")]
    [SerializeField] public Type transformation;

    [Header("Referenced Variables")]
    Rigidbody2D rb;
    Health health;

    // On Start() get references and set default Element Affinity and Used Spell
    void Start()
    {
        transformation = Type.Normal;
        rb = GetComponent<Rigidbody2D>();
        health = GetComponent<Health>();
        selectedSpell = spell[0];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Gets input from InputSystem. Checks if Player is wet or stunned and changes movement accordingly
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
        }        
        //TODO: - Handle animation depending on direction of where the player is moving
    }

    // Gets input from InputSystem. Checks if input magnitude value is greater than 0 to execute CastSpell(GameObject x, Vector2 y)
    void OnFire(InputValue input)
    {
        Vector2 xyInput = input.Get<Vector2>();
        if (xyInput.magnitude > 0)
        {
            CastSpell(selectedSpell, xyInput);
        }
        
        //TODO: - Check which direction is walking or last direction pointing so casting will be set on right direction
    }

    // Gets input from InputSystem. Executes ChangeSpell() on press.
    void OnTransform(InputValue input)
    {
        if (input.isPressed)
        {
            ChangeSpell();
        }
    }

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
        switch(transformation)
        {
            case Type.Normal:
                transformation = Type.Fire;
                selectedSpell = spell[1];
                break;
            case Type.Fire:
                transformation = Type.Water;
                selectedSpell = spell[2];
                break;
            case Type.Water:
                transformation = Type.Earth;
                selectedSpell = spell[3];
                break;
            case Type.Earth:
                transformation = Type.Normal;
                selectedSpell = spell[0];
                break;
        }
    }
}
