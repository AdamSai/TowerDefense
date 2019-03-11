using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TowerSelector : MonoBehaviour
{
    public ObjectPooler[] TowerPoolers;
    public UIController uiController;
    public new TextMeshProUGUI name;
    public TextMeshProUGUI cost;
    public TextMeshProUGUI range;
    public TextMeshProUGUI damage;
    public TextMeshProUGUI attackSpeed;
    public GameObject infoUI;
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
            SelectObectPooler(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && uiController.ShowingBuildUI)
            SelectObectPooler(1);

        if (!uiController.ShowingBuildUI)
            playerController.SetObjectPooler(null);
    }

    public void SelectObectPooler(int index)
    {
        playerController.SetObjectPooler(TowerPoolers[index]); ;
    }

    public void Button1Enter()
    {
        TowerInfo(0);
        infoUI.SetActive(true);
    }

    public void Button2Enter()
    {
        TowerInfo(1);
        infoUI.SetActive(true);
    }
    public void ButtonExit()
    {
        infoUI.SetActive(false);
    }

    public void TowerInfo(int index)
    {
        var tower = TowerPoolers[index].GetPooledObject();
        var towerInfo = tower.GetComponent<TowerController>();
        var attackRange = tower.GetComponent<TargetFinder>().range;
        name.text = towerInfo.towerName;
        damage.text = $"Damage: {towerInfo.attackDamage.ToString("F2")}";
        if (index == 1)
            damage.text += " (3x to flying units)";
        attackSpeed.text = $"Attack Speed: {towerInfo.attackCooldown.ToString("F2")}";
        range.text = $"Range: {attackRange}";
        cost.text = $"Cost: <color=yellow>{towerInfo.cost}</color>g";
    }

}
