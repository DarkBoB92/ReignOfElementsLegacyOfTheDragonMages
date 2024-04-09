
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] int speed;
    [SerializeField] GameObject spellPrefab;
    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMove(InputValue input)
    {
        Vector2 xyInput = input.Get<Vector2>();
        rb.velocity = xyInput * speed;
    }

    void OnFire(InputValue input)
    {
        if(input.isPressed)
        {
            GameObject castedSpeel = Instantiate(spellPrefab, transform.position + transform.right, Quaternion.identity);
            castedSpeel.GetComponent<Rigidbody2D>().velocity = transform.right * speed;
        }
    }
}
