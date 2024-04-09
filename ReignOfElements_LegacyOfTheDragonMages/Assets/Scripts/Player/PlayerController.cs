using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] int speed, testTypeSelection;
    [SerializeField] GameObject selectedSpell;
    [SerializeField] GameObject[] spell;
    public SpellType.Type transformation;
    Rigidbody2D rb;

    //public enum TransformType { Normal, Fire, Water, Earth };
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
            case SpellType.Type.Normal:
                transformation = SpellType.Type.Fire;
                selectedSpell = spell[1];
                break;
            case SpellType.Type.Fire:
                transformation = SpellType.Type.Water;
                selectedSpell = spell[2];
                break;
            case SpellType.Type.Water:
                transformation = SpellType.Type.Earth;
                selectedSpell = spell[3];
                break;
            case SpellType.Type.Earth:
                transformation = SpellType.Type.Normal;
                selectedSpell = spell[0];
                break;
        }
    }
}
