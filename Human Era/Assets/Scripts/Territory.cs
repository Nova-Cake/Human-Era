using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Territory
{
    string nameID;
    Color32 color;
    int id;
    public List<(int, int)> pixels = new List<(int, int)>();
    public Vector2 position;

    public Territory(int id, Color32 color, string name)
    {
        this.nameID = name;
        this.color = color;
        this.id = id;
    }

    public void FindPosition(MapInfo mapInfo)
    {
        int x = 0;
        int y = 0;

        foreach((int, int) pixel in pixels)
        {
            x += pixel.Item1;
            y += pixel.Item2;
        }

        x /= pixels.Count;
        y /= pixels.Count;

        mapInfo.map.SetPixel(x, y, new Color32(color.r, color.g, color.b, 25));
        Debug.Log(this);
        mapInfo.map.Apply();

        float posX = (x / mapInfo.pixelsPerUnit) - ( ( mapInfo.map.width / mapInfo.pixelsPerUnit) / 2);
        float posY = (y / mapInfo.pixelsPerUnit) - ( ( mapInfo.map.height / mapInfo.pixelsPerUnit) / 2);

        position = new Vector2(posX, posY);
    }

    public override string ToString()
    {
        return $"{nameID}({id})";
    }
}
