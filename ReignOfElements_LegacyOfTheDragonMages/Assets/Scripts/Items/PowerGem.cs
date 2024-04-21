using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpellType;

public class PowerGem : Collectible
{
    Type gemType;

    private void Awake()
    {
        SelectGemType();
    }

    protected override void Give()
    {
        if(playerInventory != null)
        {
            playerInventory.CollectPowerGem(gemType);
        }
    }

    protected override void Destroy()
    {
        Destroy(this.gameObject);
    }

    void SelectGemType()
    {
        switch (gameObject.tag)
        {            
            case "Fire":
                gemType = Type.Fire;
                break;
            case "Water":
                gemType = Type.Water;
                break;
            case "Earth":
                gemType = Type.Earth;
                break;
        }
    }
}
