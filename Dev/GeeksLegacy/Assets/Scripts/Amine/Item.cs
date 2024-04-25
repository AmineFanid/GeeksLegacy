using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item
{
    public string itemName { get; set; }
    public string itemType { get; set; }
    
    public float itemDurability { get; set; }
    //public List<String> recipe;

    public Dictionary<string, int> recipe;


    public Item()
    {
    } 

    // Pour enlever des points de durabilité à l'item
    public void DeductDurability(float pts)
    {
        this.itemDurability -= pts;
    }
}
