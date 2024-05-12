using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;


public class ClickDetection : MonoBehaviour
{
    public Tilemap tiles;
    public Tile tile;
    ProceduralGeneration proceduralGeneration;
    public ItemFactory iFactory;



    public Vector3Int location;
    void Start()
    {
        proceduralGeneration = FindFirstObjectByType<ProceduralGeneration>();
        iFactory = FindFirstObjectByType<ItemFactory>();

    }


    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
           //Vector3? est un nullable type, normalement on peut pas avoir de null pour les vector3 et puisqu'un vecteur3 (0,0,0) est une position réelle, on utilise le nullable vector? et on verifie s'il y a qlqchose dedans.
            Vector3? clickLocation = GetTheTile();
            if (clickLocation.HasValue)
            {
                // The method returned a non-null value
                Vector3 location = clickLocation.Value;
                // Now you can use 'location' for further processing
                //Debug.Log("Clicked on tile at position: " + location);
                if (proceduralGeneration) { 
                    destroyTile(location);
                }
                else { Debug.Log("no instance of proceduralGeneration detected"); }
            }
            else
            {
                // The method returned null
                Debug.Log("Clicked outside of any tile.");
            }

        }

    }

    public Vector3? GetTheTile()
    {
        Vector3 mp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        location = tiles.WorldToCell(mp);

        if (tiles.GetTile(location)) {
            //Debug.Log("Tile Here : " + location);
            return location;
        }
        else
        {
            //Debug.Log("Empty Here");
            return null;
        }
    }

    public void destroyTile(Vector3 tilelocation)
    {
        GameObject tempPrefab = iFactory.getPrefab(proceduralGeneration.getTileVal((int)tilelocation.x, (int)tilelocation.y));
        proceduralGeneration.updateMap((int)tilelocation.x, (int)tilelocation.y, 0);
        tilelocation.y += 1;
        tilelocation.x += 0.5f;
        GameObject dirtBlock = Instantiate(tempPrefab, tilelocation, Quaternion.identity);
        dirtBlock.AddComponent<MaterialsBehaviour>(); //Attache le script a l''objet

    }

  
}
