using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiringDetector : MonoBehaviour
{
    public bool fireDetect = false;
    public GameObject turret;
   [HideInInspector] public GameObject target;


    private void Awake()
    {

    }



    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Hero")
        {
            target = other.gameObject;
            RaycastHit hit;
            var heading = target.transform.position - turret.transform.position;
            Physics.Raycast(turret.transform.position, heading, out hit);





            if (hit.collider.tag == "Hero")
            {
                target = other.gameObject;
                fireDetect = true;
                Debug.Log("I Am Firing at a " + other.name);
            }
            else
            {
                fireDetect = false;
                target = null;
            }
        }


    }

    private void OnTriggerExit(Collider other)
    {
        if (other == target)
        {
            fireDetect = false;
        }
    }
}
