using System;
using System.Collections.Generic;
using System.Linq;
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
    [Range(0, 1)]
    private double GoldChance;
    
    public TileTypes tiles;
    private Tilemap tilemap;
    private int[,] map;
    private Random rnd;
    
    private float t = 0;
    private void Start()
    {
        rnd = new Random((int)DateTime.Now.Ticks);
        tilemap = gameObject.GetComponent<Tilemap>();
        GenerateArray();
        RenderMap();
    }

    private void Update()
    {
        if (t > 2)
        {
            t = 0;
            GenerateArray();
            RenderMap();
        }
        else
        {
            t += Time.deltaTime;
        }
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
        var layerMap = new int[Width, LayerHeights[layer]];
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < LayerHeights[layer]; y++)
            {
                layerMap[x, y] = rnd.NextDouble() > GoldChance ? layer + 1 : 5;
            }
        }

        return layerMap;
    }

    public void AddLayerToMap(int[,] layerMap, int layer)
    {
        var yOffset = LayerHeights.Take(layer).Sum();
        Debug.Log("layer: " + layer + ", yOffset: " + yOffset);
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
}
