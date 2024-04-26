using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;


public class ClickDetection : MonoBehaviour
{

    //private float clickInterval = 0.3f; // Adjust this value as needed
    //private int clickCount = 0;
    //private float lastClickTime = 0f;
    public Tilemap tiles;
    public Tile tile;
    public GameObject dirtPrefab;
    ProceduralGeneration proceduralGeneration;
    

    public Vector3Int location;
    void Start()
    {
        proceduralGeneration = FindFirstObjectByType<ProceduralGeneration>();
        
    }


    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
           //Vector3? est un nullable type, normalement on peut pas avoir de null pour les vector3 et puisqu'un vecteur3 (0,0,0) est une position r�elle, on utilise le nullable vector? et on verifie s'il y a qlqchose dedans.
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
        proceduralGeneration.updateMap((int)tilelocation.x, (int)tilelocation.y, 0);
        // Spawn a block of dirt at the same location
        tilelocation.y += 1;
        tilelocation.x += 0.5f;
        GameObject dirtBlock = Instantiate(dirtPrefab, tilelocation, Quaternion.identity);
        // Attach the MaterialsBehaviour script to the spawned dirt block
        dirtBlock.AddComponent<MaterialsBehaviour>();

    }

  
}
