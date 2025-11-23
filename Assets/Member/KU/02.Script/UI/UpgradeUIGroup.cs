using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UpgradeUIGroup : MonoBehaviour
{
    [SerializeField] private Image levelTilePref;

    [SerializeField] private List<Image> levelTileList = new();

    public void UpgradeLevelTileSpawn(BuildingSO buildData, int level)
    {
        foreach (var tile in levelTileList)
            Destroy(tile.gameObject);

        levelTileList.Clear();

        for (int i = 0; i < buildData.maxLevel; i++)
        {
            var levelTile = Instantiate(levelTilePref, transform);
            levelTileList.Add(levelTile);
            levelTile.rectTransform.sizeDelta = new Vector2(-15 * buildData.maxLevel + 125 , levelTile.rectTransform.sizeDelta.y);
            if(i < level)
            {
                levelTileList[i].color = Color.green;
            }
            else
            {
                levelTileList[i].color = Color.white;
            }
        }
    }

    public void SetUpgrade(BuildingSO buildData, int level)
    {
        for (int i = 0; i < buildData.maxLevel; i++)
        {
            if (i < level)
            {
                levelTileList[i].color = Color.green;
            }
            else
            {
                levelTileList[i].color = Color.white;
            }
        }
    }
}
