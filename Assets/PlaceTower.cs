using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceTower : MonoBehaviour
{
    public GameObject tower;
    public LayerMask layermask;
    public GameObject previewBox;
    private GameObject _previewBox;
    private Renderer _previewBoxRenderer;
    private Vector3 _newPos;
    private float _newX;
    private float _newZ;
    // Start is called before the first frame update
    void Start()
    {
        _previewBox = Instantiate(previewBox, Vector3.zero, Quaternion.identity);
        _previewBox.SetActive(true);
        _previewBoxRenderer = _previewBox.GetComponent<Renderer>();
        _newPos = _previewBox.transform.position;
        _newX = _newPos.x;
        _newZ = _newPos.z;

    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, layermask))
        {
            var newPos = CalculateNewPosition(hit);
            _previewBox.transform.position = newPos;

            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                _previewBoxRenderer.material.color = Color.green;
                if (Input.GetButton("Fire1"))
                {
                    CreateTower(hit, newPos);
                }
            }
            else
                _previewBoxRenderer.material.color = Color.red;
            if (Input.GetButton("Fire2"))
            {
                DeleteTower(hit);
            }
        }
        Debug.DrawLine(Camera.main.transform.position, hit.point);




    }

    private Vector3 CalculateNewPosition(RaycastHit hit)
    {
        _newX = Mathf.Round(hit.point.x);
        _newZ = Mathf.Round(hit.point.z);
        if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {                                       //Adding half of the boxes height, to avoid it spawning halfway through the plane
            return new Vector3(_newX, hit.point.y + (_previewBox.transform.localScale.y / 2), _newZ);
        }
        return new Vector3(_newX, hit.transform.position.y, _newZ);

    }

    private void CreateTower(RaycastHit hit, Vector3 newPos)
    {
        var box = Instantiate(tower, newPos, Quaternion.identity);
        box.SetActive(true);
    }

    private static void DeleteTower(RaycastHit hit)
    {
        print(hit.transform.gameObject.layer);
        if (hit.transform.gameObject.layer != LayerMask.NameToLayer("Ground"))
            Destroy(hit.transform.gameObject);
    }
}
