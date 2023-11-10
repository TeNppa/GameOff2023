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
    private List<GroundLayer> Layers;

    [SerializeField] 
    private List<ValuablesTiles> valuableTiles;

    private int Height => Layers.Sum(layer => layer.Height);
    [SerializeField] 
    private int wallThickness;
    [SerializeField]
    private Vector3Int mapOffset = new (-12, -2, 0);
    [SerializeField]
    [Tooltip("How many tiles in y axis to blend layerlines per direction")]
    private int layerOverlap;
    
    [SerializeField]
    private Tilemap groundTilemap;
    [SerializeField]
    private Tilemap valuablesTilemap;
    private int[,] groundMap;
    private int[,] valuablesMap;
    private Random rnd;
    
    private void Start()
    {
        rnd = new Random((int)DateTime.Now.Ticks);
        GenerateGroundArray();
        GenerateValuablesArray();
        RenderMaps();
    }

    void GenerateGroundArray()
    {
        groundMap = new int[Width + wallThickness, Height + wallThickness];
        for (var layer = 0; layer < Layers.Count; layer++)
        {
            AddLayerToMap(ref groundMap, GenerateGroundLayer(layer), layer);
        }
    }
    void GenerateValuablesArray()
    {
        valuablesMap = new int[Width, Height];
        for (var layer = 0; layer < Layers.Count; layer++)
        {
            AddLayerToMap(ref valuablesMap, GenerateValuableLayer(layer), layer);
        }
    }

    private int[,] GenerateValuableLayer(int layer)
    {
        var height = Layers[layer].Height;
        var layerMap = new int[Width, height];
        int x = 0;
        int y = 0;
        foreach (var valuable in Layers[layer].Valuables)
        {
            (int name, int amount) val = ((int)valuable.Name, (int)math.floor(valuable.Chance * (Width * height)));
            if (val.amount <= 0)
                continue;
            for (;x < Width && val.amount > 0; x++)
            {
                for (; y < height && val.amount > 0; y++, val.amount--)
                {
                    layerMap[x, y] = val.name + 1;
                }
                y = 0;
            }
        }
        for (;x < Width; x++)
        {
            for (; y < height; y++)
            {
                layerMap[x, y] = 0;
            }
            y = 0;
        }
        

        return ShuffleArray(rnd, layerMap);
    }

    int[,] GenerateGroundLayer(int layer)
    {
        var height = Layers[layer].Height;
        var layerMap = new int[Width, height];
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (height - y <= layerOverlap && layer < Layers.Count - 1) // Blending down
                {
                    layerMap[x, y] = rnd.NextDouble() < (double)(layerOverlap + height - y) / (layerOverlap * 2)
                        ? layer + 1
                        : layer + 2;
                }
                else if (y < layerOverlap && layer != 0) // Blending up
                {
                    layerMap[x, y] = rnd.NextDouble() < (double)(layerOverlap + y) / (layerOverlap * 2) ? layer + 1 : layer;
                }
                else // "normal"
                    layerMap[x, y] = layer + 1;
            }
        }

        
        return layerMap;
    }

    public void AddLayerToMap(ref int[,] map, int[,] layerMap, int layer)
    {
        var yOffset = Layers.Take(layer).Sum(l => l.Height);
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Layers[layer].Height; y++)
            {
                map[x, y + yOffset] = layerMap[x, y];
            }
        }
    }

    private void AddWalls()
    {
        for (int y = 0; y < Height + wallThickness; y++)
        {
            for (var x = 0; x < wallThickness; x++)
            {
                //map[x, y] = 
            }
        }
    }
    void RenderMaps()
    {
        groundTilemap.ClearAllTiles();
        valuablesTilemap.ClearAllTiles();
        for (int x = 0; x < Width ; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                var groundTile = groundMap[x, y];
                if (groundTile != 0)
                {
                    groundTilemap.SetTile(new Vector3Int(x, -y, 0) + mapOffset, Layers[groundTile-1].BaseTile);
                }

                var valTile = valuablesMap[x, y];
                if (valTile != 0)
                {
                    valuablesTilemap.SetTile(new Vector3Int(x, -y, 0) + mapOffset, valuableTiles.FirstOrDefault(valuable => (int)valuable.Name == (valTile -1)).tile);
                }

            }
        }
    }
    public static T[,] ShuffleArray<T>(Random random, T[,] array)
    {
        int lengthRow = array.GetLength(1);

        for (int i = array.Length - 1; i > 0; i--)
        {
            var i0 = i / lengthRow;
            var i1 = i % lengthRow;

            var j = random.Next(i + 1);
            int j0 = j / lengthRow;
            int j1 = j % lengthRow;

            (array[i0, i1], array[j0, j1]) = (array[j0, j1], array[i0, i1]);
        }

        return array;
    }

    public void Dig(Vector3 pos, float dmg)
    {
        var position = groundTilemap.WorldToCell(pos);
        groundTilemap.SetTile(new Vector3Int(position.x, position.y, 0), null);
    }
}
