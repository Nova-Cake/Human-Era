using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TerritoryGenerator : MonoBehaviour
{
    public List<Color32> colors = new List<Color32>();
    public List<(int, int, int)> invalidColors = new List<(int, int, int)>();

    public List<Territory> territories = new List<Territory>();
    public Dictionary<Color32, Territory> colorTerritory = new Dictionary<Color32, Territory>();

    public TextAsset csvFile;

    [HideInInspector]
    Texture2D map;
    MapInfo mapInfo;

    Territory fallBackTerritory = new Territory(-1, new Color32(0, 0, 0, 255), "FallBack");
    public TextMeshPro labelPrefab;

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
            TextMeshPro text = Instantiate<TextMeshPro>(labelPrefab);
            text.transform.localScale = new Vector3(.15f, .15f, .15f);
            text.transform.position = (Vector3)territory.position + new Vector3(0, 0, -.1f);
            text.text = territory.name;
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
        Color32 color = map.GetPixel(x, y);
        try 
        {
            return colorTerritory[color];
        }
        catch
        {
            Debug.Log($"Unknown Color {color} at {(x, y)}");
            map.SetPixel(x, y, fallBackTerritory.color);
            if(!invalidColors.Contains((color.r, color.g, color.b)))
            {
                invalidColors.Add((color.r, color.g, color.b));
            }
            return fallBackTerritory;
        }
    }
}
