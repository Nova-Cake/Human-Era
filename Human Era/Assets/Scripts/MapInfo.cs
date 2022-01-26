using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapInfo : MonoBehaviour
{
    public Texture2D map;
    public GameObject mapSprite;
    public int pixelsPerUnit;
    public GameObject debugCube;

    public MapMaker mapMaker;
    public MapLoading mapLoading;
}