using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour 
{
    public int hitpoint = 20;
    public AudioClip audioHit = null;
    //private bool active = true;
    public float delayTime = 0.75f;


    // Use this for initialization
    void Awake () 
	{
        audioHit = Resources.Load<AudioClip>("Audio/snd_cannonballHit");
        this.GetComponent<AudioSource>().PlayOneShot(audioHit);

    }
	
	// Update is called once per frame
	void Update () 
	{
        StartCoroutine(Cleanup());
    }

    IEnumerator Cleanup()
    {
        yield return new WaitForSeconds(delayTime);
        Destroy(this.gameObject);


    }
}


