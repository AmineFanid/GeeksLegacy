using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

[DefaultExecutionOrder(25)]

public class ProceduralGeneration : MonoBehaviour
{
    enum TileValue{
        CAVE = 99,
        BIOME1 = 1,
        BIOME2 = 2,
        BIOME3 = 3,
        MINERAL1 = 4
    }
    
    [SerializeField] float seed;

    [Header("Terrain Gen")]
    [SerializeField] int width;
    [SerializeField] int height;
    [SerializeField] float smoothness;
    int[] perlinHeightList;


    [Header("Minerals")]
    [Range(90, 100)]
    [SerializeField] int randomFillPercentMinerals;
    [SerializeField] int nbSmoothingMinerals = 1;

    [Header("Mineral 1")]
    [SerializeField] TileBase mineralTile;
    [SerializeField] Tilemap mineralTileMap;
    [SerializeField] string firstMineralDrops;
    int firstMineralVal = (int)TileValue.MINERAL1;

    [Header("Cave Gen")]
    [Range(0,100)]
    [SerializeField] int randomFillPercent;
    [SerializeField] int nbSmoothing = 1;

    [Header("Cavern")]
    [SerializeField] TileBase caveTile;
    [SerializeField] Tilemap caveTileMap;
    int caveVal = (int)TileValue.CAVE;


    [Header("Biome 1")]
    [SerializeField] TileBase firstTile;
    [SerializeField] Tilemap firstTileMap;
    [SerializeField] string firstBiomeDrops;
    int firstBiomeVal;
    Tilemap firstBiomeTileMap;
    TileBase firstBiomeTile;
    

    [Header("Biome 2")]
    [SerializeField] TileBase secondTile;
    [SerializeField] Tilemap secondTileMap;
    [SerializeField] string secondBiomeDrops;
    int secondBiomeVal;
    Tilemap secondBiomeTileMap;
    TileBase secondBiomeTile;


    [Header("Biome 3")]
    [SerializeField] TileBase thirdTile;
    [SerializeField] Tilemap thirdTileMap;
    [SerializeField] string thirdBiomeDrops;
    int thirdBiomeVal;
    Tilemap thirdBiomeTileMap;
    TileBase thirdBiomeTile;


    [Header("Background")]
    [SerializeField] TileBase backgroundTile;
    [SerializeField] Tilemap backgroundTileMap;
    int backgroundVal;

    List<int[]> surface;
    Dictionary<int,string> biomeNames;
    int[,] map;

    [Header("Charac")]
    [SerializeField] public GameObject characterPrefab;

    int biomeCount;

    // Start is called before the first frame update
    void Start()
    {
        /*
        biomeCount = BiomeCount(); // On get le nombre de biome ici
        BiomeRandomization();
        perlinHeightList = new int[width]; //liste de la taille de notre width
        surface = new List<int[]>();
        biomeNames = new Dictionary<int, string>();
        if(mineralTile != null)
        {
            biomeNames[firstMineralVal] = firstMineralDrops;
        }
        */
        //Generation();
    }

    private void Update()
    {
        if (map != null)
        {
            RenderMap(map);
        }
    }

    public int BiomeCount()
    {
        int biomeCount = 0;
        if (firstTile != null) biomeCount++;
        if (secondTile != null) biomeCount++;
        if (thirdTile != null) biomeCount++;
        return biomeCount;
    }

    public void prepareProceduralGeneration()
    {
        biomeCount = BiomeCount(); // On get le nombre de biome ici
        BiomeRandomization();
        perlinHeightList = new int[width]; //liste de la taille de notre width
        surface = new List<int[]>();
        biomeNames = new Dictionary<int, string>();
        if (mineralTile != null)
        {
            biomeNames[firstMineralVal] = firstMineralDrops;
        }
    }

