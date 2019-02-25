using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    public List<Node> FinalPath;
    public float movementSpeed;
    public int i;
    bool shouldMove = false;
    // Start is called before the first frame update
    void Start()
    {
        FinalPath = new List<Node>();
        shouldMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (i == FinalPath.Count && i > 0)
        {
            shouldMove = false;
        }
        if (FinalPath.Count > 0 && shouldMove)
        {
            shouldMove = true;
            var newPos = new Vector3(FinalPath[i].Position.x, transform.position.y, FinalPath[i].Position.z);

            //var x = transform.position.x - FinalPath[i].gridX;
            //var y = transform.position.y - FinalPath[i].gridY;

            // print(Vector3.Distance(transform.position, newPos));

            if (Vector3.Distance(transform.position, newPos) > 0.2f)
            {
                transform.position += (newPos - transform.position).normalized * movementSpeed * Time.deltaTime;
                //StartCoroutine(MoveEnemy(newPos));
            }
            else
            {
                i++;
            }

        }
    }


    private IEnumerator MoveEnemy(Vector3 newPos)
    {
        while ((transform.position - newPos).sqrMagnitude > 0.1f)
        {
            transform.position += (transform.position - newPos).normalized * 10f * Time.deltaTime;
            yield return null;

        }

    }
}
