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
    public Collider selectedTarget { get; private set; }
    private Collider[] targets;
    int i = 0;
    int length = 0;

    void FixedUpdate()
    {

        targets = Physics.OverlapSphere(transform.position, range, attackLayer, QueryTriggerInteraction.Ignore);

        if (targets.Length != 0)
            selectedTarget = SelectTarget(targets);
        else
            selectedTarget = null;


        if (selectedTarget != null)
        {
            if (Vector3.Distance(selectedTarget.transform.position, transform.position) < range && Debug.isDebugBuild)
            {

                Debug.DrawLine(transform.position, selectedTarget.transform.position, Color.red);
            }
        }
    }


    private Collider SelectTarget(Collider[] targets)
    {
        if (selectedTarget == null)
            return targets[0];

        if (i < targets.Length)
        {
            var newTargetPos = Vector3.Distance(targets[i].transform.position, transform.position);
            var selectedTargetPos = Vector3.Distance(selectedTarget.transform.position, transform.position);

            if (attackClosestTarget && newTargetPos < selectedTargetPos)
            {
                i++;
                return targets[i - 1];
            }
            else if (!attackClosestTarget && newTargetPos > selectedTargetPos)
            {
                i++;
                return targets[i - 1];
            }
            else if (selectedTargetPos < range)
            {
                i++;
                return selectedTarget;
            }
        }

        i = 0;
        return selectedTarget;
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




