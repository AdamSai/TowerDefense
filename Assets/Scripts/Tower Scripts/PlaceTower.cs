using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlaceTower : MonoBehaviour
{
    public NavMeshSurface surface;
    public LayerMask layermask;
    public LayerMask towerLayer;
    public GameObject previewBox;
    private ObjectPooler objectPooler;
    public bool canPlaceTower = true;
    public UIController uiController;
    private Renderer _previewBoxRenderer;
    private Vector3 _newPos;
    private GameObject selectedObject;
    private float _newX;
    private float _newZ;
    private TargetToUI targetInfoUI;
    Collider[] targets;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(buildNavMesh());
        _previewBoxRenderer = previewBox.GetComponent<Renderer>();
        _newPos = previewBox.transform.position;
        _newX = _newPos.x;
        _newZ = _newPos.z;
        targetInfoUI = GameObject.Find("Game Manager").GetComponent<TargetToUI>();

    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, layermask))
        {

            var newPos = CalculateNewPosition(hit);

            if (objectPooler != null)
            {
                CheckForCollisions();

                previewBox.transform.position = newPos;
                previewBox.SetActive(true);

            }

            if (uiController.ShowingBuildUI)
            {
                targetInfoUI.parent.SetActive(false);

            }

            else
            {
                previewBox.SetActive(false);
            }

            if (Input.GetButtonDown("Fire1"))
            {
                if (canPlaceTower && objectPooler != null && uiController.ShowingBuildUI)
                {
                    targetInfoUI.parent.SetActive(false);
                    selectedObject = null;
                    CreateTower(newPos);
                    StartCoroutine(buildNavMesh());

                }
                else if (hit.transform.tag == "Tower" && !uiController.ShowingBuildUI)
                {
                    selectedObject = hit.transform.gameObject;
                    targetInfoUI.SetSelectedTower(selectedObject);
                    targetInfoUI.parent.SetActive(true);
                }
            }
        }




    }

    private void CheckForCollisions()
    {
        var RaycastPoint = new Vector3(previewBox.transform.position.x, previewBox.transform.position.y + 2, previewBox.transform.position.z);
        if (Physics.SphereCast(RaycastPoint, .5f, Vector3.down, out RaycastHit hit2, 10f, towerLayer, QueryTriggerInteraction.Ignore))
        {
            if (hit2.transform.gameObject.tag == "Tower")
            {
                previewBox.GetComponent<Renderer>().material.color = new Color(140, 0, 0, 0f);
                canPlaceTower = false;
            }
        }
        else
        {
            previewBox.GetComponent<Renderer>().material.color = new Color(0, 140, 0, 0f);
            canPlaceTower = true;
        }
    }

    private Vector3 CalculateNewPosition(RaycastHit hit)
    {
        _newX = Mathf.Round(hit.point.x);
        _newZ = Mathf.Round(hit.point.z);
        if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {                                       //Adding half of the boxes height, to avoid it spawning halfway through the plane
            return new Vector3(_newX, hit.point.y + (previewBox.transform.localScale.y / 2) - 0.01f, _newZ);
        }
        return new Vector3(_newX, hit.transform.position.y - 0.01f, _newZ);

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
        buildNavMesh();
        coll.gameObject.SetActive(false);
    }


    IEnumerator buildNavMesh()
    {
        surface.BuildNavMesh();
        yield return null;
    }

    public void SetObjectPooler(ObjectPooler newPooler)
    {
        objectPooler = newPooler;
    }
    private void OnDrawGizmos()
    {
        if (Debug.isDebugBuild)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawSphere(previewBox.transform.position, .5f);
        }
    }
}
