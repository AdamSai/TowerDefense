using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pregame : MonoBehaviour
{
    bool isPaused = true;
    // Start is called before the first frame update
    void Awake()
    {
        isPaused = true;
    }

    private void Update()
    {
        if (isPaused)
            Time.timeScale = 0;
    }


    public void Startgame()
    {
        gameObject.SetActive(false);
        isPaused = false;
        Time.timeScale = 1;
    }
}
