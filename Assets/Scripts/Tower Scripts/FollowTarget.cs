using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    public float projectileSpeed = 10f;
    public float projectileHitRange = 1f;
    public float damage = 10f;
    Collider _followTarget;

    void Update()
    {
        if (_followTarget)
        {
            if ((transform.position - _followTarget.transform.position).sqrMagnitude > projectileHitRange)
            {
                transform.position += (_followTarget.transform.position - transform.position).normalized * projectileSpeed * Time.deltaTime;
            }
            else
            {
                _followTarget.GetComponent<EnemyController>().health -= damage;
                gameObject.SetActive(false);

            }
        }
    }

    public void SetTarget(Collider target)
    {
        _followTarget = target;
    }

    public void SetDamage(float damage)
    {
        this.damage = damage;
    }



}


