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
        if (playerCamera.position.x >= minX)
        {
            if (Input.mousePosition.x <= moveBorder || Input.GetKey(KeyCode.A))
                playerCamera.position -= new Vector3(panSpeed * Time.deltaTime, 0, 0);
        }

        if (playerCamera.transform.position.x <= maxX)
        {
            if(Input.mousePosition.x >= Screen.width - moveBorder || Input.GetKey(KeyCode.D))
                playerCamera.position += new Vector3(panSpeed * Time.deltaTime, 0, 0);
        }

        if (playerCamera.transform.position.z > minZ)
        {
            if(Input.mousePosition.y <= moveBorder || Input.GetKey(KeyCode.S))
                playerCamera.position -= new Vector3(0, 0, panSpeed * Time.deltaTime);
        }
        if (playerCamera.position.z <= maxZ)
        {
            if(Input.mousePosition.y >= Screen.height - moveBorder || Input.GetKey(KeyCode.W))
                playerCamera.position += new Vector3(0, 0, panSpeed * Time.deltaTime);
        }
    }
}
