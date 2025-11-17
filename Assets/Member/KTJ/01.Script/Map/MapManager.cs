using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoSingleton<MapManager>
{
    public Tilemap tilemap;

    public int GetTileCount()
    {
        int count = 0;
        BoundsInt bounds = tilemap.cellBounds;

        foreach(var pos in bounds.allPositionsWithin)
        {
            if (tilemap.GetTile(pos) != null)
            {
                count++;
            }
        }

        return count;
    }
}
