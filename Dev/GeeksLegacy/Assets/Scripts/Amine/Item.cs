using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public string itemName { get; set; }
    public string itemType { get; set; }
    public Sprite itemSprite { get; set; }
    [SerializeField] public GameObject itemPrefab { get; set; }
    public Dictionary<string, int> recipe;
    public float itemDurability { get; set; }  
    
    public virtual float doDamage()
    {
        return 0f;
    }

    // Pour enlever des points de durabilité à l'item
    public void deductDurability(float pts)
    {
        this.itemDurability -= pts;
    }

}
