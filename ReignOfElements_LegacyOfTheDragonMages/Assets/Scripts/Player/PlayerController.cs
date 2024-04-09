
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] int speed;
    [SerializeField] GameObject selectedSpell;
    [SerializeField] GameObject[] spell;
    Rigidbody2D rb;
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
        GameObject castedSpeel = Instantiate(currentSpell, transform.position + transform.right, Quaternion.identity);
        castedSpeel.GetComponent<Rigidbody2D>().velocity = transform.right * speed;
    }

    void ChangeSpell()
    {
        switch(selectedSpell)
        {
            //TODO: - Check Spell State and change to the next one
        }
    }
}
