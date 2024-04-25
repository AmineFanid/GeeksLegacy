using System.Collections.Generic;
using UnityEngine;


public class Sword : Item
{


    public Sword()
    {
        itemName = "Sword";
        itemType = "Weapon";
        itemDurability = 150.0f;
        recipe = new Dictionary<string, int>
        {
            { "Wood", 10 },
            { "Iron", 5 }
        };
    }

}
