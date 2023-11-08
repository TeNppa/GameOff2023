using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = System.Random;

public class MapManager : MonoBehaviour
{
    [SerializeField]
    private int Width = 24;
    [SerializeField]
    private List<int> LayerHeights;
    private int Height => LayerHeights.Sum();
    [SerializeField]
    private Vector3Int mapOffset = new (-12, -2, 0);
    [SerializeField]
    [Tooltip("How many tiles in y axis to blend layerlines per direction")]
    private int layerOverlap;
    [SerializeField]
    [Range(0, 1)]
    private double GoldChance;
    
    public TileTypes tiles;
    private Tilemap tilemap;
    private int[,] map;
    private Random rnd;
    
    private void Start()
    {
        rnd = new Random((int)DateTime.Now.Ticks);
        tilemap = gameObject.GetComponent<Tilemap>();
        GenerateArray();
        RenderMap();
    }

    private void Update()
    {
    }

    void GenerateArray()
    {
        map = new int[Width, Height];
        for (var layer = 0; layer < LayerHeights.Count; layer++)
        {
            AddLayerToMap(GenerateLayer(layer), layer);
        }
    }

    int[,] GenerateLayer(int layer)
    {
        var height = LayerHeights[layer];
        var layerMap = new int[Width, height];
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (height - y <= layerOverlap && layer < LayerHeights.Count - 1) // Blending down
                {
                    layerMap[x, y] = rnd.NextDouble() > GoldChance ? 
                        rnd.NextDouble() < (double)(layerOverlap + height - y) / (layerOverlap * 2)  ? layer + 1 : layer + 2
                        : 5;
                }
                else if (y < layerOverlap && layer != 0) // Blending up
                {
                    layerMap[x, y] = rnd.NextDouble() > GoldChance ? 
                        rnd.NextDouble() < (double)(layerOverlap + y) / (layerOverlap * 2)  ? layer + 1 : layer
                        : 5;
                }
                else // "normal"
                    layerMap[x, y] = rnd.NextDouble() > GoldChance ? layer + 1 : 5;
            }
        }

        return layerMap;
    }

    public void AddLayerToMap(int[,] layerMap, int layer)
    {
        var yOffset = LayerHeights.Take(layer).Sum();
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < LayerHeights[layer]; y++)
            {
                map[x, y + yOffset] = layerMap[x, y];
            }
        }
    }
    void RenderMap()
    {
        tilemap.ClearAllTiles();
        
        for (int x = 0; x < Width ; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                var tile = map[x, y];
                if (tile != 0)
                {
                    tilemap.SetTile(new Vector3Int(x, -y, 0) + mapOffset, tiles.GetArray()[tile - 1]);
                }
            }
        }
    }
    public static void ShuffleArray<T>(Random random, T[,] array)
    {
        int lengthRow = array.GetLength(1);

        for (int i = array.Length - 1; i > 0; i--)
        {
            int i0 = i / lengthRow;
            int i1 = i % lengthRow;

            int j = random.Next(i + 1);
            int j0 = j / lengthRow;
            int j1 = j % lengthRow;

            (array[i0, i1], array[j0, j1]) = (array[j0, j1], array[i0, i1]);
        }
    }

    public void Dig(Vector3 pos, float dmg)
    {
        var position = tilemap.WorldToCell(pos);
        tilemap.SetTile(new Vector3Int(position.x, position.y, 0), null);
    }
}
