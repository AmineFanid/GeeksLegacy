using UnityEngine;
using System.Collections.Generic;


public class CharacterInventory : MonoBehaviour
{

//    [SerializeField] private GameObject inventoryPrefab;
    public int NumberOfItems { get; private set; } //n'importe quel script peut get la valeur, mais seulement ce script peut modifier la valeur
    public Dictionary<string, int> inventory;
    public int maxSize;

    public CharacterInventory()
    {
        NumberOfItems = 0;
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
            Debug.Log("INVENTORY IS FULL");
            return;
        }

        NumberOfItems++; //Pour sauvegarder le nombre d'item total
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
