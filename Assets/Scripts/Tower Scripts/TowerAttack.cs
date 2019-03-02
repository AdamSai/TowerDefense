using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteAlways]
public partial class TowerAttack : MonoBehaviour
{
    public string towerName = "Tower";
    public float attackDamage = 10f;
    [Tooltip("If this is selected, the attack damage will arrive instantly at the selected target")] public bool instantAttack = false;
    public float attackCooldown = 1f;
    public ObjectPooler objectPooler;

    bool _canAttack = true;
    Collider _selectedTarget;

    // Update is called once per frame
    void Update()
    {
        if (_canAttack)
        {
            Attack();
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

    IEnumerator SetCanAttack()
    {
        yield return new WaitForSeconds(attackCooldown);
        _canAttack = true;
    }

    private void OnEnable()
    {
        _canAttack = true;
    }
}
