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

    Collider[] _targets;
    Transform _towerMesh;
    bool _searching = true;
    string towerName;

    private void Start()
    {
        towerName = GetComponent<TowerController>().towerName;
        _searching = true;
        if (towerName.StartsWith("Tower"))
            _towerMesh = transform.GetChild(0).GetChild(1);
    }
    void FixedUpdate()
    {
        _targets = Physics.OverlapSphere(transform.position, range, attackLayer, QueryTriggerInteraction.Ignore);

        if (_targets.Length > 0 && _searching)
            StartCoroutine(SelectTarget());

        if (_targets.Length == 0)
            SelectedTarget = null;

        if (SelectedTarget != null)
        {
            var lookPos = SelectedTarget.transform.position - transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);
            if(towerName.StartsWith("Tower"))
                _towerMesh.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 100f);
            else if (towerName.StartsWith("Laser"))
            {
                transform.rotation = Quaternion.LookRotation(lookPos);// (transform.rotation, rotation, 1);
            }

            if (Vector3.Distance(SelectedTarget.transform.position, transform.position) < range && Debug.isDebugBuild)
            {

                Debug.DrawLine(transform.position, SelectedTarget.transform.position, Color.red);
            }
        }
    }


    private IEnumerator SelectTarget()
    {
        while (_searching)
        {
            SelectedTarget = _targets[0];
            for (int i = 0; i < _targets.Length; i++)
            {
                var newTargetPos = Vector3.Distance(_targets[i].transform.position, transform.position);
                var selectedTargetPos = Vector3.Distance(SelectedTarget.transform.position, transform.position);

                if (attackClosestTarget && newTargetPos < selectedTargetPos)
                {
                    SelectedTarget = _targets[i];
                }
                if (i >= _targets.Length - 1)
                {
                    _searching = false;
                }
            }
        }
        yield return new WaitForSeconds(.1f);
        _searching = true;
    }
}




