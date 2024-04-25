using System.Collections.Generic;
using System;
using UnityEngine;

public class Dirt : Item
{

    public Dirt()
    {
        itemName = "Dirt";
        itemType = "Material";
        itemDurability = 25.0f;
    }

}

public class Iron : Item
{

    public Iron()
    {
        itemName = "Iron";
        itemType = "Material";
        itemDurability = 50.0f;
    }

}

public class Wood : Item
{

    public Wood()
    {
        itemName = "Wood";
        itemType = "Material";
        itemDurability = 35.0f;
    }

}