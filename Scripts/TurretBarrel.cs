using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TurretBarrel : MonoBehaviour 
{
    public GameObject parent = null;
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "magicOrb")
        {
            int hp = 20; //other.GetComponent<MagicOrb>().hitpoint;
            parent.GetComponent<Turret>().GetHealth(hp);
            parent.GetComponent<Turret>().health -= hp;
        }
    }
}
