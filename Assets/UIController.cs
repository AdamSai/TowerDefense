using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public GameObject BuildUI;
    private bool ShowingBuildUI = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            if (ShowingBuildUI)
            {
                ShowingBuildUI = false;
                BuildUI.SetActive(false);
            }else
            {
                ShowingBuildUI = true;
                BuildUI.SetActive(true);
            }
        }
    }
}
