using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    public float projectileSpeed = 10f;
    public float projectileHitRange = 1f;
    private Collider followTarget;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (followTarget)
        {

            if ((transform.position - followTarget.transform.position).sqrMagnitude > projectileHitRange)
            {
                transform.position += (followTarget.transform.position - transform.position).normalized * projectileSpeed * Time.deltaTime;
            }
            else
            {
                gameObject.SetActive(false);

            }
        }
    }

    public void SetTarget(Collider target)
    {
        followTarget = target;
    }




}


