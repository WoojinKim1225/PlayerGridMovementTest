using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    public static MapManager instance;

    [SerializeField] private Tilemap _map;
    [SerializeField] private GameObject _character;
    [SerializeField] private List<TileData> tileDataList;

    private Dictionary<TileBase, TileData> tileBase2Data;

    void Awake()
    {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(this);
        }

        tileBase2Data = new Dictionary<TileBase, TileData>();
        foreach (var tileData in tileDataList) {
            foreach (var tile in tileData.tiles) {
                tileBase2Data.Add(tile, tileData);
            }
        }
    }

    void Update()
    {
        /*
        Vector2 characterPosition = _character.transform.position;
        Vector3Int gridPosition = _map.WorldToCell(characterPosition);
        TileBase currentTile = _map.GetTile(gridPosition);
        if (currentTile == null) return;
        float walkingSpeed = tileBase2Data.ContainsKey(currentTile) ? tileBase2Data[currentTile].walkingSpeed : -1;
        Debug.Log("this Tile:" + currentTile + ", " + walkingSpeed);
        */
    }
}