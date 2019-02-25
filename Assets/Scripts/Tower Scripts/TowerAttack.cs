using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteAlways]
public partial class TowerAttack : MonoBehaviour
{
    public float attackDamage = 10f;
    [Tooltip("If this is selected, the attack damage will arrive instantly at the selected target")] public bool instantAttack = false;
    public float attackCooldown = 1f;

    public ObjectPooler objectPooler;
    private bool canAttack = true;

    private Collider selectedTarget;

    // Update is called once per frame
    void Update()
    {
        if (canAttack)
        {
            Attack();
        }
    }


    void Attack()
    {
        canAttack = false;
        selectedTarget = GetComponent<TargetFinder>().SelectedTarget;
        StartCoroutine(SetCanAttack());

        if (!instantAttack && selectedTarget != null)
        {
            var projectile = objectPooler.GetPooledObject(); //  Instantiate(projectile, transform.position, Quaternion.identity);
            if (projectile == null)
            {
                return; 
            }

            var followScript = projectile.GetComponent<FollowTarget>();
            projectile.transform.position = transform.position;
            followScript.SetTarget(selectedTarget);
            projectile.SetActive(true);
        }

    }

    IEnumerator SetCanAttack()
    {
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    private void OnEnable()
    {
        canAttack = true;
    }
}
