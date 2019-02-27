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
    private Transform towerMesh;

    private void Start()
    {
        towerMesh = transform.GetChild(0).GetChild(1);
    }
    void FixedUpdate()
    {

        targets = Physics.OverlapSphere(transform.position, range, attackLayer, QueryTriggerInteraction.Ignore);

        if (targets.Length != 0)
            SelectedTarget = SelectTarget(targets);
        else
            SelectedTarget = null;


        if (SelectedTarget != null)
        {
            var lookPos = SelectedTarget.transform.position - transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);

            towerMesh.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 100f);
            
            if (Vector3.Distance(SelectedTarget.transform.position, transform.position) < range && Debug.isDebugBuild)
            {

                Debug.DrawLine(transform.position, SelectedTarget.transform.position, Color.red);
            }
        }
    }


    private Collider SelectTarget(Collider[] targets)
    {
        var CurTarget = targets[0];
        

        for (int i = 0;  i < targets.Length; i++)
        {
            var newTargetPos = Vector3.Distance(targets[i].transform.position, transform.position);
            var selectedTargetPos = Vector3.Distance(CurTarget.transform.position, transform.position);

            if (attackClosestTarget && newTargetPos < selectedTargetPos)
            {
                CurTarget = targets[i];
            }
   
            //else if (selectedTargetPos < range)
            //{
            //    return CurTarget;
            //}
        }

        return CurTarget;
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




