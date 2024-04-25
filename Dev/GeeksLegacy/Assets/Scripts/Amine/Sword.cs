using System.Collections.Generic;
using UnityEngine;


public class Sword : Item
{

    public float damage;

    public Sword()
    {
      
    }

    public void InflictDamageOn() // À coder
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
        recipe = new Dictionary<string, int>
        {
            { "Wood", 50 }
        };
    }

}

public class IronSword : Sword
{


    public IronSword()
    {
        itemName = "IronSword";
        itemType = "Weapon";
        itemDurability = 150.0f;
        recipe = new Dictionary<string, int>
        {
            { "Wood", 70 },
            { "Iron", 80 }
        };
    }

}