    public void BiomeRandomization()
    {
        //List<TileValue> liste = new List<TileValue> { TileValue.BIOME1, TileValue.BIOME2, TileValue.BIOME3 };
        List<TileValue> liste = new List<TileValue>();

        foreach(TileValue i in Enum.GetValues(typeof(TileValue)))
        {
            if(i != TileValue.CAVE && i != TileValue.MINERAL1) liste.Add(i);
        }

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

        firstBiomeVal = firstTile != null ? l2[0] : -1;
        secondBiomeVal = secondTile != null ? l2[1] : -1;
        thirdBiomeVal = thirdTile != null ? l2[2] : -1;


        Debug.Log(firstBiomeVal + "  " + secondBiomeVal + "  " + thirdBiomeVal);
    }


    public void Generation()
    {
        clearMap();
        if (map == null)
        {
            map = GenerateArray(width, height, true);
            map = TerrainGeneration(map);
            smoothMap(nbSmoothing);
        }
        GetSurfaceTiles();
        BiomeNumAssignation();
        if(mineralTile != null)
        {
            placeMinerals(map);
            mineralSmoothing(nbSmoothingMinerals);
        }
        RenderMap(map);
        //SpawnCharacter(characterPrefab);
    }

    /*
    public void GenerateExistingMap() // Similaire a Generation(), mais pour quand on a deja une map  dans la bd (JSON)
    {
        GetSurfaceTiles();
        BiomeNumAssignation();
        if (mineralTile != null)
        {
            placeMinerals(map);
            mineralSmoothing(nbSmoothingMinerals);
        }
        RenderMap(map);
    }*/

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
        int sectionWidth = width / biomeCount; // Calcul la largeur des sections de biomes

        // Determine le biome basé sur la position en x
        int sectionIndex = xWidth / sectionWidth;

        switch (sectionIndex)
        {
            case 0:
                return firstBiomeVal; // Return la valeur du 1er biome
            case 1:
                return secondBiomeVal; // Return la valeur du second biome
            case 2:
                return thirdBiomeVal; // Return la valeur du troisieme biome
            default:
                return firstBiomeVal; 
        }
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
                int num = MapWidthDivision(x);
                map[x, y] = (pseudoRandom.Next(1, 100) < randomFillPercent) ? num : caveVal; //si la valeur du pseudoRandom est plus grande que le randomFillPercent, on place un groundTile/iceTile/desolationTile, sinon un caveTile
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
                        int surroundingGroundCount = GetSurroundingGroundCount(x,y, caveVal);
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

    public void mineralSmoothing(int nbSmoothingMinerals)
    {
        for (int i = 0; i < nbSmoothingMinerals; i++)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < perlinHeightList[x]; y++)
                {
                    // If the tile is not a mineral, check its neighbors
                    if (map[x, y] != firstMineralVal)
                    {
                        // Count the number of neighboring mineral tiles
                        int mineralCount = GetSurroundingMineralCount(x, y, firstMineralVal);

                        // If there are at least 4 neighboring mineral tiles, convert this tile to a mineral
                        if(mineralCount < 4)
                        {
                            continue;
                        }
                        else if (mineralCount >= 4)
                        {
                            map[x, y] = firstMineralVal;
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

    public List<int[]> GetSurface() {
        return surface;
    }


    public int GetSurroundingGroundCount(int gridX, int gridY, int val) 
    {
        //Fonction qui nous permet de compter les occurences de ground ou cave/mineral autour de chaque tile
        int groundCount = 0;

        for (int neighbourX = gridX -1; neighbourX <= gridX+1; neighbourX++)
        {
            for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++) 
            {
                if (neighbourX >= 0 && neighbourX < width && neighbourY >= 0 && neighbourY < height) //On s'assure d'être à l'intérieur de notre map
                {
                    if (neighbourX != gridX || neighbourY != gridY) //Exclusion de la tile au milieu
                    {
                        if (map[neighbourX, neighbourY] != 0 && map[neighbourX, neighbourY] != val) //Si on a plus d'un groundTile comme voisin, on incrémente la variable groundCount
                        {
                            groundCount++;
                        }
                    }
                }
            }
        }

        return groundCount;
    }

    public int GetSurroundingMineralCount(int gridX, int gridY, int val)
    {
        //Fonction qui nous permet de compter les occurences de ground ou cave/mineral autour de chaque tile
        int mineralCount = 0;

        for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++)
        {
            for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++)
            {
                if (neighbourX >= 0 && neighbourX < width && neighbourY >= 0 && neighbourY < height) //On s'assure d'être à l'intérieur de notre map
                {
                    if (neighbourX != gridX || neighbourY != gridY) //Exclusion de la tile au milieu
                    {
                        if (map[neighbourX, neighbourY] != 0 && map[neighbourX, neighbourY] == val) //Si on a plus d'un mineralTile comme voisin, on incrémente la variable groundCount
                        {
                            mineralCount++;
                        }
                    }
                }
            }
        }

        return mineralCount;
    }

