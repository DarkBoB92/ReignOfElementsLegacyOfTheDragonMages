
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] int speed, testTypeSelection;
    [SerializeField] GameObject selectedSpell;
    [SerializeField] GameObject[] spell;
    [SerializeField] SpellType transformation;
    Rigidbody2D rb;

    public enum SpellType { Normal, Fire, Water, Earth };
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        selectedSpell = spell[0];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMove(InputValue input)
    {
        Vector2 xyInput = input.Get<Vector2>();
        rb.velocity = xyInput * speed;
        //TODO: - Handle animation depending on direction of where the player is moving
        //      - Add checks to hold last movement
    }

    void OnFire(InputValue input)
    {
        if(input.isPressed)
        {
            CastSpell(selectedSpell);
        }
        //TODO: - Check which direction is walking or last direction pointing so casting will be set on right direction
    }

    void OnTransform(InputValue input)
    {
        if (input.isPressed)
        {
            ChangeSpell();
        }
    }

    void CastSpell(GameObject currentSpell)
    {
        GameObject castedSpell = Instantiate(currentSpell, transform.position + transform.right, Quaternion.identity);
        int castSpeed = castedSpell.GetComponent<Spell>().castSpeed;
        castedSpell.GetComponent<Rigidbody2D>().velocity = transform.right * castSpeed;
    }

    void ChangeSpell()
    {
        switch(transformation)
        {
            case SpellType.Normal:
                transformation = SpellType.Fire;
                selectedSpell = spell[1];
                break;
            case SpellType.Fire:
                transformation = SpellType.Water;
                selectedSpell = spell[2];
                break;
            case SpellType.Water:
                transformation = SpellType.Earth;
                selectedSpell = spell[3];
                break;
            case SpellType.Earth:
                transformation = SpellType.Normal;
                selectedSpell = spell[0];
                break;
        }
    }
}
