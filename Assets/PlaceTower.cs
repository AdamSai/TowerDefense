using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceTower : MonoBehaviour
{
    public GameObject tower;
    public LayerMask layermask;
    private GameObject previewBox;
    private Vector3 _oldPos;
    private float _oldX;
    private float _oldZ;
    // Start is called before the first frame update
    void Start()
    {
        previewBox = Instantiate(tower, Vector3.zero, Quaternion.identity);
        previewBox.SetActive(true);
        _oldPos = previewBox.transform.position;
        _oldX = _oldPos.x;
        _oldZ = _oldPos.z;
        
    }

    // Update is called once per frame
    void Update()
    {

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);


        if (Physics.Raycast(ray, out RaycastHit hit, 1000f, layermask))
        {
            //print(Mathf.Round(hit.point.x) % 2);
            //if(Mathf.Round(hit.point.x) % 2 == 0)
            //{
                _oldX = Mathf.Round(hit.point.x);
            //}
            //if (Mathf.Round(hit.point.z) % 2 == 0)
            //{
                _oldZ = Mathf.Round(hit.point.z);
            //}
            var newPos = new Vector3(_oldX, hit.point.y, _oldZ);
            previewBox.transform.position = newPos;
            if (Input.GetButton("Fire1"))
            {
                var box = Instantiate(tower, newPos, Quaternion.identity);
                box.SetActive(true);
                Debug.DrawLine(Camera.main.transform.position, hit.point);
            }
        }



    }
}
