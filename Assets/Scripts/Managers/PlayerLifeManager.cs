using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class PlayerLifeManager : MonoBehaviour
{
    public int life = 30;
    public TextMeshProUGUI lifeText;
    public TextMeshProUGUI gameovertext;
    public GameObject button;

    Vector3 textStartScale;
    // Start is called before the first frame update
    void Start()
    {
        lifeText.text = $"Lives: {life.ToString()}";
        Time.timeScale = 1;
        textStartScale = lifeText.transform.localScale;
    }


    public void DecrementLife()
    {
        StartCoroutine(ScaleText());
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

    IEnumerator ScaleText()
    {
        lifeText.color = Color.red;
        yield return new WaitForSeconds(.5f);
        lifeText.color = Color.white;


    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }
}
