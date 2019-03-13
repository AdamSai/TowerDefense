using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

//This class is used to:
//Set the position of the tower previewbox.
//See if it is possible to place a tower at the mouse position
//Create towers at the mouse positioon
//Delete towers
//Select / Deselect Towers
//Enable / disable Target UI & Building UI
public class PlaceTower : MonoBehaviour
{
    public NavMeshSurface surface;
    public LayerMask raycastLayer; //Layers that the initial raycast will hit
    public GameObject previewBox;
    private ObjectPooler objectPooler;
    public bool canPlaceTower = true;
    public UIController uiController;
    public float doubleClickTimer = 0.5f;


    GoldManager _goldManager;
    Renderer _previewBoxRenderer;
    Vector3 _newPos;
    GameObject selectedObject;
    float _newX;
    float _newZ;
    TargetToUI _targetInfoUI;
    GameObject _gameManager;

    float _doubleClickTimeTracker;
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
        _goldManager = _gameManager.GetComponent<GoldManager>();
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
        
        if (Time.timeScale == 0)
            return;

        if (_doubleClickTimeTracker >= doubleClickTimer)
        {
            _clickCounter = 1;
            _doubleClickTimeTracker = 0;
            _trackClickTimer = false;
        }
        if (_trackClickTimer)
        {
            _doubleClickTimeTracker += Time.deltaTime;
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
            //If the build menu is showing
            if (uiController.ShowingBuildUI)
            {
                _targetInfoUI.parent.SetActive(false);
                //Remove the green circle from towers if the build menu is open & deselect them
                if (selectedObject && selectedObject.CompareTag("Tower"))
                {
                    selectedObject.GetComponent<TowerController>().isSelected = false;
                    SelectOrDeselectTowersByName(false, selectedObject.GetComponent<TowerController>());
                    DeselectAllTowers();
                }
            }
            else
                previewBox.SetActive(false);
            //If the cursor is over a button, we return to not place a tower behind the button
            if (EventSystem.current.IsPointerOverGameObject() && EventSystem.current.currentSelectedGameObject != null && EventSystem.current.currentSelectedGameObject.CompareTag("UI"))
            {
                return;
            }

            if (Input.GetButtonDown("Fire1"))
            {
                if (hit.transform.gameObject.tag != "Target" && hit.transform.gameObject.tag != "Tower")
                {
                    DeselectAllTowers();
                    _targetInfoUI.parent.SetActive(false);
                }
                if (selectedObject)
                {
                    //Deselect current tower if we click on a new tower
                    if (selectedObject.CompareTag("Tower") && selectedObject != hit.transform.gameObject && hit.transform.CompareTag("Tower"))
                    {
                        SelectOrDeselectTowersByName(false, selectedObject.GetComponent<TowerController>());
                        selectedObject.GetComponent<TowerController>().isSelected = false;

                    }
                    if (hit.transform.CompareTag("Target"))
                    {
                        DeselectAllTowers();
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
                    //If we double click the same tower, increase the counter
                    if (selectedObject == hit.transform.gameObject)
                    {
                        _trackClickTimer = true;
                        if (_clickCounter == 1)
                            _clickCounter++;
                        else
                            _clickCounter = 1;
                    }
                    //If we are clicking a new tower, reset the double click timer
                    else
                    {
                        _trackClickTimer = true;
                        _clickCounter = 1;
                        _doubleClickTimeTracker = 0;
                    }
                    //On double click, select all towers with the same name
                    if (_clickCounter % 2 == 0 && _selectedTowers.Count > 0)
                    {
                        SelectOrDeselectTowersByName(true, selectedObject.GetComponent<TowerController>());
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
                        _targetInfoUI.SetSelectedObject(selectedObject);
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

    private void SelectOrDeselectTowersByName(bool isSelected, TowerController selectedTower)
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
        //We loop backwards trhoughs the list in other parts of the code to avoid index out of range exception
        _selectedTowers.Add(selectedObject.GetComponent<TowerController>());

        foreach (TowerController tower in _selectedTowers)
        {
            if (tower.towerName == selectedTower.towerName)
                tower.isSelected = isSelected;
        }
    }

    private void CheckForCollisions()
    {
        var RaycastPoint = new Vector3(previewBox.transform.position.x, previewBox.transform.position.y + 100, previewBox.transform.position.z);
        if (Physics.SphereCast(RaycastPoint, .5f, Vector3.down, out RaycastHit hit2, 200f, raycastLayer, QueryTriggerInteraction.Ignore))
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
        var tower = objectPooler.GetPooledObject();
        var cost = tower.GetComponent<TowerController>().cost;
        if (_goldManager.Gold >= cost)
        {
            if (tower == null)
                return;
            _goldManager.RemoveGold(cost);
            tower.transform.position = newPos;
            tower.SetActive(true);
            StartCoroutine(BuildNavMesh());
        }
        else
        {
            StartCoroutine(_goldManager.DisplayErrorText());
        }
    }

    public void SellTower()
    {
        foreach (TowerController tower in _selectedTowers)
        {
            _goldManager.AddGold(tower.cost / 4);
            tower.gameObject.SetActive(false);
            _targetInfoUI.parent.SetActive(false);
        }
        DeselectAllTowers();
        StartCoroutine(BuildNavMesh());
    }

    public void UpgradeTower()
    {
        TowerController upgradedTower = null;
        for (int i = _selectedTowers.Count - 1; i >= 0; i--)
        {
            //If no towers can be upgraded, break so the towers aren't deselected
            if (i == _selectedTowers.Count - 1 && _goldManager.Gold < _selectedTowers[i].cost)
            {
                StartCoroutine(_goldManager.DisplayErrorText());
                break;
            }
            else if (_goldManager.Gold >= _selectedTowers[i].cost)
            {
                _goldManager.RemoveGold(_selectedTowers[i].cost);
                _selectedTowers[i].UpgradeTower();
                upgradedTower = _selectedTowers[i];
            }
            else if (_goldManager.Gold < _selectedTowers[i].cost)
            {
                StartCoroutine(_goldManager.DisplayErrorText());
                break;
            }
        }
        //Deselect towers which have not been upgraded
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
        yield return new WaitForEndOfFrame();
        surface.BuildNavMesh();
    }

    public void SetObjectPooler(ObjectPooler newPooler)
    {
        objectPooler = newPooler;
    }
}
