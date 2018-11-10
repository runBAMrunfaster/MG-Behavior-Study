using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * FOR NEXT TIME!
 * using physics.raycast to detect a player and begin firing!
 */

public class Turret : MonoBehaviour
{
    public int health = 100;

    public Color hitColor = Color.white;

    public GameObject cannonball = null;
    public GameObject deathExplosion = null;
    public Transform player = null;
    public Transform socket = null;
    public Transform self = null;
    public ParticleSystem smoke = null;

    public float minDelay = 1.0f;
    public float maxDelay = 4.0f;

    private Color originalColor = Color.white;
    private float lastTime = 0.0f;
    private float delayTime = 0.0f;
    
    
    

    private void Awake()
    {
        originalColor = this.GetComponent<Renderer>().material.color;
    
    }

    private void Update()
    {
        FollowPlayer();
        Shoot();
    }

    void FollowPlayer()
    {
        this.transform.LookAt(player);
    }

    void Shoot()
    {
        if (Time.time > lastTime + delayTime)
        {
            lastTime = Time.time;
            delayTime = GetRandomValue();

            GameObject obj = Instantiate(cannonball, socket.transform.position, socket.transform.rotation) as GameObject;
            obj.name = "cannonball";

        }
    }

    float GetRandomValue()
    {
        return Random.Range(minDelay, maxDelay);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "magicOrb")
        {
            int hp = other.GetComponent<MagicOrb>().hitpoint;
            GetHealth(hp);
        }
    }

    public void GetHealth(int hp)
    {
        if (health > 0)
        {
            health -= hp;
            if (this.gameObject.tag == "Enemies") StartCoroutine(GetHit());
        }
        else
        {
            smoke = Instantiate(smoke, self.position, self.rotation) as ParticleSystem;
            deathExplosion = Instantiate(deathExplosion, self.position, self.rotation) as GameObject;
            Destroy(this.gameObject);
        }
    }

    public IEnumerator GetHit()
    {
        this.GetComponent<Renderer>().material.color = hitColor;
        yield return new WaitForSeconds(0.4f);
        this.GetComponent<Renderer>().material.color = originalColor;

    }

}
