using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "NewRandomTile", menuName = "Tiles/Random Tile")]
public class CustomRandomTile : TileBase
{
    [SerializeField] Sprite[] sprites;

    public override void GetTileData(Vector3Int location, ITilemap tilemap, ref TileData tileData)
    {
        if (sprites != null && sprites.Length > 0)
        {
            tileData.sprite = sprites[Random.Range(0, sprites.Length)];
        }
    }
}
