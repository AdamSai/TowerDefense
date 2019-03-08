using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Transform playerCamera;
    public float panSpeed = 10f;
    public float moveBorder = 10f;
    public float minZ = -18f;
    public float maxZ = 40f;
    public float minX = -10f;
    public float maxX = 14f;
    // Start is called before the first frame update
    void Start()
    {
        playerCamera = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.mousePosition.x <= moveBorder && playerCamera.position.x >= minX)
        {
            playerCamera.position -= new Vector3(panSpeed * Time.deltaTime, 0, 0);
        }

        if (Input.mousePosition.x >= Screen.width - moveBorder && playerCamera.transform.position.x <= maxX)
        {
            playerCamera.position += new Vector3(panSpeed * Time.deltaTime, 0, 0);
        }

        if (Input.mousePosition.y <= moveBorder && playerCamera.transform.position.z > minZ)
        {
            playerCamera.position -= new Vector3(0, 0, panSpeed * Time.deltaTime);
        }
        if (Input.mousePosition.y >= Screen.height - moveBorder && playerCamera.position.z <= maxZ)
        {
            playerCamera.position += new Vector3(0, 0, panSpeed * Time.deltaTime);
        }
    }
}
