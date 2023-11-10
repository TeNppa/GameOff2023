using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum Grounds
{
    Mud,
    Dirt,
    Stone
}
[Serializable]
public struct GroundTiles
{
    public Grounds Name;
    public TileBase Tile;
}
