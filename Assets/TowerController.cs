using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using UnityEditor;
public class TowerController : MonoBehaviour
{
    public float range = 15f;
    public LayerMask attackLayer;
    public bool attackClosestTarget = true;
    private Collider _selectedTarget;
    private readonly Collider[] targets;
    float count;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(Selection.objects.Length != count)
        {
            count = Selection.objects.Length;
            print(Selection.objects.Length);
        }
        _selectedTarget = SelectTarget(targets);

        if (_selectedTarget != null && Debug.isDebugBuild)
            Debug.DrawLine(transform.position, _selectedTarget.transform.position, Color.red);
    }

    private Collider SelectTarget(Collider[] targets)
    {
        targets = Physics.OverlapSphere(transform.position, range, attackLayer, QueryTriggerInteraction.Ignore);

        if (targets.Length != 0)
        {
            if (attackClosestTarget)
                return SelectClosestTarget(targets);
            else
                return SelectFurthestTarget(targets);
        }
        return null;
    }

    private Collider SelectClosestTarget(Collider[] targets)
    {
        for (var i = 0; i < targets.Length; i++)
        {
            if (_selectedTarget == null)
                return targets[0];
            if (i < targets.Length)
            {
                var newTargetPos = Vector3.Distance(targets[i].transform.position, transform.position);
                var curTargetPos = Vector3.Distance(_selectedTarget.transform.position, transform.position);

                if (newTargetPos < curTargetPos)
                {
                    return targets[i];
                }
            }
            else
            {
                i = 0;
            }
        }
        return null;
    }


    private Collider SelectFurthestTarget(Collider[] targets)
    {
        for (var i = 0; i < targets.Length; i++)
        {
            if (_selectedTarget == null)
                return targets[0];
            if (i < targets.Length)
            {
                var newTargetPos = Vector3.Distance(targets[i].transform.position, transform.position);
                var curTargetPos = Vector3.Distance(_selectedTarget.transform.position, transform.position);

                if (newTargetPos > curTargetPos)
                {
                    return targets[i];
                }
            }
            else
            {
                i = 0;
            }
        }
        return null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 50, 0, .05f);
        Gizmos.DrawSphere(transform.position, range);
    }

}




