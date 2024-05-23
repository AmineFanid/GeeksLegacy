using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
public class CharacterInventory
{

//    [SerializeField] private GameObject inventoryPrefab;
    public int NumberOfItems { get; private set; } //n'importe quel script peut get la valeur, mais seulement ce script peut modifier la valeur
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


    public void updateInventory()
    {
     //   if (this.inventory[])
        
        foreach (KeyValuePair<string, int> kvp in this.inventory)
        {
            if(kvp.Key != null)
            {
                if(kvp.Value < 0)
                {
                    this.inventory.Remove(kvp.Key);
                }
            }
            //Debug.Log("clé : " + kvp.Key);
            //Debug.Log("valeur : " + kvp.Value);
        }
    }

    public bool inInventory(string itemName)
    {
        return inventory.ContainsKey(itemName);
    }

    public void deleteOne(string itemName) //SVP TOUJOURS VERIFIER SI L'ITEM EST DANS L'INVENTAIRE, A L'AIDE de inInventory()
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


    public void dropStuff(ControlCharacters charac, string itemName)
    {
        Vector3 characPosition = charac.transform.position;

        //proceduralGeneration.updateMap((int)tilelocation.x, (int)tilelocation.y, 0);
        // Spawn a block of dirt at the same location
        //tilelocation.y += 1;
        //tilelocation.x += 0.5f;
        //GameObject dirtBlock = Instantiate(dirtPrefab, tilelocation, Quaternion.identity);
        // Attach the MaterialsBehaviour script to the spawned dirt block
        //dirtBlock.AddComponent<MaterialsBehaviour>();

    }

}
