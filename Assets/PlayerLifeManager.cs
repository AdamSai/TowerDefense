using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerLifeManager : MonoBehaviour
{
    public int life = 30;
    public TextMeshProUGUI lifeText;
    // Start is called before the first frame update
    void Start()
    {
        lifeText.text = $"Lives: {life.ToString()}";
    }


    public void DecrementLife()
    {
        life--;
        lifeText.text = $"Lives: {life.ToString()}";
    }
}
