using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSelector : MonoBehaviour
{
    public ObjectPooler[] TowerPoolers;
    public UIController uiController;
    PlaceTower playerController;
    // Start is called before the first frame update

    private void Start()
    {
        playerController = GameObject.Find("Player Controller").GetComponent<PlaceTower>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && uiController.ShowingBuildUI)
        {
            playerController.SetObjectPooler(TowerPoolers[0]);
        }

        if (!uiController.ShowingBuildUI)
            playerController.SetObjectPooler(null);
    }
}
