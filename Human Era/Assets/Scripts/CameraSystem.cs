using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSystem : MonoBehaviour
{
    Camera mainCamera;
    
    [SerializeField]
    MapInfo mapInfo;

    void Awake()
    {
        mainCamera = Camera.main;
    }
    void HandleCameraMovement()
    {
        Vector3 position = mainCamera.transform.localPosition;

        float deltaX = Input.GetAxis("Horizontal") * .1f * (.25f + mainCamera.fieldOfView * .02f);
        float deltaY = Input.GetAxis("Vertical") * .1f * (.25f + mainCamera.fieldOfView * .02f);;
        float deltaZoom = -Input.GetAxis("Mouse ScrollWheel") * 10f * (1 + mainCamera.fieldOfView * .02f);

        position.x = Mathf.Clamp( position.x + deltaX, -(mapInfo.map.width / mapInfo.pixelsPerUnit) / 2f, (mapInfo.map.width / mapInfo.pixelsPerUnit) / 2f);
        position.y = Mathf.Clamp( position.y + deltaY, -(mapInfo.map.height / mapInfo.pixelsPerUnit) / 2f, (mapInfo.map.height / mapInfo.pixelsPerUnit) / 2f);

        mainCamera.transform.localPosition = position;
        mainCamera.fieldOfView = Mathf.Clamp(deltaZoom + mainCamera.fieldOfView, 5, 100);
    }
}
