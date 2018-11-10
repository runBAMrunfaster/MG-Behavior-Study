using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannonball : MonoBehaviour
{
    
    public float minForce = 400.0f;
    public float maxForce = 700.0f;
    public float delayTime = 2.0f;
    
    public AudioClip audioShoot = null;
    public ParticleSystem particle = null;
    public GameObject explosion = null;
    public Transform self = null;



    private bool isActive = true;

    private void Awake()
    {
        this.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0, GetRandomValue(), GetRandomValue()));
        this.GetComponent<AudioSource>().PlayOneShot(audioShoot);
    }


    float GetRandomValue()
    {
        return Random.Range(minForce, maxForce);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isActive)
        {
            isActive = false;
            GameObject obj = Instantiate(explosion, self.position, self.rotation) as GameObject;
            obj.name = "explosion";
            Destroy(this.gameObject);
            StartCoroutine(DisableParticle());


        }
    }


    IEnumerator DisableParticle()
    {
        yield return new WaitForSeconds(delayTime);

        particle.enableEmission = false;

     
    }
}
