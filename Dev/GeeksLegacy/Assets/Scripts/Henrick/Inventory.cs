using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using TMPro;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    private GameObject _PLayerObject;
    private ControlCharacters _PlayerControl;
    public CharacterInventory inventory;
    private Animator _AnimatorInv;
    int InventoryIndex = 0;
    public Sprite[] spriteArray;
    int keyIndex = -1;
    public ItemFactory iFactory;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        _AnimatorInv = GetComponent<Animator>();

        _PLayerObject = GameObject.FindGameObjectWithTag("Player");
        _PlayerControl = _PLayerObject.GetComponent<ControlCharacters>();
        inventory = _PlayerControl.findPlayerObject().GetPlayerInventory();
        iFactory = FindFirstObjectByType<ItemFactory>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxisRaw("Scroll") > 0.0f)
        {
            _AnimatorInv.SetTrigger("ScrollUp");
            InventoryIndex++;
        }
        if (Input.GetAxisRaw("Scroll") < 0.0f)
        {
            _AnimatorInv.SetTrigger("ScrollDown");
            InventoryIndex--;
        }
        //// ICI ON VA AVOIR LA GESTION DU VISUEL DE SPRITE
        if(inventory.inventoryCount() > 0)
        {

            // If keyIndex is not set or out of bounds, set it to 0
            if (keyIndex == -1 || keyIndex >= inventory.inventoryCount())
            {
                keyIndex = 0;
            }
            

            //inventory.insideOfInventory();
            foreach (KeyValuePair<string, int> kvp in inventory.getInventoryDict())
            {
                string keyAtIndex = inventory.getInventoryDict().Keys.ElementAt(keyIndex); //Pas sur encore. a verifier quand on a plus de chose dans notre dictionaire

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

            if (Input.GetButtonDown("Throw"))
            {
                throwAwaySum(InventoryIndex%9);
            }
        
        }
        try { 
            inventory.updateInventory();
        }
        catch(Exception e)
        {
            print(e);            
        }
    }

    public void throwAwaySum(int indexInventory)
    {
        int i = 0;
        Dictionary<string, int> tempDict = inventory.getInventoryDict();
        List<string> keysToRemove = new List<string>();
        foreach (KeyValuePair<string, int> kvp in tempDict)
        {
            if (i == indexInventory)
            {
                string s = kvp.Key;
                //EventManager.TriggerEvent(EventManager.PossibleEvent.eVieJoueurChange, s); // Utilisation de l'observer
                if (inventory.inInventory(s)) { 
                    Vector3 characPosition = _PlayerControl.transform.position;
                    Vector2 direction = _PlayerControl.GetDirectionPersonnage();
                    characPosition.x += direction.x != 0.0f ? (direction.x * 2) : 2.0f;
                    GameObject temp = iFactory.getPrefab(s);
                    Instantiate(temp, characPosition, Quaternion.identity);
                    keysToRemove.Add(s);
                    break;
                }
            }
            i++;
        }
        foreach (string key in keysToRemove)
        {
            inventory.deleteOne(key);
        }
    }

    public int changeSprite(string dictKey)
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
