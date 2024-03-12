using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ProceduralGeneration : MonoBehaviour
{
    [SerializeField] int width, height;
    [SerializeField] float smoothness;
    [SerializeField] float seed;
    [SerializeField] TileBase groundTile;
    [SerializeField] Tilemap groundTileMap;
    int[,] map;


    // Start is called before the first frame update
    void Start()
    {
        Generation();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Generation();
        }
    }

    public void Generation()
    {
        groundTileMap.ClearAllTiles();
        map = GenerateArray(width, height, true);
        map = TerrainGeneration(map);
        RenderMap(map, groundTileMap, groundTile);
    }

    public int[,] GenerateArray(int width, int height, bool empty) { 

        int[,] map = new int[width, height];   

        for(int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                map[x,y] = (empty) ? 0 : 1;
            }
        }

        return map;
    }

    public int[,] TerrainGeneration(int[,] map)
    {
        int perlinhHeight;
        for (int x = 0;x < width; x++)
        {
            perlinhHeight = Mathf.RoundToInt(Mathf.PerlinNoise(x / smoothness, seed) * height / 2);
            perlinhHeight += height / 2;
            for (int y = 0; y < perlinhHeight; y++)
            {
                map[x, y] = 1;
            }
        }
        return map;
    }

    public void RenderMap(int[,] map, Tilemap groundTileMap, TileBase groundTileBase)
    {
        for(int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++)
            {
                if (map[x, y] == 1)
                {
                    groundTileMap.SetTile(new Vector3Int(x, y, 0), groundTileBase);
                }
            }
        }

    }
}
