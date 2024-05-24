// Sword
// Sous-classe d'Item, outils tel que sword ou axe
// Auteurs: Amine Fanid et Henrick Baril
using System.Collections.Generic;
using UnityEngine;


public class Sword : Item
{
    public float damage;
    public Sword()
    {
      
    }
}

public class WoodenSword : Sword
{
    public WoodenSword() 
    {
        itemName = "WoodenSword";
        itemType = "Weapon";
        itemDurability = 75.0f;
        damage = 10.0f;
        recipe = new Dictionary<string, int>
        {
            { "Wood", 50 }
        };
    }

    public override float doDamage()
    {
        return damage;
    }
}

public class IronSword : Sword
{
    public IronSword()
    {
        itemName = "IronSword";
        itemType = "Weapon";
        itemDurability = 150.0f;
        damage = 20.0f;
        recipe = new Dictionary<string, int>
        {
            { "Wood", 70 },
            { "Iron", 80 }
        };
    }

    public override float doDamage()
    {
        return damage;
    }
}
