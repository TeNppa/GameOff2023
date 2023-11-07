
using System;
using UnityEngine;
using UnityEngine.Tilemaps;

[Serializable]
public struct TileTypes {
    public TileBase Ground1;
    public TileBase Ground2;
    public TileBase Ground3;
    public TileBase GroundDragon;
    public TileBase Gold;

    public TileBase[] GetArray()
    {
        var array = new TileBase[5];
        array[0] = Ground1;
        array[1] = Ground2;
        array[2] = Ground3;
        array[3] = GroundDragon;
        array[4] = Gold;
        return array;
    }
}

