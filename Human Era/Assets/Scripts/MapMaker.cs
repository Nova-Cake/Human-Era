using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMaker : MonoBehaviour
{
    public List<Color32> colors = new List<Color32>();
    public List<Territory> territories = new List<Territory>();
    public Dictionary<Color32, Territory> colorTerritory = new Dictionary<Color32, Territory>();

    public TextAsset csvFile;

    [HideInInspector]
    Texture2D map;
    MapInfo mapInfo;
    

    void Awake()
    {
        mapInfo = gameObject.GetComponent<MapInfo>();
        map = mapInfo.map;
        ParseMapData();
        for(int x = 0; x < map.width; x++)
        {
            for(int y = 0; y < map.height; y++)
            {
                FromPixel(x, y).pixels.Add((x, y));
            }
        }
    }

    void Start()
    {
        foreach(Territory territory in territories)
        {
            territory.FindPosition(mapInfo);
        }
    }

    public void ParseMapData()
    {
        string[] entries = csvFile.text.Split('\n');
        for(int i = 1; i < entries.Length; i++)
        {
            string entry = entries[i];
            string[] values = entry.Split(',');
            int id = int.Parse(values[0]);
            Color32 color = new Color32(byte.Parse(values[1]), byte.Parse(values[2]), byte.Parse(values[3]), 255);
            string name = values[4];
            Territory territory = new Territory(id, color, name);

            colorTerritory.Add(color, territory);
            colors.Add(color);
            territories.Add(territory);
        }
    }

    public Territory FromPixel(int x, int y)
    {
        return colorTerritory[(Color32)map.GetPixel(x, y)];
    }
}
