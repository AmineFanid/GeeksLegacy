// CharacterInventory
// Classe CharacterInventory
// Auteur: Amine Fanid 
using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
public class CharacterInventory
{

    public int numberOfItems { get; private set; }
    public Dictionary<string, int> inventory { get; private set; }

    private Dictionary<string, Type> itemTypeMap = new Dictionary<string, Type>()
    {
        { "Dirt", typeof(Dirt) },
        { "Iron", typeof(Iron) },
        { "Wood", typeof(Wood) }
    };

    public int maxSize;

    public CharacterInventory()
    {
        numberOfItems = 0;
        inventory = new Dictionary<string, int>();
        maxSize = 9;
    }


    public bool canAddInventory()
    {
        return inventory.Count < maxSize;
    }

    public void addItem(string itemName)
    {

        if (!canAddInventory())
        {
            return;
        }

        numberOfItems++;
        if (inventory.ContainsKey(itemName))
        {
            inventory[itemName]++;
        }
        else
        {
            inventory.Add(itemName, 1);
        }
        
    }


    public void updateInventory() 
    {
        
        foreach (KeyValuePair<string, int> kvp in this.inventory)
        {
            if(kvp.Key != null)
            {
                if(kvp.Value < 0)
                {
                    this.inventory.Remove(kvp.Key);
                }
            }
        }
    }

    public bool inInventory(string itemName)
    {
        return inventory.ContainsKey(itemName);
    }

    public void deleteOne(string itemName) //TOUJOURS VERIFIER SI L'ITEM EST DANS L'INVENTAIRE, A L'AIDE de inInventory(), exemple de logique dans le script Inventory.cs
    {        
        inventory[itemName]--;
    }

    public Dictionary <string, int> getInventoryDict()
    {
        return this.inventory;
    }

    public int inventoryCount()
    {
        return inventory.Count;
    }

}
