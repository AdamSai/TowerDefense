﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GoldManager : MonoBehaviour
{
    public int Gold { get; private set; } = 100;
    public TextMeshProUGUI GoldText;
    public TextMeshProUGUI ErrorText;

    private void Start()
    {
        GoldText.text = $"Gold: {((Gold < 10) ? 0 + Gold.ToString() : Gold.ToString())}";
        ErrorText.text = "";
    }

    public void AddGold(int amount)
    {
        Gold += amount;
        GoldText.text = $"Gold: {Gold}";
    }

    public void RemoveGold(int amount)
    {
        Gold -= amount;
        GoldText.text = $"Gold: {Gold}";
    }

    public IEnumerator DisplayErrorText()
    {
        if (ErrorText.text == "")
        {
            ErrorText.text = "Not enough gold";
            yield return new WaitForSeconds(2f);
            ErrorText.text = "";
        }
    }

}
