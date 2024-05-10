using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public string itemName { get; set; }
    public string itemType { get; set; }
    public Sprite itemSprite { get; set; }
    public GameObject itemPrefab { get; set; }

    public virtual float DoDamage()
    {
        return 0f;
    }

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
