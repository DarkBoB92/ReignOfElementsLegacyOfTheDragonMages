using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : Collectible
{
    protected override void Give()
    {
        if(playerInventory != null)
        {
            if (!playerInventory.hasKey)
            {
                playerInventory.CollectKey();
            }
        }
    }

    protected override void Destroy()
    {
        if (playerInventory != null)
        {
            if (playerInventory.hasKey)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
