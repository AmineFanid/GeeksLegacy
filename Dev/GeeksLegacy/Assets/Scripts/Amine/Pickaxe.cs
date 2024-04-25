using System.Collections.Generic;
using System;
using UnityEngine;

public class Pickaxe : Item
{

    public float damage;

    public Pickaxe()
    {
      
    }

    public void InflictDamageOn() // À coder
    {

    }

}
public class WoodenPickaxe : Pickaxe
{


    public WoodenPickaxe()
    {
        itemName = "WoodenPickaxe";
        itemType = "Tool";
        itemDurability = 30.0f;
        recipe = new Dictionary<string, int>
        {
            { "Wood", 15 }
        };
    }

}
public class IronPickaxe : Pickaxe
{


    public IronPickaxe()
    {
        itemName = "IronPickaxe";
        itemType = "Tool";
        itemDurability = 50.0f;
        recipe = new Dictionary<string, int>
        {
            { "Wood", 25 },
            { "Iron", 10 }
        };
    }

}
