using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script in charge of having the Camera follow a subject or to stay in place.
/// </summary>
public class CameraFollow : MonoBehaviour
{
    private Camera gameCamera;
    private UnitMove focusUnit;
    private Vector3 offset;
    private Vector3 velocityRef;
    private Vector3 arbitraryPosition;
    private byte cameraToFollow; //0: Follow player, 1: select place
    private float xOffset;

    public Vector2 cameraXClamp;
    public Vector2 cameraYClamp;

    private void Awake()
    {
        cameraToFollow = 1;
        gameCamera = GetComponent<Camera>();
        focusUnit = FindObjectOfType<PlayerMove>();
        GameManager.Instance.SetCamera(GetComponent<Camera>());
    }
    private void Start()
    {
        xOffset = 0;
        offset = new Vector3(0, 4f, -100f);
        arbitraryPosition = new Vector3(0, 0, -100f);
    }
    private void Update()
    {
        if (cameraToFollow == 1)
        {
            if (Mathf.Abs(focusUnit.HorizontalSpeed()) > 0)
            {
                if (focusUnit.transform.localScale.x == 1)
                {
                    //Add
                    if (xOffset < 3f)
                    {
                        xOffset += (xOffset < 0f) ? Time.deltaTime * 15f : Time.deltaTime * 5f;
                        xOffset = Mathf.Clamp(xOffset, -3f, 3f);
                    }
                }
                else
                {
                    //Subtract
                    if (xOffset > -3f)
                    {
                        xOffset -= (xOffset > 0f) ? Time.deltaTime * 15f : Time.deltaTime * 5f;
                        xOffset = Mathf.Clamp(xOffset, -3f, 3f);
                    }
                }
            }
        }
    }
    private void LateUpdate()
    {
        switch(cameraToFollow)
        {
            case 0:
                //Stop the camera completely
                break;
            case 1:
                if (focusUnit == null)
                {
                    return;
                }
                offset = new Vector3(xOffset, offset.y, offset.z);
                gameCamera.transform.position = Vector3.Lerp(focusUnit.transform.position + offset,
                    focusUnit.transform.position + offset, 2f * Time.fixedDeltaTime);
                break;
            case 2:
                gameCamera.transform.position = Vector3.SmoothDamp(gameCamera.transform.position, arbitraryPosition, ref velocityRef, 0.1f);
                break;
        }
        Vector3 clampedPosition = new Vector3(Mathf.Clamp(gameCamera.transform.position.x, cameraXClamp.x, cameraXClamp.y),
            Mathf.Clamp(gameCamera.transform.position.y, cameraYClamp.x, cameraYClamp.y), offset.z);
        gameCamera.transform.position = clampedPosition;
    }

    /// <summary>
    /// Stop the camera in its tracks.
    /// </summary>
    public void StopCamera()
    {
        cameraToFollow = 0;
    }
    /// <summary>
    /// Make the camera follow the player.
    /// </summary>
    public void CameraFollowPlayer()
    {
        if (focusUnit == null)
        {
            focusUnit = FindObjectOfType<PlayerMove>();
        }
        cameraToFollow = 1;
        xOffset = 0;
        offset = new Vector3(xOffset, offset.y, offset.z);
        gameCamera.orthographicSize = 10;
    }
    /// <summary>
    /// Make the camera follow the specified Unit.
    /// </summary>
    /// <param name="unit"></param>
    public void CameraFollowUnit(UnitMove unit)
    {
        focusUnit = unit;
        cameraToFollow = 1;
        xOffset = 0;
        offset = new Vector3(xOffset, offset.y, offset.z);
        gameCamera.orthographicSize = 10;
    }
    /// <summary>
    /// Set the arbitrary position for the camera.
    /// </summary>
    /// <param name="arbitraryPosition"></param>
    public void SetArbitraryPosition(Vector3 arbitraryPosition)
    {
        cameraToFollow = 2;
        this.arbitraryPosition = new Vector3(arbitraryPosition.x, arbitraryPosition.y, -100);
        gameCamera.orthographicSize = 12;
    }
    /// <summary>
    /// Set the camera clamps.
    /// </summary>
    /// <param name="locationBinds"></param>
    public void SetCameraClamps(LocationBinds locationBinds)
    {
        cameraXClamp = locationBinds.cameraXClamp;
        cameraYClamp = locationBinds.cameraYClamp;
    }
}
