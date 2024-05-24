// ProceduralGeneration
// Biblioth�que qui permet de g�m�rer une carte al�atoire, selon le nombre de biome, la hauteur et largeur que l'utilisateur met dans le script dans Unity
// Auteur: Amine Fanid
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[DefaultExecutionOrder(25)]

// Ici nous avons utilis� un tutoriel pour partir notre libraire et nous avons d�velopp� dessus pour obtenir ce r�sultat final.
// Le tutoriel a �t� film� par ChronoABI, il a mis comme r�f�rence un site qui contient des fonctions qu'on utilise dans notre code.
// ref 1: https://youtu.be/neTvQEDhZZM?si=7Bdp6xrINJxV7MQU (Vid�o de ChronoABI)
// ref 1.5 : https://blog.unity.com/engine-platform/procedural-patterns-you-can-use-with-tilemaps-part-1 (Site web r�f�renc� par ChronoABI)
// ref 2: https://youtu.be/l5KVBDOsHfg?si=_RxjTLFp0sT07Zml (Vid�o de ChronoABI)
// ref 2.5 : https://blog.unity.com/engine-platform/procedural-patterns-to-use-with-tilemaps-part-2 (Site web r�f�renc� par ChronoABI)
//Les fonctions qui ont �t� inspir�/fond� sur le code de ChronoABI arboreront ce commentaire : "// Inspir� ou provenant de la vid�o de ChronoABI"

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

    void Start()
    {

    }

    private void Update()
    {
        if (map.GetLength(0) > 0) // Limite l'appel de RenderMap() pour les nouveaux usagers, tant qu'ils n'ont pas de Map compl�te
        {
            RenderMap(map);
        }
    }

    public int BiomeCount() //Retourne le nombre de biomes existant
    {
        int biomeCount = 0;
        if (firstTile != null) biomeCount++;
        if (secondTile != null) biomeCount++;
        if (thirdTile != null) biomeCount++;
        return biomeCount;
    }

    public void prepareProceduralGeneration()
    {
        biomeCount = BiomeCount(); 
        BiomeRandomization();
        perlinHeightList = new int[width]; 
        surface = new List<int[]>();
        biomeNames = new Dictionary<int, string>();
        if (mineralTile != null)
        {
            biomeNames[firstMineralVal] = firstMineralDrops;
        }
    }

    public void BiomeRandomization()
    {
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


    public void Generation() // Inspir� ou provenant de la vid�o de ChronoABI
    {
        clearMap();
        if (map.GetLength(0) <= 0)
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
    }

    public int[,] GenerateArray(int width, int height, bool empty) // Inspir� ou provenant de la vid�o de ChronoABI
    { 

        int[,] map = new int[width, height];   

        for(int x = 0; x < width; x++) //loop � travers la width de notre map
        {
            for (int y = 0; y < height; y++) // loop � travers la hauteur de notre map
            {
                map[x,y] = (empty) ? 0 : 1; 
            }
        }

        return map;
    }

    public int MapWidthDivision(int xWidth)
    {
        int sectionWidth = width / biomeCount; // Calcul la largeur des sections de biomes

        // Determine le biome bas� sur la position en x
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
                return firstBiomeVal; // Return la valeur de 1er biome par d�faut
        }
    }

    public int[,] TerrainGeneration(int[,] map) // Inspir� ou provenant de la vid�o de ChronoABI
    {
        System.Random pseudoRandom = new System.Random(seed.GetHashCode()); //Nous donne des valeurs al�atoires selon notre seed
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

    public void smoothMap(int nbSmoothing) // Inspir� ou provenant de la vid�o de ChronoABI
    {
        int num = firstBiomeVal;
        for (int i = 0; i < nbSmoothing; i++)
        {
            for (int x = 0; x < width; x++) //loop � travers la width de notre map
            {
                for (int y = 0; y < perlinHeightList[x]; y++) //loop � travers une liste de nos valeur de PerlinHeignt
                {
                    if (x == 0 || y == 0 || x == width - 1 || y == perlinHeightList[x] - 1) //Pour cr�er une bordure autour de la map et ainsi �viter que les caves soient expos�s
                    {
                        num = MapWidthDivision(x);
                        map[x, y] = num;
                    }
                    else
                    {
                        //Moore's neighborhood
                        int surroundingGroundCount = GetSurroundingGroundCount(x,y, caveVal);
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

    public void mineralSmoothing(int nbSmoothingMinerals) // Inspir� ou provenant de la vid�o de ChronoABI
    {
        for (int i = 0; i < nbSmoothingMinerals; i++)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < perlinHeightList[x]; y++)
                {
                    // Si la tile n'est pas un mineral, on check ses voisins
                    if (map[x, y] != firstMineralVal)
                    {
                        // Compte le nombre de tile de mineral autour
                        int mineralCount = GetSurroundingMineralCount(x, y, firstMineralVal);

                        // Si on a au moins 4 tiles autour qui sont des tiles de mineral, on transforme cette tile en mineral
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
        //Iteration vers le haut, � partir du tile courant, jusqua ce qu'un tile vide soit trouv�
        for (int y = gridY; y < height; y++)
        {
            // Check si le tile courant est vide
            if (map[gridX, y] == 0)
            {
                surface.Add(new int[] { gridX, y+2 }); // Ajoute le tile � la surface dans ma liste surface
                return; // Sort de la fonction quand on a trouv� la tile de surface
            }
        }
    }

    public List<int[]> GetSurface() {
        return surface;
    }


    public int GetSurroundingGroundCount(int gridX, int gridY, int val) // Inspir� ou provenant de la vid�o de ChronoABI
    {
        //Fonction qui nous permet de compter les occurences de ground ou cave/mineral autour de chaque tile
        int groundCount = 0;

        for (int neighbourX = gridX -1; neighbourX <= gridX+1; neighbourX++)
        {
            for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++) 
            {
                if (neighbourX >= 0 && neighbourX < width && neighbourY >= 0 && neighbourY < height) //On s'assure d'�tre � l'int�rieur de notre map
                {
                    if (neighbourX != gridX || neighbourY != gridY) //Exclusion de la tile au milieu
                    {
                        if (map[neighbourX, neighbourY] != 0 && map[neighbourX, neighbourY] != val) //Si on a plus d'un groundTile comme voisin, on incr�mente la variable groundCount
                        {
                            groundCount++;
                        }
                    }
                }
            }
        }

        return groundCount;
    }

    public int GetSurroundingMineralCount(int gridX, int gridY, int val) // Inspir� ou provenant de la vid�o de ChronoABI
    {
        //Fonction qui nous permet de compter les occurences de ground ou cave/mineral autour de chaque tile
        int mineralCount = 0;

        for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++)
        {
            for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++)
            {
                if (neighbourX >= 0 && neighbourX < width && neighbourY >= 0 && neighbourY < height) //On s'assure d'�tre � l'int�rieur de notre map
                {
                    if (neighbourX != gridX || neighbourY != gridY) //Exclusion de la tile au milieu
                    {
                        if (map[neighbourX, neighbourY] != 0 && map[neighbourX, neighbourY] == val) //Si on a plus d'un mineralTile comme voisin, on incr�mente la variable groundCount
                        {
                            mineralCount++;
                        }
                    }
                }
            }
        }

        return mineralCount;
    }

    public void BiomeNumAssignation() //Cr��e par Amine et am�lior�/compl�t� par chatGPT
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


    public void RenderMap(int[,] map) // Inspir� ou provenant de la vid�o de ChronoABI
    {
        for(int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++)
            {

                switch (map[x, y])
                {
                    // "case var value when value" N�cessaire, ref : https://stackoverflow.com/questions/7593377/switch-case-in-c-sharp-a-constant-value-is-expected
                    case var value when value == firstBiomeVal:
                        firstBiomeTileMap.SetTile(new Vector3Int(x, y, 0), firstBiomeTile);
                        break;
                    case var value when value == secondBiomeVal:
                        secondBiomeTileMap.SetTile(new Vector3Int(x, y, 0), secondBiomeTile);
                        break;
                    case var value when value == thirdBiomeVal:
                        thirdBiomeTileMap.SetTile(new Vector3Int(x, y, 0), thirdBiomeTile);
                        break;
                    case var value when value == caveVal: 
                        caveTileMap.SetTile(new Vector3Int(x, y, 0), caveTile);
                        break;
                    case var value when value == firstMineralVal:
                        mineralTileMap.SetTile(new Vector3Int(x, y, 0), mineralTile);
                        break;
                    case var value when value == 0:
                        if (firstBiomeTileMap != null && firstBiomeTile != null) firstBiomeTileMap.SetTile(new Vector3Int(x, y, 0), null);
                        if (secondBiomeTileMap != null && secondBiomeTile) secondBiomeTileMap.SetTile(new Vector3Int(x, y, 0), null);
                        if (thirdBiomeTileMap != null && thirdBiomeTile) thirdBiomeTileMap.SetTile(new Vector3Int(x, y, 0), null);
                        break;
                    default:
                        break;
                }
            }
        }

    }

    public void clearMap() // Inspir� ou provenant de la vid�o de ChronoABI
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
            // S�lectionne une position al�atoire sur la surface de la Map
            int randomIndex = UnityEngine.Random.Range(0, surface.Count);
            int[] surfaceTile = surface[randomIndex];

            // Calcule la position ou on va faire appara�tre le character
            Vector3 spawnPosition = new Vector3(surfaceTile[0], surfaceTile[1], 0);

            // Fait appara�tre le character � la position choisi
            Instantiate(characterPrefab, spawnPosition, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("Surface list is empty. Make sure to generate the terrain first." + surface.Count);
        }
    }

    public void placeMinerals(int[,] map)
    {
        System.Random pseudoRandom = new System.Random(seed.GetHashCode()); //Nous donne des valeurs al�atoires selon notre seed
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
