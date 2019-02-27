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
    public Collider SelectedTarget { get; private set; }
    private Collider[] targets;

    void FixedUpdate()
    {

        targets = Physics.OverlapSphere(transform.position, range, attackLayer, QueryTriggerInteraction.Ignore);

        if (targets.Length != 0)
            SelectedTarget = SelectTarget(targets);
        else
            SelectedTarget = null;


        if (SelectedTarget != null)
        {
            if (Vector3.Distance(SelectedTarget.transform.position, transform.position) < range && Debug.isDebugBuild)
            {

                Debug.DrawLine(transform.position, SelectedTarget.transform.position, Color.red);
            }
        }
    }


    private Collider SelectTarget(Collider[] targets)
    {
        if (SelectedTarget == null)
            return targets[0];

        for (int i = 0;  i < targets.Length; i++)
        {
            var newTargetPos = Vector3.Distance(targets[i].transform.position, transform.position);
            var selectedTargetPos = Vector3.Distance(SelectedTarget.transform.position, transform.position);

            if (attackClosestTarget && newTargetPos < selectedTargetPos)
            {
                return targets[i];
            }
            else if (!attackClosestTarget && newTargetPos > selectedTargetPos)
            {
                return targets[i];
            }
            else if (selectedTargetPos < range)
            {
                return SelectedTarget;
            }
        }

        return SelectedTarget;
    }



    private void OnDrawGizmos()
    {
        //if (Debug.isDebugBuild)
        //{
        //    Gizmos.color = new Color(0, 50, 0, .05f);
        //    Gizmos.DrawSphere(transform.position, range);
        //}
    }

}




