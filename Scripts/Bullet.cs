using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour 
{
    public int hitpoint = 5;
    public float lifespan = 500.0f;
    public float speed = 25.0f;

    private bool canMove = true;
    private Vector3 originPoint;
    private Vector3 currentPoint;
    private bool timetodie = false;


    private void Awake()
    {
        //this.GetComponent<AudioSource>().PlayOneShot(audioShoot);
        originPoint = transform.position;
    }

    private void Update()
    {
        MoveObject();
        currentPoint = transform.position;
        DistanceTraveled();
        if (timetodie == true) Destroy(this.gameObject);
    }

    void MoveObject()
    {
        if (canMove)
        {
            this.transform.Translate(0, 0, speed * Time.deltaTime);
        }

    }

    private bool DistanceTraveled()
    {
        float diff = Mathf.Abs(originPoint.z - currentPoint.z);
        if (diff > lifespan)
        {
            timetodie = true;
        }
        return timetodie;
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.tag != "Enemies" && other.tag != "Detectors")
        {
            //this.GetComponent<AudioSource>().PlayOneShot(audioHit);

            this.GetComponent<Renderer>().enabled = false;
            this.GetComponent<Collider>().enabled = false;
            speed = 0.0f;

            canMove = false;
            //particle.enableEmission = false;
            Destroy(this.gameObject);
        }
    }
}
