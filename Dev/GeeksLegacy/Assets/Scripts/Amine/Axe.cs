using System.Collections.Generic;
using System;
using UnityEngine;

public class Axe : Item
{


    public Axe()
    {
        itemName = "Pickaxe";
        itemType = "Tool";
        itemDurability = 70.0f;
        recipe = new Dictionary<string, int>
        {
            { "Wood", 7 },
            { "Iron", 5 }
        };
    }

}
