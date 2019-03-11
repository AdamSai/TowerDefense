using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TargetToUI : MonoBehaviour
{
    public new TextMeshProUGUI name;
    public TextMeshProUGUI range;
    public TextMeshProUGUI damage;
    public TextMeshProUGUI attackSpeed;
    public TextMeshProUGUI sellButtonText;
    public TextMeshProUGUI upgradeButtonText;
    public GameObject parent;
    GameObject _selectedObject; //parent to the ui elements
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_selectedObject && !_selectedObject.activeInHierarchy)
        {
            parent.SetActive(false);
        }
        else if(_selectedObject)
        {
            SetSelectedTower(_selectedObject);
        }
    }

    public void SetSelectedTower(GameObject selectedObject)
    {
        _selectedObject = selectedObject;
        switch (selectedObject.tag)
        {
            case "Tower":
                var towerInfo = selectedObject.GetComponent<TowerController>();
                var attackRange = selectedObject.GetComponent<TargetFinder>().range;
                name.text = towerInfo.towerName;
                damage.text = $"Damage: {towerInfo.attackDamage.ToString("F2")}";
                attackSpeed.text = $"Attack Speed: {towerInfo.attackCooldown.ToString("F2")}";
                range.text = $"Range: {attackRange}";
                sellButtonText.text = $"Sell <color=#FF8400>({towerInfo.cost/4}g)</color>";
                sellButtonText.transform.parent.gameObject.SetActive(true);
                upgradeButtonText.transform.parent.gameObject.SetActive(true);
                upgradeButtonText.text = $"Upgrade <color=#FF8400>({towerInfo.cost}g)</color>";
                break;
            case "Target":
                var enemyInfo = selectedObject.GetComponent<EnemyController>();
                name.text = enemyInfo.enemyName;
                damage.text = $"health: {enemyInfo.health.ToString("F2")}/{enemyInfo._maxHealth}";
                attackSpeed.text = "";
                range.text = "";
                sellButtonText.transform.parent.gameObject.SetActive(false);
                upgradeButtonText.transform.parent.gameObject.SetActive(false);

                break;

        }
    }
}
