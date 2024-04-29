using System.Collections.Generic;
using System;
using UnityEngine;

public class Axe : Item
{
    public float damage;

    public Axe()
    { 

    }
}

public class WoodenAxe : Axe
{


    public WoodenAxe()
    {
        itemName = "WoodenAxe";
        itemType = "Tool";
        itemDurability = 25.0f;
        damage = 7.0f;
        recipe = new Dictionary<string, int>
        {
            { "Wood", 15 }
        };
        damage = 5.0f;
    }

    public override float DoDamage()
    {
        return damage;
    }

}

public class IronAxe : Axe
{


    public IronAxe()
    {
        itemName = "IronAxe";
        itemType = "Tool";
        itemDurability = 100.0f;
        damage = 14.0f;
        recipe = new Dictionary<string, int>
        {
            { "Wood", 15 },
            { "Iron", 15 }
        };
        damage = 10.0f;
    }

    public override float DoDamage()
    {
        return damage;
    }

}



