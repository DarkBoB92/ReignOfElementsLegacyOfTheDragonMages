using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : Collectible
{
    protected override void Give()
    {
        if(playerInventory != null)
        {
            playerInventory.CollectPotion(gameObject);
        }
    }    

    protected override void Destroy()
    {
        Destroy(this.gameObject);
    }
}
