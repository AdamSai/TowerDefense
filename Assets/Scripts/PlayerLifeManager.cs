using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerLifeManager : MonoBehaviour
{
    public int life = 30;
    public TextMeshProUGUI lifeText;
    public TextMeshProUGUI gameovertext;
    public GameObject button;
    // Start is called before the first frame update
    void Start()
    {
        lifeText.text = $"Lives: {life.ToString()}";
        Time.timeScale = 1;
    }


    public void DecrementLife()
    {
        life--;
        lifeText.text = $"Lives: {life.ToString()}";
        if(life <= 0)
        {
            button.SetActive(true);
            gameovertext.gameObject.SetActive(true);
            gameovertext.text = "Game over!";
            Time.timeScale = 0;
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }
}
