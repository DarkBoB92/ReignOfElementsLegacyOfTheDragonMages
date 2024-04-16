using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : Collectible
{
    protected override void Give()
    {
        if(playerInventory != null)
        {
            playerInventory.CollectKey();
        }
    }

    protected override void Destroy()
    {
        base.Destroy();
    }
}
