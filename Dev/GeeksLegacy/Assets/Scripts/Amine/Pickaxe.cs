// Pickaxe
// Sous-classe d'Item, outils tel que sword ou axe
// Auteurs: Amine Fanid et Henrick Baril
using System.Collections.Generic;
using System;
using UnityEngine;

public class Pickaxe : Item
{
    public float damage;
    public Pickaxe()
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
        damage = 5.0f;
        recipe = new Dictionary<string, int>
        {
            { "Wood", 15 }
        };
    }

    public override float doDamage()
    {
        return damage;
    }

}
public class IronPickaxe : Pickaxe
{


    public IronPickaxe()
    {
        itemName = "IronPickaxe";
        itemType = "Tool";
        itemDurability = 50.0f;
        damage = 10.0f;
        recipe = new Dictionary<string, int>
        {
            { "Wood", 25 },
            { "Iron", 10 }
        };
    }

    public override float doDamage()
    {
        return damage;
    }

}
