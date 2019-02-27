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
    public GameObject parent; //parent to the ui elements
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetSelectedTower(GameObject selectedObject)
    {
        switch (selectedObject.tag)
        {
            case "Tower":
                var towerInfo = selectedObject.GetComponent<TowerAttack>();
                var attackRange = selectedObject.GetComponent<TargetFinder>().range;
                name.text = towerInfo.towerName;
                damage.text = $"Damage: {towerInfo.attackDamage}";
                attackSpeed.text = $"Attack Speed: {((towerInfo.instantAttack)? "instant" : towerInfo.attackCooldown.ToString())}/s";
                range.text = $"Range: {attackRange}";
                break;
            case "Target":

                break;

        }
    }
}
