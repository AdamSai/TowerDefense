using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteAlways]
public partial class TowerController : MonoBehaviour
{
    public string towerName = "Tower";
    public float attackDamage = 10f;
    [Tooltip("If this is selected, the attack damage will arrive instantly at the selected target")] public bool instantAttack = false;
    public float attackCooldown = 1f;
    public ObjectPooler objectPooler;
    public int cost = 10;
    public bool isSelected = false;
    int i = 1;
    bool _canAttack = true;
    Collider _selectedTarget;
    string _startName;
    float _startdDamage;
    float _startCooldown;
    int _startCost;

    private void Awake()
    {
        _startName = towerName;
        _startdDamage = attackDamage;
        _startCooldown = attackCooldown;
        _startCost = cost;
    }

    // Update is called once per frame
    void Update()
    {
        if (_canAttack)
        {
            Attack();
        }
        if(isSelected)
        {
            transform.GetChild(1).gameObject.SetActive(true);
        } else
        {
            transform.GetChild(1).gameObject.SetActive(false);

        }
    }


    void Attack()
    {
        _canAttack = false;
        _selectedTarget = GetComponent<TargetFinder>().SelectedTarget;
        StartCoroutine(SetCanAttack());

        if (!instantAttack && _selectedTarget != null)
        {
            var projectile = objectPooler.GetPooledObject(); //  Instantiate(projectile, transform.position, Quaternion.identity);
            if (projectile == null)
            {
                return;
            }
            if (_selectedTarget.gameObject.activeInHierarchy)
            {
                var followScript = projectile.GetComponent<FollowTarget>();
                followScript.SetDamage(attackDamage);
                projectile.transform.position = transform.position;
                followScript.SetTarget(_selectedTarget);
                projectile.SetActive(true);
            }
        }

    }

    public void UpgradeTower()
    {
        i++;
        attackDamage += (attackDamage *  1.5f);
        if(attackCooldown > 0.1f)
        {
            attackCooldown -= 0.1f;
        }
        cost += Mathf.RoundToInt(cost * 1.5f);
        towerName = "Tower " + ToRoman(i);

    }

    IEnumerator SetCanAttack()
    {
        yield return new WaitForSeconds(attackCooldown);
        _canAttack = true;
    }

    private void OnEnable()
    {

    }

    private void OnDisable()
    {
        _canAttack = true;
        cost = _startCost;
        attackDamage = _startdDamage;
        attackCooldown = _startCooldown;
        towerName = _startName;
        i = 1;
    }

    public static string ToRoman(int number)
    {
        if (number < 1)
            return string.Empty;
        if (number >= 1000)
            return "M" + ToRoman(number - 1000);
        if (number >= 900)
            return "CM" + ToRoman(number - 900); //EDIT: i've typed 400 instead 900
        if (number >= 500)
            return "D" + ToRoman(number - 500);
        if (number >= 400)
            return "CD" + ToRoman(number - 400);
        if (number >= 100)
            return "C" + ToRoman(number - 100);
        if (number >= 90)
            return "XC" + ToRoman(number - 90);
        if (number >= 50)
            return "L" + ToRoman(number - 50);
        if (number >= 40)
            return "XL" + ToRoman(number - 40);
        if (number >= 10)
            return "X" + ToRoman(number - 10);
        if (number >= 9)
            return "IX" + ToRoman(number - 9);
        if (number >= 5)
            return "V" + ToRoman(number - 5);
        if (number >= 4)
            return "IV" + ToRoman(number - 4);
        if (number >= 1)
            return "I" + ToRoman(number - 1);
        else
            return "";
    }
}
