using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class TowerController : MonoBehaviour
{
    public float range = 15f;
    public LayerMask attackLayer;
    public bool attackClosestTarget = true;
    public Collider _selectedTarget;
    public Collider[] _targets;

    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 50, 0, .05f);
        Gizmos.DrawSphere(transform.position, range);
    }


}

class ControllerSystem : ComponentSystem
{
    struct Components
    {
        public Transform transform;
        public TowerController towerController;

    }
    

    protected override void OnUpdate()
    {
        foreach (var e in GetEntities<Components>())
        {
           SelectTarget(e.towerController._targets, e);
            if (e.towerController._selectedTarget != null && Debug.isDebugBuild)
                Debug.DrawLine(e.transform.position, e.towerController._selectedTarget.transform.position, Color.red);
        }
    }

    private Collider SelectTarget(Collider[] targets, Components e)
    {
        targets = Physics.OverlapSphere(e.towerController.transform.position, e.towerController.range, e.towerController.attackLayer, QueryTriggerInteraction.Ignore);

        if (targets.Length != 0)
        {
            if (e.towerController.attackClosestTarget)
                e.towerController._selectedTarget = SelectClosestTarget(targets, e);
            else
                e.towerController._selectedTarget = SelectFurthestTarget(targets, e);
        }
        return null;
    }

    private Collider SelectClosestTarget(Collider[] targets, Components e)
    {
        for (var i = 0; i < targets.Length; i++)
        {
            if (e.towerController._selectedTarget == null)
                return targets[0];
            if (i < targets.Length)
            {
                var newTargetPos = Vector3.Distance(targets[i].transform.position, e.towerController.transform.position);
                var curTargetPos = Vector3.Distance(e.towerController._selectedTarget.transform.position, e.towerController.transform.position);

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


    private Collider SelectFurthestTarget(Collider[] targets, Components e)
    {
        for (var i = 0; i < targets.Length; i++)
        {
            if (e.towerController._selectedTarget == null)
                return targets[0];
            if (i < targets.Length)
            {
                var newTargetPos = Vector3.Distance(targets[i].transform.position, e.towerController.transform.position);
                var curTargetPos = Vector3.Distance(e.towerController._selectedTarget.transform.position, e.towerController.transform.position);

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


}


