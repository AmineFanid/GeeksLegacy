using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ProceduralGeneration : MonoBehaviour
{
    enum TileValue{
        CAVE = 99,
        BIOME1 = 1,
        BIOME2 = 2,
        BIOME3 = 3
    }
    
    [SerializeField] float seed;

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
    //[SerializeField][Range(1, 3)] int nbBiomes = 1;

    [Header("Cavern")]
    [SerializeField] TileBase caveTile;
    [SerializeField] Tilemap caveTileMap;
    int caveVal = (int)TileValue.CAVE;


    [Header("Biome 1")]
    [SerializeField] TileBase groundTile;
    [SerializeField] Tilemap groundTileMap;
    int firstBiomeVal;
    

    [Header("Biome 2")]
    [SerializeField] TileBase iceTile;
    [SerializeField] Tilemap iceTileMap;
    int secondBiomeVal;

    [Header("Biome 3")]
    [SerializeField] TileBase desolationTile;
    [SerializeField] Tilemap desolationTileMap;
    int thirdBiomeVal;

    [Header("Background")]
    [SerializeField] TileBase backgroundTile;
    [SerializeField] Tilemap backgroundTileMap;
    int backgroundVal;

    List<int[]> surface;
    int[,] map;

    [Header("Charac")]
    [SerializeField] GameObject characterPrefab;

    int biomeCount;

    // Start is called before the first frame update
    void Start()
    {
        biomeCount = BiomeCount(); // On get le nombre de biome ici
        BiomeRandomization();
        perlinHeightList = new int[width]; //liste de la taille de notre width
        surface = new List<int[]>();
        Generation();
    }

    private void Update()
    {
        RenderMap(map, groundTileMap, caveTileMap, desolationTileMap, desolationTile, groundTile, caveTile, backgroundTileMap);
        /*if(Input.GetKeyDown(KeyCode.Space))
        {
            Generation();
        }*/
    }

    public int BiomeCount()
    {
        int biomeCount = 0;
        if (groundTile != null) biomeCount++;
        if (iceTile != null) biomeCount++;
        if (desolationTile != null) biomeCount++;
        return biomeCount;
    }

    public void BiomeRandomization()
    {
        List<TileValue> liste = new List<TileValue> { TileValue.BIOME1, TileValue.BIOME2, TileValue.BIOME3 };
        List<int> l2 = new List<int>();

        System.Random random = new System.Random();

        int listLength = liste.Count; // Get le nombre d'elements de la liste

        for (int i = 0; i < listLength; i++)
        {
            int randomIndex = random.Next(liste.Count); // Genere un index aleatoire depuis la liste
            int randomValue = (int)liste[randomIndex]; // Get la valeur en int depuis l'enum
            l2.Add(randomValue); // Ajoute la valeur a l2
            liste.RemoveAt(randomIndex); // Enleve la valeur de la liste
        }
        firstBiomeVal = l2[0];
        secondBiomeVal = l2[1];
        thirdBiomeVal = l2[2];
        //Debug.Log(firstBiomeVal + "  " + secondBiomeVal + "  " + thirdBiomeVal);
    }


    public void Generation()
    {
        //seed = Time.time;
        clearMap();
        map = GenerateArray(width, height, true);
        map = TerrainGeneration(map);
        smoothMap(nbSmoothing);
        GetSurfaceTiles();
        RenderMap(map, groundTileMap, caveTileMap, desolationTileMap, desolationTile, groundTile, caveTile, backgroundTileMap);
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

    public int MapWidthDivision(int xWidth)
    {
        //System.Random pseudoRandom = new System.Random(seed.GetHashCode());
        int limit;
        switch (biomeCount)
        {
            case 1:
                limit = width;
                if (xWidth < limit)
                {
                    return firstBiomeVal;
                }
                break;
            case 2:
                limit = width / 2;
                if (xWidth < (limit * 2) && xWidth > limit)
                {
                    return secondBiomeVal;
                }
                break;
            case 3: 
                limit = width / 3;
                if (xWidth < (limit * 3) && xWidth > (limit * 2))
                {
                    return thirdBiomeVal;
                }
                break;
        }
        return firstBiomeVal;
        /*
        if(xWidth < limit)
        {
            map[x, y] = (pseudoRandom.Next(1, 100) < randomFillPercent) ? firstBiomeVal : caveBiomeVal;
        }
        if(xWidth < (limit * 2) && xWidth > limit)
        {
            map[x, y] = (pseudoRandom.Next(1, 100) < randomFillPercent) ? secondBiomeVal : caveBiomeVal;
        }
        if (xWidth < (limit * 3) && xWidth > (limit * 2))
        {
            map[x, y] = (pseudoRandom.Next(1, 100) < randomFillPercent) ? thirdBiomeVal : caveBiomeVal;
        }*/
    }

    public int[,] TerrainGeneration(int[,] map)
    {
        System.Random pseudoRandom = new System.Random(seed.GetHashCode()); //Nous donne des valeurs aléatoires selon notre seed
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
                int num = MapWidthDivision(x);
                map[x, y] = (pseudoRandom.Next(1, 100) < randomFillPercent) ? num : caveVal; //si la valeur du pseudoRandom est plus grande que le randomFillPercent, on place un groundTile, sinon un caveTile

            }
        }
        return map;
    }

    public void smoothMap(int nbSmoothing) 
    {
        int num = firstBiomeVal;
        for (int i = 0; i < nbSmoothing; i++)
        {
            for (int x = 0; x < width; x++) //loop à travers la width de notre map
            {
                for (int y = 0; y < perlinHeightList[x]; y++) //loop à travers une liste de nos valeur de PerlinHeignt
                {
                    if (x == 0 || y == 0 || x == width - 1 || y == perlinHeightList[x] - 1) //Pour créer une bordure autour de la map et ainsi éviter que les caves soient exposés
                    {
                        num = MapWidthDivision(x);
                        map[x, y] = num;
                    }
                    else
                    {
                        //Moore's neighborhood
                        int surroundingGroundCount = GetSurroundingGroundCount(x,y);
                        //GetSurfaceGround(x, y);
                        if (surroundingGroundCount > 4)
                        {
                            map[x, y] = num;
                        }
                        else if (surroundingGroundCount < 4)
                        {
                            map[x, y] = caveVal;
                        }
                    }
                }
            }            
        }


    }

    public void GetSurfaceTiles()
    {
        surface.Clear();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GetSurfaceGround(x, y);
            }
        }
    }


    /*public void GetSurfaceGround(int gridX, int gridY)
    {

        if (map[gridX, gridY] == 1 && map[gridX, gridY + 1] == 0)
        {
            surface.Add(new int[] { gridX, (gridY+2) });
        }

    }*/

    public void GetSurfaceGround(int gridX, int gridY)
    {
        //Iteration vers le haut, à partir du tile courant, jusqua ce qu'un tile vide soit trouvé
        for (int y = gridY; y < height; y++)
        {
            // Check si le tile courant est vide
            if (map[gridX, y] == 0)
            {
                surface.Add(new int[] { gridX, y+2 }); // Ajoute le tile à la surface dans ma liste surface
                return; // Sort de la fonction quand on a trouvé la tile de surface
            }
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
                        if (map[neighbourX, neighbourY] != 0 && map[neighbourX, neighbourY] != caveVal) //Si on a plus d'un groundTile comme voisin, on incrémente la variable groundCount
                        {
                            groundCount++;
                        }


                    }
                }
            }
        }

        return groundCount;
    }

    // DOIT MODIFIER CETTE FCT, GESTION DU SWITCH PAS COMPLETÉ
    public void RenderMap(int[,] map, Tilemap groundTileMap, Tilemap caveTileMap, Tilemap desolationTileMap, TileBase desolationTile, TileBase groundTile, TileBase caveTile, Tilemap backgroundTileMap)
    {
        for(int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++)
            {

                switch (map[x, y])
                {
                    case var value when value == firstBiomeVal:
                        groundTileMap.SetTile(new Vector3Int(x, y, 0), groundTile);
                        break;
                    case var value when value == secondBiomeVal:
                        desolationTileMap.SetTile(new Vector3Int(x, y, 0), desolationTile);
                        break;
                    case var value when value == thirdBiomeVal:
                        desolationTileMap.SetTile(new Vector3Int(x, y, 0), desolationTile);
                        break;
                    case var value when value == caveVal: // BIZARRE OUI, MAIS REGLE UN BUG DE UNITY. J'AI TROUVÉ CE TRUC SUR STACK OVER FLOW :) ca prenait pas juste caveVal
                        caveTileMap.SetTile(new Vector3Int(x, y, 0), caveTile);
                        break;
                    case var value when value == 0:
                        groundTileMap.SetTile(new Vector3Int(x, y, 0), null);
                        desolationTileMap.SetTile(new Vector3Int(x, y, 0), null);
                        backgroundTileMap.SetTile(new Vector3Int(x, y, 0), null);
                        break;
                    default:
                        //rien
                        break;
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

    public void updateMap(int x, int y, int val)
    {
        Debug.Log("updating....: " + x + ", " + y);
        Debug.Log(map[x, y]);
        Debug.Log(Enum.GetName(typeof(TileValue), map[x, y]));
        map[x, y] = val;
        backgroundTileMap.SetTile(new Vector3Int(x, y, 0), null);
        //map[x, y].SetTile()
    }

    public void SpawnCharacter(GameObject characterPrefab)
    {
        if (surface.Count > 0)
        {
            // Choose a random surface tile
            int randomIndex = UnityEngine.Random.Range(0, surface.Count);
            int[] surfaceTile = surface[randomIndex];

            // Calculate the position to spawn the character
            Vector3 spawnPosition = new Vector3(surfaceTile[0], surfaceTile[1], 0);

            // Spawn the character prefab at the calculated position
            Instantiate(characterPrefab, spawnPosition, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("Surface list is empty. Make sure to generate the terrain first." + surface.Count);
        }
    }


}
