using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceTower : MonoBehaviour
{
    public GameObject tower;
    public LayerMask layermask;
    public GameObject previewBox;
    public ObjectPooler objectPooler;
    private Renderer _previewBoxRenderer;
    private Vector3 _newPos;
    private float _newX;
    private float _newZ;
    public bool canPlaceTower = true;
    // Start is called before the first frame update
    void Start()
    {
        _previewBoxRenderer = previewBox.GetComponent<Renderer>();
        _newPos = previewBox.transform.position;
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
            previewBox.transform.position = newPos;

            if (canPlaceTower)
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    CreateTower(newPos);
                }
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
            return new Vector3(_newX, hit.point.y + (previewBox.transform.localScale.y / 2), _newZ);
        }
        return new Vector3(_newX, hit.transform.position.y, _newZ);

    }

    private void CreateTower(Vector3 newPos)
    {
        var box = objectPooler.GetPooledObject();
        if (box == null)
            return;
        box.transform.position = newPos;
        box.SetActive(true);
    }

    public void DeleteTower(Collider coll)
    { 
        coll.gameObject.SetActive(false);
    }
}
