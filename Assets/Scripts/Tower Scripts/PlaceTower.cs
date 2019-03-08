using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlaceTower : MonoBehaviour
{
    public NavMeshSurface surface;
    public LayerMask raycastLayer; //Layers that the initial raycast will hit
    public LayerMask blockingLayer; //Layers that blocks placement of towers
    public GameObject previewBox;
    private ObjectPooler objectPooler;
    public bool canPlaceTower = true;
    public UIController uiController;
    public float doubleClickTimer = 0.5f;


    GoldManager _gold;
    Renderer _previewBoxRenderer;
    Vector3 _newPos;
    GameObject selectedObject;
    float _newX;
    float _newZ;
    TargetToUI _targetInfoUI;
    GameObject _gameManager;
    float _doubleClickTracker;
    int _clickCounter = 1;
    bool _trackClickTimer = false;
    GameObject _towerContainer;
    List<TowerController> _selectedTowers;


    // Start is called before the first frame update
    void Start()
    {
        _selectedTowers = new List<TowerController>();
        _gameManager = GameObject.Find("Game Manager");
        _towerContainer = GameObject.Find("Towers");
        _gold = _gameManager.GetComponent<GoldManager>();
        _targetInfoUI = _gameManager.GetComponent<TargetToUI>();
        _previewBoxRenderer = previewBox.GetComponent<Renderer>();
        _newPos = previewBox.transform.position;
        _newX = _newPos.x;
        _newZ = _newPos.z;
        StartCoroutine(BuildNavMesh());
    }

    // Update is called once per frame
    void Update()
    {
        if (_doubleClickTracker >= doubleClickTimer)
        {
            _clickCounter = 0;
            _doubleClickTracker = 0;
            _trackClickTimer = false;
        }
        if (_trackClickTimer)
        {
            _doubleClickTracker += Time.deltaTime;
        }
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, raycastLayer))
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
                _targetInfoUI.parent.SetActive(false);
                if (selectedObject && selectedObject.CompareTag("Tower"))
                {
                    selectedObject.GetComponent<TowerController>().isSelected = false;
                    SelectOrDeselectAllTowerByName(false, selectedObject.GetComponent<TowerController>());
                    DeselectAllTowers();
                }
            }

            else
                previewBox.SetActive(false);


            if (Input.GetButtonDown("Fire1"))
            {

                if (selectedObject)
                {
                    if (selectedObject.CompareTag("Tower") && selectedObject != hit.transform.gameObject && hit.transform.CompareTag("Tower"))
                    {
                        SelectOrDeselectAllTowerByName(false, selectedObject.GetComponent<TowerController>());

                        selectedObject.GetComponent<TowerController>().isSelected = false;

                    }
                    if (hit.transform.CompareTag("Target"))
                    {
                        selectedObject.GetComponent<TowerController>().isSelected = false;
                        //SelectOrDeselectAllTowerByName(false, selectedObject.GetComponent<TowerController>());

                    }

                }


                //Create tower
                if (canPlaceTower && objectPooler != null && uiController.ShowingBuildUI)
                {
                    selectedObject = null;
                    CreateTower(newPos);
                }
                //Select target to display in UI
                else if (!uiController.ShowingBuildUI && (hit.transform.tag == "Tower" || hit.transform.tag == "Target"))
                {
                    _trackClickTimer = true;
                    _clickCounter++;
                    if (_clickCounter % 2 == 0)
                    {
                        SelectOrDeselectAllTowerByName(true, selectedObject.GetComponent<TowerController>());
                    }
                    else
                    {
                        DeselectAllTowers();

                        selectedObject = hit.transform.gameObject;
                        if (selectedObject.CompareTag("Tower"))
                        {
                            selectedObject.GetComponent<TowerController>().isSelected = true;
                            _selectedTowers.Add(selectedObject.GetComponent<TowerController>());
                        }
                        _targetInfoUI.SetSelectedTower(selectedObject);
                        _targetInfoUI.parent.SetActive(true);
                    }
                }
            }
        }
    }

    private void DeselectAllTowers()
    {
        foreach (TowerController tower in _selectedTowers)
        {
            tower.isSelected = false;
        }
        _selectedTowers = new List<TowerController>();
    }

    private void SelectOrDeselectAllTowerByName(bool isSelected, TowerController selectedTower)
    {
        DeselectAllTowers();
        TowerController[] allTowers = _towerContainer.GetComponentsInChildren<TowerController>(false);
        for (int i = 0; i < allTowers.Length; i++)
        {
            if (allTowers[i] != selectedObject.GetComponent<TowerController>() && allTowers[i].towerName == selectedTower.towerName)
            {
                _selectedTowers.Add(allTowers[i]);
            }
        }
        //The selected object is added in the last index, so it will always be the first element to be upgraded
        //when we loop backwards through the list.
        //We loop backwards in case we need to remove an element from the list while iterating in other parts of the code
        _selectedTowers.Add(selectedObject.GetComponent<TowerController>());
        foreach (TowerController tower in _selectedTowers)
        {
            if (tower.towerName == selectedTower.towerName)
                tower.isSelected = isSelected;
        }
    }

    private void CheckForCollisions()
    {
        var RaycastPoint = new Vector3(previewBox.transform.position.x, previewBox.transform.position.y + 2, previewBox.transform.position.z);
        if (Physics.SphereCast(RaycastPoint, .5f, Vector3.down, out RaycastHit hit2, 10f, raycastLayer, QueryTriggerInteraction.Ignore))
        {
            var hitTag = hit2.transform.gameObject.tag;
            if (hitTag == "Tower" || hitTag == "Restricted" || hitTag == "Target")
            {
                previewBox.GetComponent<Renderer>().material.color = new Color(140, 0, 0, .5f);
                canPlaceTower = false;
            }
            else
            {
                previewBox.GetComponent<Renderer>().material.color = new Color(0, 140, 0, .5f);
                canPlaceTower = true;
            }
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
        var cost = box.GetComponent<TowerController>().cost;
        if (_gold.Gold >= cost)
        {
            if (box == null)
                return;
            StartCoroutine(BuildNavMesh());
            _gold.RemoveGold(cost);
            box.transform.position = newPos;
            box.SetActive(true);
        }
        else
        {
            StartCoroutine(_gold.DisplayErrorText());
        }
    }

    public void SellTower()
    {
        BuildNavMesh();
        foreach (TowerController tower in _selectedTowers)
        {
            _gold.AddGold(tower.cost / 4);
            tower.gameObject.SetActive(false);
            _targetInfoUI.parent.SetActive(false);
        }
        DeselectAllTowers();

    }

    public void UpgradeTower()
    {
        TowerController upgradedTower = null;
        for (int i = _selectedTowers.Count - 1; i >= 0; i--)
        {
            //If no towers can be upgraded, break so the towers aren't deselected
            if (i == _selectedTowers.Count - 1 && _gold.Gold < _selectedTowers[i].cost)
            {
                StartCoroutine(_gold.DisplayErrorText());
                break;
            }
            if (_gold.Gold >= _selectedTowers[i].cost)
            {
                _gold.RemoveGold(_selectedTowers[i].cost);
                _selectedTowers[i].UpgradeTower();
                upgradedTower = _selectedTowers[i];
            }
            else if (_gold.Gold < _selectedTowers[i].cost)
            {
                StartCoroutine(_gold.DisplayErrorText());
                break;
            }
        }
        //Deselected towers which have not been upgraded
        if (upgradedTower)
        {
            for (int i = _selectedTowers.Count - 1; i >= 0; i--)
            {
                if (_selectedTowers[i].towerName != upgradedTower.towerName)
                {
                    _selectedTowers[i].isSelected = false;
                    _selectedTowers.Remove(_selectedTowers[i]);
                }
            }
        }
    }

    IEnumerator BuildNavMesh()
    {
        surface.RemoveData();
        surface.BuildNavMesh();
        yield return null;
    }

    public void SetObjectPooler(ObjectPooler newPooler)
    {
        objectPooler = newPooler;
    }
}
