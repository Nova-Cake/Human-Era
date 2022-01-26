using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapLoading : MonoBehaviour
{
    MapInfo mapInfo;
    Texture2D map;
    GameObject mapSprite;

    Camera mainCamera;

    [SerializeField]
    LayerMask mapLayer;

    [SerializeField]
    [Range(0, 255)]
    byte mapTransparency;

    [SerializeField]
    float pixelsPerUnit;

    bool updateMap;
    public Color selectedColor;

    bool brushSize1 = true;

    GameObject debugCube;

    void Awake()
    {
        mapInfo = gameObject.GetComponent<MapInfo>();
        mainCamera = Camera.main;
        pixelsPerUnit = mapInfo.pixelsPerUnit;
        mapSprite = mapInfo.mapSprite;

        map = mapInfo.map;
        map.Apply();
        
        mapSprite.GetComponent<SpriteRenderer>().sprite = Sprite.Create(map, new Rect(0.0f, 0.0f, map.width, map.height), new Vector2(0.5f, 0.5f), pixelsPerUnit);

        if(mapSprite.GetComponent<BoxCollider2D>() == null)
        {
            mapSprite.AddComponent<BoxCollider2D>();
        }
    }

    void Update()
    {
        HandleCameraMovement();
        HandleInput();
    }

    void HandleCameraMovement()
    {
        Vector3 position = mainCamera.transform.localPosition;

        float deltaX = Input.GetAxis("Horizontal") * .1f * (.25f + mainCamera.fieldOfView * .02f);
        float deltaY = Input.GetAxis("Vertical") * .1f * (.25f + mainCamera.fieldOfView * .02f);;
        float deltaZoom = -Input.GetAxis("Mouse ScrollWheel") * 10f * (1 + mainCamera.fieldOfView * .02f);

        position.x = Mathf.Clamp( position.x + deltaX, -(map.width / pixelsPerUnit) / 2f, (map.width / pixelsPerUnit) / 2f);
        position.y = Mathf.Clamp( position.y + deltaY, -(map.height / pixelsPerUnit) / 2f, (map.height / pixelsPerUnit) / 2f);

        mainCamera.transform.localPosition = position;
        mainCamera.fieldOfView = Mathf.Clamp(deltaZoom + mainCamera.fieldOfView, 5, 100);
    }

    void HandleInput()
    {
        updateMap = false;
        if(Input.GetMouseButtonDown(0))
        {
            Vector3 screenPos = Input.mousePosition;
            screenPos.z = 0 - mainCamera.transform.position.z;
            Vector3 mousePos = mainCamera.ScreenToWorldPoint(screenPos); 

            RaycastHit2D hit = Physics2D.GetRayIntersection(mainCamera.ScreenPointToRay(Input.mousePosition));

            if(hit.collider != null)
            {
                Vector3 hitPoint = hit.point;
                int x = Mathf.FloorToInt(hitPoint.x * pixelsPerUnit) + (map.width / 2);
                int y = Mathf.FloorToInt(hitPoint.y * pixelsPerUnit) + (map.height / 2);

                Territory territory = mapInfo.mapMaker.FromPixel(x, y);
                Vector3 position = territory.position;

                map.Apply();
            }
        }

        if(updateMap)
        {
            map.Apply();
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            brushSize1 = !brushSize1;
        }
    }

    (int, int)[] GetHighlightedPixels(int x, int y)
    {
        if(brushSize1)
        {
            return new (int, int)[] {(x, y)};
        }
        else
        {
            return new (int, int)[] 
            {
                (x, y),
                (x + 1, y),
                (x - 1, y),
                (x, y + 1),
                (x, y - 1),
            };
        }
    }
}
