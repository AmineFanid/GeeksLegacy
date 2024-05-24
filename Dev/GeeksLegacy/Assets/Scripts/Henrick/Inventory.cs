// Inventory 
// Gestion de l'inventaire dans le jeu.
// Auteurs: Amine Fanid et Henrick Baril
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using TMPro;
using UnityEngine;

[DefaultExecutionOrder(30)]

public class Inventory : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    private GameObject _PLayerObject;
    private ControlCharacters _PlayerControl;
    public CharacterInventory inventory;
    private Animator _AnimatorInv;
    int inventoryIndex = 0;
    public Sprite[] spriteArray;
    int keyIndex = -1;
    public ItemFactory iFactory;
    GeeksLegacyLauncher geeksLegacyLauncher;

    void Start()
    {
        geeksLegacyLauncher = FindFirstObjectByType<GeeksLegacyLauncher>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        _AnimatorInv = GetComponent<Animator>();
        _PLayerObject = GameObject.FindGameObjectWithTag("Player");
        _PlayerControl = _PLayerObject.GetComponent<ControlCharacters>();
        inventory = geeksLegacyLauncher.getPlayerFromDB().GetPlayerInventory();
        iFactory = FindFirstObjectByType<ItemFactory>();
    }

    void Update()
    { 
        // Pour le côté FrontEnd de l'inventaire, permet de scroll la souris et ainsi se déplacer d'index en index dans l'inventaire
        if (Input.GetAxisRaw("Scroll") > 0.0f)
        {
            _AnimatorInv.SetTrigger("ScrollUp");
            inventoryIndex++;
        }
        if (Input.GetAxisRaw("Scroll") < 0.0f)
        {
            _AnimatorInv.SetTrigger("ScrollDown");
            inventoryIndex--;
        }

        //Pour la gestion de l'inventaire
        if(inventory.inventoryCount() > 0)
        {

            if (keyIndex == -1 || keyIndex >= inventory.inventoryCount())
            {
                keyIndex = 0;
            }
            //Permet l'affichage des éléments dans l'inventaire selon ce qu'on a dant l'inventaire, ainsi que le nombre de chaque élément
            foreach (KeyValuePair<string, int> kvp in inventory.getInventoryDict()) //Parcours le dictionnaire dans l'inventaire, qui contient les infos sur les éléments dans l'inventaire
            {
                GameObject slot = this.gameObject.transform.GetChild(keyIndex).gameObject;
                if(kvp.Value > 0)
                {
                    slot.GetComponent<SpriteRenderer>().sprite = spriteArray[changeSprite(kvp.Key)];
                    slot.transform.GetChild(0).GetComponent<TMP_Text>().text = kvp.Value.ToString();
                }
                else
                {
                    slot.GetComponent<SpriteRenderer>().sprite = spriteArray[changeSprite("empty")];
                    slot.transform.GetChild(0).GetComponent<TMP_Text>().text = "";
                }
            }

            if (Input.GetButtonDown("Throw")) // "g" dans le clavier
            {
                throwAwaySum(inventoryIndex % 9); // Pour jeter un élément de l'inventaire
            }
        
        }
        try { 
            inventory.updateInventory(); //Mettre à jour notre inventaire
        }
        catch(Exception e)
        {
            print(e);            
        }
    }

    public void throwAwaySum(int indexInventory) // Pour jeter un élément de l'inventaire
    {
        int i = 0;
        Dictionary<string, int> tempDict = inventory.getInventoryDict();
        List<string> keysToRemove = new List<string>();
        foreach (KeyValuePair<string, int> kvp in tempDict) //Parcours l'inventaire
        {
            if (i == indexInventory) // trouve l'élément à l'index
            {
                string s = kvp.Key;
                if (inventory.inInventory(s))
                { 
                    Vector3 characPosition = _PlayerControl.transform.position;
                    Vector2 direction = _PlayerControl.GetDirectionPersonnage(); //le jette à gauche ou a droite du personnage selon la direction à laquelle il fait face
                    characPosition.x += direction.x != 0.0f ? (direction.x * 2) : 2.0f;
                    GameObject temp = iFactory.getPrefab(s); 
                    Instantiate(temp, characPosition, Quaternion.identity); //Instantie un Prefab de l'élément jeté.
                    keysToRemove.Add(s); // Ajoute la clé dans cette liste qu'on va parcourir plus tard, pour supprimer un des éléments de l'inventaire
                    break;
                }
            }
            i++;
        }
        foreach (string key in keysToRemove)
        {
            inventory.deleteOne(key); // Supprime 1 exemplaire d'un élément de l'inventaire, pour chaque élément jeté
        }
    }

    public int changeSprite(string dictKey) //Pour le sprite dans l'inventaire, selon la liste de Sprite dans Unity
    {
        switch (dictKey)
        {
            case "Dirt":
                return 0;

            case "Iron":
                return 1;

            case "Wood":
                return 2;            
            
            case "Axe":
                return 3;  
                
            case "PickAxe":
                return 4;     
                
            case "Sword":
                return 5;
            case "empty":
                return 8;
        }

        return 0;
    }
}
