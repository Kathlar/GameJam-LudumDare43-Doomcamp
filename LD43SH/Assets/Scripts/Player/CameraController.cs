using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 20;

    public float minimalX = -50, maximalX = 50;
    public float minimalZ = -50, maximalZ = 50;

    public bool moveWithMouse = true;
    private int screenWidth, screenHeight;
    public int screenBound = 50;

    private float currentOffset;
    public float minimalOffset, maximalOffset;

    void Start()
    {
        currentOffset = transform.position.y;
        minimalOffset = currentOffset * .7f;
        maximalOffset = currentOffset * 1.6f;
    }

    void Update()
    {
        //Move with keys
        Vector3 newPosition = transform.position;
        float xMove = Input.GetAxis("Horizontal");
        float zMove = Input.GetAxis("Vertical");
        newPosition.x = Mathf.Clamp(newPosition.x + xMove * moveSpeed * Time.deltaTime, minimalX, maximalX);
        newPosition.z = Mathf.Clamp(newPosition.z + zMove * moveSpeed * Time.deltaTime, minimalZ, maximalZ);

        //Move with mouse
        if (moveWithMouse)
        {
            screenWidth = Screen.width;
            screenHeight = Screen.height;

            if (Input.mousePosition.x > screenWidth - screenBound)
            {
                newPosition.x += moveSpeed * Time.deltaTime;
            }

            if (Input.mousePosition.x < screenBound)
            {
                newPosition.x -= moveSpeed * Time.deltaTime;
            }

            if (Input.mousePosition.y > screenHeight - screenBound)
            {
                newPosition.z += moveSpeed * Time.deltaTime;
            }

            if (Input.mousePosition.y < screenBound)
            {
                newPosition.z -= moveSpeed * Time.deltaTime;
            }
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            newPosition.y = Mathf.Clamp(transform.position.y - 2 * moveSpeed * moveSpeed * Time.deltaTime * Time.deltaTime, minimalOffset,
                maximalOffset);
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            newPosition.y = Mathf.Clamp(transform.position.y + 2 * moveSpeed * moveSpeed * Time.deltaTime * Time.deltaTime, minimalOffset,
                maximalOffset);
        }

        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * moveSpeed);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.gray;
        Gizmos.DrawWireCube(new Vector3(minimalX + maximalX, transform.position.y, minimalZ + maximalZ), new Vector3(-minimalX + maximalX, .1f, -minimalZ + maximalZ));

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, .5f);
    }
}
