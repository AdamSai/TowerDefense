using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewTower : MonoBehaviour
{
    private Material material;
    private PlaceTower playerController;
    private List<Collider> colliders;
    // Start is called before the first frame update
    void Start()
    {
        colliders = new List<Collider>();
        material = GetComponent<Renderer>().material;
        playerController = GameObject.Find("Player Controller").GetComponent<PlaceTower>();
    }

    // Update is called once per frame
    void Update()
    {
        if(colliders.Count != 0)
        {
            playerController.canPlaceTower = false;
            material.color = Color.red;

        }else
        {
            material.color = Color.green;
            playerController.canPlaceTower = true;

        }

    }

    private void OnTriggerEnter(Collider other)
    {
    }
    private void OnTriggerStay(Collider other)
    {
                               //TODO: replace this with a method which raycasts & selects the tower, which can then be sold
        if (Input.GetButtonDown("Fire2") && colliders.Contains(other))
        {
            colliders.Remove(other);
            playerController.DeleteTower(other);
        }

        else if (other.gameObject.tag == "Tower" && (other.transform.position - transform.position).sqrMagnitude <= 3 && !colliders.Contains(other))
        {
            colliders.Add(other);
        } else if(other.gameObject.tag == "Tower" && (other.transform.position - transform.position).sqrMagnitude > 3 && colliders.Contains(other))
        {
            
            colliders.Remove(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
     
        
            colliders.Remove(other);

    }



}
