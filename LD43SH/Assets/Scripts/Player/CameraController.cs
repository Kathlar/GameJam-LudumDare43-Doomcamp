using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 40.0F;
    public float moveFollowSpeed = 20.0F;
    public float moveMouseSensitivity = 0.2F;
    public float rotationSpeed = 10.0F;
    public float rotationFollowSpeed = 20.0F;
    public bool moveWithMouse = true;
    public float maxDistance = 100.0F;
    // public int screenBound = 50.0F;
    public float zoomSpeed = 1.0F;
    public float zoomFollowSpeed = 1.0F;
    public Transform zoomedTransform;
    public Transform cameraDolly;

    protected Transform unzoomedTransform;
    //protected int screenWidth;
    //protected int screenHeight;
    protected float zoomValue = 1.0F;
    protected float rotationValue = 0.0F;
    protected Vector3 lastMousePosition;
    protected Vector3 mousePositionDelta;

    Vector3 guardLerpStartPos;
    Vector3 desiredGuardPos;
    float guardLerpProgress = 1;

    void Start()
    {
        GameObject unzoomedTransformHelper = new GameObject("UnzoomedTransformHelper");

        unzoomedTransform = unzoomedTransformHelper.transform;

        unzoomedTransform.transform.position = cameraDolly.transform.position;
        unzoomedTransform.transform.rotation = cameraDolly.transform.rotation;
        unzoomedTransform.transform.parent = cameraDolly.transform.parent;

        lastMousePosition = Input.mousePosition;
    }

    private void Update()
    {
        if (guardLerpProgress < 1)
        {
            guardLerpProgress = Mathf.Clamp(guardLerpProgress + Time.deltaTime, 0, 1);

            float lerper = 
                3 * guardLerpProgress * guardLerpProgress -
                2 * guardLerpProgress * guardLerpProgress * guardLerpProgress;

            transform.position = Vector3.Lerp(guardLerpStartPos, desiredGuardPos, lerper);
            return;
        }
        mousePositionDelta = lastMousePosition - Input.mousePosition;

        Vector3 newPosition = transform.position;

        if (Input.GetMouseButton(1))
        {
            newPosition = DragWithMouse(newPosition);
        }
        else if (Input.GetMouseButton(2))
        {
            RotateWithMouse();
        }
        else
        {
            newPosition = DragWithKeys(newPosition);
        }

        if (newPosition.sqrMagnitude > maxDistance * maxDistance)
        {
            newPosition = newPosition.normalized * maxDistance;
        }

        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * moveFollowSpeed);

        UpdateZoom();

        lastMousePosition = Input.mousePosition;
    }

    protected Vector3 DragWithMouse(Vector3 newPosition)
    {
        Vector3 delta = new Vector3();

        delta.x = mousePositionDelta.x * moveSpeed * moveMouseSensitivity * Time.deltaTime;
        delta.z = mousePositionDelta.y * moveSpeed * moveMouseSensitivity * Time.deltaTime;

        delta = transform.localToWorldMatrix * delta;

        newPosition += delta;

        return newPosition;
    }

    protected Vector3 DragWithKeys(Vector3 newPosition)
    {
        Vector3 delta = new Vector3();

        delta.x = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        delta.z = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;

        delta = transform.localToWorldMatrix * delta;

        newPosition += delta;

        return newPosition;
    }

    protected void RotateWithMouse()
    {
        rotationValue += mousePositionDelta.x * Time.deltaTime * rotationSpeed;

        Vector3 orientation = transform.eulerAngles;

        orientation.y = Mathf.LerpAngle(orientation.y, rotationValue, Time.deltaTime * rotationFollowSpeed);

        transform.eulerAngles = orientation;
    }

    protected void UpdateZoom()
    {
        Vector3 zoomPosition = cameraDolly.transform.localPosition;
        Quaternion zoomRotation = cameraDolly.transform.rotation;

        zoomValue = Mathf.Clamp01(zoomValue + Input.GetAxis("Mouse ScrollWheel") * -zoomSpeed * Time.deltaTime);
        zoomPosition = Vector3.Lerp(zoomedTransform.position, unzoomedTransform.position, zoomValue);
        zoomRotation = Quaternion.Slerp(zoomedTransform.rotation, unzoomedTransform.rotation, zoomValue);

        cameraDolly.transform.position = Vector3.Lerp(cameraDolly.transform.position, zoomPosition, zoomFollowSpeed * Time.deltaTime);
        cameraDolly.transform.rotation = Quaternion.Slerp(cameraDolly.transform.rotation, zoomRotation, zoomFollowSpeed * Time.deltaTime);
    }

    public void StartGuardLerp(Vector3 position)
    {
        guardLerpStartPos = transform.position;
        desiredGuardPos = position;
        guardLerpProgress = 0;
    }
}
