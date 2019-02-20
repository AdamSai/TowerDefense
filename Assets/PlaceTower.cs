using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceTower : MonoBehaviour
{
    public GameObject tower;
    public LayerMask layermask;
    public GameObject previewBox;
    private GameObject _previewBox;
    private Vector3 _oldPos;
    private float _oldX;
    private float _oldZ;
    // Start is called before the first frame update
    void Start()
    {
        _previewBox = Instantiate(previewBox, Vector3.zero, Quaternion.identity);
        _previewBox.SetActive(true);
        _oldPos = _previewBox.transform.position;
        _oldX = _oldPos.x;
        _oldZ = _oldPos.z;

    }

    // Update is called once per frame
    void Update()
    {

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);


        if (Physics.Raycast(ray, out RaycastHit hit, 1000f, layermask))
        {
            if (hit.transform.gameObject.layer != 10)
            {
                
                var newPos = CalculateNewPosition(hit);
                _previewBox.transform.position = newPos;
                if (Input.GetButton("Fire1"))
                {
                    CreateTower(hit, newPos);
                }
            }
            if (Input.GetButton("Fire2"))
            {
                DeleteTower(hit);
            }
        }



    }

    private Vector3 CalculateNewPosition(RaycastHit hit)
    {
        _oldX = Mathf.Round(hit.point.x);
        _oldZ = Mathf.Round(hit.point.z);
        return new Vector3(_oldX, hit.point.y, _oldZ);
    }

    private void CreateTower(RaycastHit hit, Vector3 newPos)
    {
        var box = Instantiate(tower, newPos, Quaternion.identity);
        box.SetActive(true);
        Debug.DrawLine(Camera.main.transform.position, hit.point);
    }

    private static void DeleteTower(RaycastHit hit)
    {
        print(hit.transform.gameObject.layer);
        if (hit.transform.gameObject.layer != 9)
            Destroy(hit.transform.gameObject);
    }
}
