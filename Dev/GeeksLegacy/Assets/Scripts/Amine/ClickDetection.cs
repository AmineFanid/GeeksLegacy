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
           //Vector3? est un nullable type, normalement on peut pas avoir de null pour les vector3 et puisqu'un vecteur3 (0,0,0) est une position réelle, on utilise le nullable vector? et on verifie s'il y a quelquechose dedans.
            Vector3? clickLocation = getTheTile();
            if (clickLocation.HasValue)
            {
                Vector3 location = clickLocation.Value;
                if (proceduralGeneration) { 
                    destroyTile(location);
                }
                else { Debug.Log("Pas d'instance de proceduralGeneration detecté"); }
            }
            else
            {
                Debug.Log("Vous n'avez pas cliqué sur une tile");
            }

        }

    }

    public Vector3? getTheTile()
    {
        Vector3 mp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        location = tiles.WorldToCell(mp);

        if (tiles.GetTile(location)) {
            return location;
        }
        else
        {
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
