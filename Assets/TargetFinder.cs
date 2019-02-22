using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using UnityEditor;
public class TargetFinder : MonoBehaviour
{
    public float range = 15f;
    public LayerMask attackLayer;
    public bool attackClosestTarget = true;
    public Collider _selectedTarget { get; private set; }
    private Collider[] targets;
    int i = 0;
    int length = 0;

    void Update()
    {
        targets = Physics.OverlapSphere(transform.position, range, attackLayer, QueryTriggerInteraction.Ignore);
        length = targets.Length;
        _selectedTarget = SelectTarget(targets);

        if (_selectedTarget != null && Debug.isDebugBuild)
            Debug.DrawLine(transform.position, _selectedTarget.transform.position, Color.red);
    }

    private Collider SelectTarget(Collider[] targets)
    {

        if (targets.Length != 0)
        {
            return SelectClosestTarget(targets);
        }
        return null;
    }

    private Collider SelectClosestTarget(Collider[] targets)
    {
        if (i < targets.Length)
        {
            if (_selectedTarget == null)
                return targets[0];

            var newTargetPos = Vector3.Distance(targets[i].transform.position, transform.position);
            var curTargetPos = Vector3.Distance(_selectedTarget.transform.position, transform.position);

            if (attackClosestTarget && newTargetPos < curTargetPos)
            {
                i++;
                return targets[i - 1];
            }
            else if (!attackClosestTarget && newTargetPos > curTargetPos)
            {
                i++;
                return targets[i - 1];
            }
            else if (curTargetPos < range)
            {
                i++;
                return _selectedTarget;
            }

        }
        i = 0;
        return _selectedTarget;
    }



    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 50, 0, .05f);
        Gizmos.DrawSphere(transform.position, range);
    }

}