    public void BiomeNumAssignation() //Crée par Amine et amélioré par chatGPT
    {
        System.Random random = new System.Random();

        // Randomize the order of biomes (TLDR)
        // /Initialization of Lists: We start by initializing two lists, biomeTilemaps and biomeTiles, to store the available tilemaps and tiles for the biomes, respectively.
        List<Tilemap> biomeTilemaps = new List<Tilemap>();
        List<TileBase> biomeTiles = new List<TileBase>();


        // Adding Available Biomes: We check each biome to see if it's available (i.e., if the biome value is greater than 0 and the corresponding tile is not null). If it is, we add its tilemap and tile to the lists.
        if (firstBiomeVal > 0 && firstTile != null)
        {
            biomeTilemaps.Add(firstTileMap);
            biomeTiles.Add(firstTile);
        }

        if (secondBiomeVal > 0 && secondTile != null)
        {
            biomeTilemaps.Add(secondTileMap);
            biomeTiles.Add(secondTile);
        }

        if (thirdBiomeVal > 0 && thirdTile != null)
        {
            biomeTilemaps.Add(thirdTileMap);
            biomeTiles.Add(thirdTile);
        }

        // Shuffle the biome order (TLDR)
        // Shuffling the Lists: After adding the available biomes to the lists, we shuffle these lists to randomize the order of the biomes. We achieve this by iterating over the lists and swapping elements randomly.
        for (int i = 0; i < biomeTiles.Count; i++)
        {
            int randomIndex = random.Next(i, biomeTiles.Count);
            var tempTilemap = biomeTilemaps[i];
            biomeTilemaps[i] = biomeTilemaps[randomIndex];
            biomeTilemaps[randomIndex] = tempTilemap;

            var tempTile = biomeTiles[i];
            biomeTiles[i] = biomeTiles[randomIndex];
            biomeTiles[randomIndex] = tempTile;
        }

        // Assign the shuffled tiles to biome tilemaps (TLDR)
        // Assigning Shuffled Biomes to Tilemaps: Finally, we assign the shuffled tiles to the corresponding biome tilemaps. We iterate over the shuffled lists and assign each tile to the appropriate tilemap, handling cases where the tilemap or tile might be null.
        for (int i = 0; i < biomeTilemaps.Count; i++)
        {
            if (biomeTilemaps[i] != null && biomeTiles[i] != null)
            {
                switch (i)
                {
                    case 0:
                        firstBiomeTileMap = biomeTilemaps[i];
                        firstBiomeTile = biomeTiles[i];
                        biomeNames[firstBiomeVal] = firstBiomeDrops;
                        break;
                    case 1:
                        secondBiomeTileMap = biomeTilemaps[i];
                        secondBiomeTile = biomeTiles[i];
                        biomeNames[secondBiomeVal] = secondBiomeDrops;
                        break;
                    case 2:
                        thirdBiomeTileMap = biomeTilemaps[i];
                        thirdBiomeTile = biomeTiles[i];
                        biomeNames[thirdBiomeVal] = thirdBiomeDrops;
                        break;
                }
            }
        }

        // This process ensures that the available biomes are randomly assigned to the available tilemaps while handling cases where some biomes or tilemaps might be null.
    }


