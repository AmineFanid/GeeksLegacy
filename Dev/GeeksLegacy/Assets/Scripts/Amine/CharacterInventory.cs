using UnityEngine;
using System.Collections.Generic;


public class CharacterInventory : MonoBehaviour
{
   
    public int NumberOfItems { get; private set; } //n'importe quel script peut get la valeur, mais seulement ce script peut modifier la valeur
    public Dictionary<string, int> inventory = new Dictionary<string, int>();

    public void ItemsCollected()
    {
        NumberOfItems++;
        print("number of items = " + NumberOfItems);
    }

    public void addItem(string itemName)
    {
        if (inventory.ContainsKey(itemName))
        {
            inventory[itemName]++;
        }
        else
        {
            inventory.Add(itemName, 1);
        }
    }

}
