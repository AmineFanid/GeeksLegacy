using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ProceduralGeneration : MonoBehaviour
{
    [Header("Terrain Gen")]
    [SerializeField] int width;
    [SerializeField] int height;
    [SerializeField] float smoothness;
    int[] perlinHeightList;

    [Header("Cave Gen")]
    //[Range(0, 1)]
    //[SerializeField] float modifier;
    [Range(0,100)]
    [SerializeField] int randomFillPercent;
    [SerializeField] int nbSmoothing = 1;



    [Header("Tile")]
    [SerializeField] TileBase groundTile;
    [SerializeField] TileBase caveTile;
    [SerializeField] Tilemap groundTileMap;
    [SerializeField] Tilemap caveTileMap;

    List<int[]> surface;
    int[,] map;
    [SerializeField] float seed;

    [Header("Charac")]
    [SerializeField] GameObject characterPrefab;


    // Start is called before the first frame update
    void Start()
    {
        perlinHeightList = new int[width]; //liste de la taille de notre width
        surface = new List<int[]>();
        Generation();
    }

    private void Update()
    {
        /*if(Input.GetKeyDown(KeyCode.Space))
        {
            Generation();
        }*/
    }

    public void Generation()
    {
        //seed = Time.time;
        clearMap();
        map = GenerateArray(width, height, true);
        map = TerrainGeneration(map);
        smoothMap(nbSmoothing);
        GetSurfaceTiles();
        RenderMap(map, groundTileMap, caveTileMap, groundTile, caveTile);
        SpawnCharacter(characterPrefab);
    }

    public int[,] GenerateArray(int width, int height, bool empty) { 

        int[,] map = new int[width, height];   

        for(int x = 0; x < width; x++) //loop à travers la width de notre map
        {
            for (int y = 0; y < height; y++) // loop à travers la hauteur de notre map
            {
                map[x,y] = (empty) ? 0 : 1; 
            }
        }

        return map;
    }

    public int[,] TerrainGeneration(int[,] map)
    {
        System.Random pseudoRandom = new System.Random(seed.GetHashCode()); //Nous des des valeurs aléatoires selon notre seed
        int perlinhHeight;
        for (int x = 0;x < width; x++)
        {
            perlinhHeight = Mathf.RoundToInt(Mathf.PerlinNoise(x / smoothness, seed) * height / 2);
            perlinhHeight += height / 2;
            perlinHeightList[x] = perlinhHeight;
            for (int y = 0; y < perlinhHeight; y++)
            {
                //map[x, y] = 1;
                //int  caveValue = Mathf.RoundToInt(Mathf.PerlinNoise((x*modifier)+ seed, (y * modifier) + seed)); //Nous donne des 0 et des 1
                //map[x, y] = (caveValue == 1) ? 2 : 1;
                map[x, y] = (pseudoRandom.Next(1, 100) < randomFillPercent) ? 1 : 2; //si la valeur du pseudoRandom est plus grande que le randomFillPercent, on place un groundTile, sinon un caveTile

            }
        }
        return map;
    }

    public void smoothMap(int nbSmoothing) 
    {

        for (int i = 0; i < nbSmoothing; i++)
        {
            for (int x = 0; x < width; x++) //loop à travers la width de notre map
            {
                for (int y = 0; y < perlinHeightList[x]; y++) //loop à travers une liste de nos valeur de PerlinHeignt
                {
                    if (x == 0 || y == 0 || x == width - 1 || y == perlinHeightList[x] - 1) //Pour créer une bordure autour de la map et ainsi éviter que les caves soient exposés
                    {
                        map[x, y] = 1;
                    }
                    else
                    {
                        //Moore's neighborhood
                        int surroundingGroundCount = GetSurroundingGroundCount(x,y);
                        //GetSurfaceGround(x, y);
                        if (surroundingGroundCount > 4)
                        {
                            map[x, y] = 1;
                        }
                        else if (surroundingGroundCount < 4)
                        {
                            map[x, y] = 2;
                        }
                    }
                }
            }            
        }


    }

    public void GetSurfaceTiles()
    {
        surface.Clear(); // Clear the list before populating it with new values

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GetSurfaceGround(x, y);
            }
        }
    }


    public void GetSurfaceGround(int gridX, int gridY)
    {

        if (map[gridX, gridY] == 1 && map[gridX, gridY + 1] == 0)
        {
            surface.Add(new int[] { gridX, (gridY+2) });
        }

    }
    
    public int GetSurroundingGroundCount(int gridX, int gridY) 
    {
        //Fonction qui nous permet de compter les occurences de ground ou cave autour de chaque tile
        int groundCount = 0;

        for (int neighbourX = gridX -1; neighbourX <= gridX+1; neighbourX++)
        {
            for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++) 
            {
                if (neighbourX >= 0 && neighbourX < width && neighbourY >= 0 && neighbourY < height) //On s'assure d'être à l'intérieur de notre map
                {
                    if (neighbourX != gridX || neighbourY != gridY) //Exclusion de la tile au milieu
                    {
                        if (map[neighbourX, neighbourY] == 1) //Si on a plus d'un groundTile comme voisin, on incrémente la variable groundCount
                        {
                            groundCount++;
                        }


                    }
                }
            }
        }

        return groundCount;
    }

    public void RenderMap(int[,] map, Tilemap groundTileMap, Tilemap caveTileMap, TileBase groundTile, TileBase caveTile)
    {
        for(int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++)
            {
                if (map[x, y] == 1)
                {
                    groundTileMap.SetTile(new Vector3Int(x, y, 0), groundTile);
                }
                else if (map[x, y] == 2) 
                { 
                    caveTileMap.SetTile(new Vector3Int(x, y, 0), caveTile);
                }
            }
        }

    }

    public void clearMap() 
    {
        groundTileMap.ClearAllTiles();
        caveTileMap.ClearAllTiles();
    }

    public List<int[]> getSurface() {
        return surface;
    }

    public void SpawnCharacter(GameObject characterPrefab)
    {
        if (surface.Count > 0)
        {
            // Choose a random surface tile
            int randomIndex = Random.Range(0, surface.Count);
            int[] surfaceTile = surface[randomIndex];

            // Calculate the position to spawn the character
            Vector3 spawnPosition = new Vector3(surfaceTile[0], surfaceTile[1], 0);

            // Spawn the character prefab at the calculated position
            Instantiate(characterPrefab, spawnPosition, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("Surface list is empty. Make sure to generate the terrain first.");
        }
    }


}
