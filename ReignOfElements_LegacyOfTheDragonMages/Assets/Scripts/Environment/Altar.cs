using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpellType;

public class Altar : MonoBehaviour
{
    [Header("Altar Variables")]
    [SerializeField] Type altarElement;
    [SerializeField] bool activated;
    [SerializeField] SpriteRenderer gem;
    [SerializeField] GameObject shinyGemParticle;

    // On Start() it sets the door element type
    void Start()
    {
        activated = false;
        SelectAltarType();
    }

    public void ActivateAltar(Type collidingSpell)
    {        
        if(collidingSpell == altarElement)
        {
            activated = true;
            StartCoroutine(ActivateGem());
            shinyGemParticle.SetActive(true);
        }
        
    }

    IEnumerator ActivateGem()
    {        
        Color gemColor = gem.color;
        while(gemColor.a <= 1)
        {
            Debug.Log("UwU");
            gemColor.a += 0.3f;
            gemColor = gem.color;
            yield return new WaitForSeconds(0.1f);
        }        
    }

    // On execution it assigns the door element type depending on the gameobject Name
    void SelectAltarType()
    {
        switch (gameObject.name)
        {
            case "FireAltar":
                altarElement = Type.Fire;
                break;
            case "WaterAltar":
                altarElement = Type.Water;
                break;
            case "EarthAltar":
                altarElement = Type.Earth;
                break;
        }
    }
}