using System.Collections.Generic;
using System;
using UnityEngine;

public class Pickaxe : Item
{


    public Pickaxe()
    {
        itemName = "Pickaxe";
        itemType = "Tool";
        itemDurability = 50.0f;
        recipe = new Dictionary<string, int>
        {
            { "Wood", 5 },
            { "Iron", 3 }
        };
    }

}
