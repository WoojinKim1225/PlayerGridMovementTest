using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "TileData", menuName = "TileData", order = 0)]
public class TileData : ScriptableObject {
    public TileBase[] tiles;

    public float walkingSpeed;
}