    public void RenderMap(int[,] map)
    {
        for(int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++)
            {

                switch (map[x, y])
                {
                    case var value when value == firstBiomeVal:
                        firstBiomeTileMap.SetTile(new Vector3Int(x, y, 0), firstBiomeTile);
                        break;
                    case var value when value == secondBiomeVal:
                        secondBiomeTileMap.SetTile(new Vector3Int(x, y, 0), secondBiomeTile);
                        break;
                    case var value when value == thirdBiomeVal:
                        thirdBiomeTileMap.SetTile(new Vector3Int(x, y, 0), thirdBiomeTile);
                        break;
                    case var value when value == caveVal: // BIZARRE OUI, MAIS REGLE UN BUG DE UNITY. J'AI TROUVÉ CE TRUC SUR STACK OVER FLOW :) ca prenait pas juste caveVal
                        caveTileMap.SetTile(new Vector3Int(x, y, 0), caveTile);
                        break;
                    case var value when value == firstMineralVal: // BIZARRE OUI, MAIS REGLE UN BUG DE UNITY. J'AI TROUVÉ CE TRUC SUR STACK OVER FLOW :) ca prenait pas juste caveVal
                        //caveTileMap.SetTile(new Vector3Int(x, y, 0), caveTile);
                        mineralTileMap.SetTile(new Vector3Int(x, y, 0), mineralTile);
                        break;
                    case var value when value == 0:
                        if (firstBiomeTileMap != null && firstBiomeTile != null) firstBiomeTileMap.SetTile(new Vector3Int(x, y, 0), null);
                        if (secondBiomeTileMap != null && secondBiomeTile) secondBiomeTileMap.SetTile(new Vector3Int(x, y, 0), null);
                        if (thirdBiomeTileMap != null && thirdBiomeTile) thirdBiomeTileMap.SetTile(new Vector3Int(x, y, 0), null);
                        break;
                    default:
                        //rien for now
                        break;
                }
            }
        }

    }

    public void clearMap() 
    {
        firstTileMap.ClearAllTiles();
        caveTileMap.ClearAllTiles();
    }

    public List<int[]> getSurface() {
        return surface;
    }

    public void updateMap(int x, int y, int val)
    {
        map[x, y] = val;
        backgroundTileMap.SetTile(new Vector3Int(x, y, 0), null);
    }

    public void SpawnCharacter()
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
            //characterPrefab.transform.position = spawnPosition;

        }
        else
        {
            Debug.LogWarning("Surface list is empty. Make sure to generate the terrain first." + surface.Count);
        }
    }

    public void placeMinerals(int[,] map)
    {
        System.Random pseudoRandom = new System.Random(seed.GetHashCode()); //Nous donne des valeurs aléatoires selon notre seed
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < perlinHeightList[x]; y++)
            {
                if(map[x, y] != caveVal && map[x, y] != firstMineralVal && map[x, y] != 0)
                {
                    if(pseudoRandom.Next(1, 100) > randomFillPercentMinerals)
                    {
                        map[x, y] = firstMineralVal;
                    }
                    //map[x, y] = (pseudoRandom.Next(1, 100) < randomFillPercentMinerals) ? num : firstMineral; //si la valeur du pseudoRandom est plus grande que le randomFillPercent, on place un groundTile/iceTile/desolationTile, sinon un mineral
                
                }
            }
        }

    }

    public string getTileVal(int x, int y)
    {
        return biomeNames[map[x, y]];
    }

    public int[,] getMap()
    {
        return map;
    }

    public void setMap(int[,] map)
    {
        this.map = map;
    }

}
