using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicOrb : MonoBehaviour
{
    //speed, hitpoint, shoot sound, hit sound, particle

    public int hitpoint = 10;
    public float lifespan = 500.0f;
    public float speed = 5.0f;

    //Audio Clips used. Look to Awake() for identifiers.
    [HideInInspector] public AudioClip audioHit = null;
    [HideInInspector] public AudioClip audioShoot = null;


    public ParticleSystem particle = null;
    private bool canMove = true;
    private Vector3 originPoint;
    private Vector3 currentPoint;
    private bool timetodie = false;

    private void Awake()
    {
        audioHit = Resources.Load<AudioClip>("snd_orbHit");
        audioShoot = Resources.Load<AudioClip>("snd_orbShoot");
        this.GetComponent<AudioSource>().PlayOneShot(audioShoot);
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
        //Move Forward @ speed.
        if (canMove)
        {
            this.transform.Translate(0, 0, speed * Time.deltaTime);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        //If it hits something solid, play the hit sound and kill yourself.
        if (other.tag != "Detectors")
        {
            this.GetComponent<AudioSource>().PlayOneShot(audioHit);

            this.GetComponent<Renderer>().enabled = false;
            this.GetComponent<Collider>().enabled = false;
            speed = 0.0f;

            canMove = false;
            particle.enableEmission = false;
            Destroy(this.gameObject, audioHit.length);
        }
    }


    //Method to track distance the orb has traveled and call when it's gone TOO FAR THIS TIME, ANIKIN.
    private bool DistanceTraveled()
    {
        float diff = Mathf.Abs(originPoint.z - currentPoint.z);
        if ( diff > lifespan)
        {
            timetodie = true;
        }
        return timetodie;
    }

}
