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
    ProceduralGeneration proceduralGeneration;

    public Vector3Int location;
    void Start()
    {
        proceduralGeneration = FindFirstObjectByType<ProceduralGeneration>();
        
    }


    void Update()
    {
        /*
        if(_IsColliding == true)
        {
            float collisionDuration = Time.time - _WaitTime;

            if (collisionDuration >= 3.0f)
            {
                Debug.Log("Collision has been happening for more than 3 seconds");
            }
        }
        */
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
           //Vector3? est un nullable type, normalement on peut pas avoir de null pour les vector3 et puisqu'un vecteur3 (0,0,0) est une position réelle, on utilise le nullable vector? et on verifie s'il y a qlqchose dedans.
            Vector3? clickLocation = GetTheTile();
            if (clickLocation.HasValue)
            {
                // The method returned a non-null value
                Vector3 location = clickLocation.Value;
                // Now you can use 'location' for further processing
                Debug.Log("Clicked on tile at position: " + location);
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


            /*
            float currentTime = Time.time;
            if (currentTime - lastClickTime <= clickInterval)
            {
                clickCount++;
                Debug.Log("Click count: " + clickCount);
            }
            else
            {
                clickCount = 1;
            }
            lastClickTime = currentTime;
            */
        }
        /*
        if (clickCount >= 3)
        {
            Debug.Log("Clicked repeatedly for 3 seconds!");
            // Reset click count
            clickCount = 0;
            Destroy(gameObject);
        }*/
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
        
    }

  
}
