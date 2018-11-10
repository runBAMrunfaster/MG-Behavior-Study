using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour 
{
    public bool detect = false;
    public GameObject turret;
    //public GameObject hero;
   [HideInInspector] public GameObject target;
    // Update is called once per frame

    private void Awake()
    {
        target = this.gameObject;
    }
    void Update () 
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
            

            Debug.Log("I Detect a " + other.name);


            if (hit.collider.tag == "Hero")
            {
                target = other.gameObject;
                detect = true;
            }
            else detect = false;
        }


    }

    private void OnTriggerExit(Collider other)
    {
        if (other == target)
        {
            detect = false;
            target = this.gameObject;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        RaycastHit hit;
        var heading = target.transform.position - turret.transform.position;
        Physics.Raycast(turret.transform.position, heading, out hit);

        if (hit.collider.name != target.name) detect = false;
        else
        {
            detect = true;
            Debug.DrawRay(turret.transform.position, heading, Color.red);
        }


    }

    private void DetectCheck()
    {
        if (detect == true) Debug.Log("Detect is set to TRUE!");
        if (detect == false) Debug.Log("Detect is set to FALSE!");
    }




}
