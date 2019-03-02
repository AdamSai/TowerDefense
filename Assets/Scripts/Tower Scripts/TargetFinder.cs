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
    bool searching = true;

    private void Start()
    {
        searching = true;
        towerMesh = transform.GetChild(0).GetChild(1);
    }
    void FixedUpdate()
    {

        targets = Physics.OverlapSphere(transform.position, range, attackLayer, QueryTriggerInteraction.Ignore);

        if (targets.Length > 0 && searching)
            StartCoroutine(SelectTarget());
        if(targets.Length == 0)
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


    private IEnumerator SelectTarget()
    {
        while (searching)
        {
                SelectedTarget = targets[0];
            for (int i = 0; i < targets.Length; i++)
            {
                var newTargetPos = Vector3.Distance(targets[i].transform.position, transform.position);
                var selectedTargetPos = Vector3.Distance(SelectedTarget.transform.position, transform.position);

                if (attackClosestTarget && newTargetPos < selectedTargetPos)
                {
                    SelectedTarget = targets[i];
                }
                if(i >= targets.Length - 1)
                {
                    searching = false;
                }
            }
        }
        yield return new WaitForSeconds(.5f);
        searching = true;
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




