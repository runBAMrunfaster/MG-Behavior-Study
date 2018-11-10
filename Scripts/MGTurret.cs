using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MGTurret : MonoBehaviour
{
    //Object Hit Points
    public int health = 100;

    public Color hitColor = Color.white;

    //To Tom: You need to fix these so they're declared In Code, not in the Inspector.
    //To MK: Please Don't Touch!
    public GameObject cannonball = null;
    public GameObject deathExplosion = null;
    public GameObject detectionCone = null;
    public GameObject firingCone = null;
    public GameObject dicks = null;
    public Transform player = null;
    public Transform socket = null;
    public Transform self = null;
    public ParticleSystem smoke = null;

    //Audio Clips. Look to Awake() for reference pointers
    private AudioClip snd_gunshot;
    private AudioClip snd_fire;
    private AudioClip snd_sweep;
    private AudioClip snd_safe;


    //Gun Timing Behavior. Burst is number of shots in a burst, action is the time between shots in a burst, cooldown is the time to wait between bursts.
    public int burst = 4;
    public float cooldown = 1.0f;
    public float action = 0.1f;

    //Assorted variables used in controlling gun behavior
    private Color originalColor = Color.white;
    public bool canShoot = true;
    private bool haveFired = false;
    private bool detect;
    private bool fireDetect;
    private int shots = 0;
    private float alertTime;
    private GameObject target;
    private Vector3 targetpos;
    private Vector3 targetDir;
    private Vector3 newDir;

    //Turret Sweep variables
    private float sweep;
    private float sweepmin;
    private float sweepmax;
    public float sweepspeed = 20.0f;
    public float sweepangle = 90.0f;
    private bool sweepdirection = true;
    private bool alertReset;
    private float lastChange;
    
    //Turret behavior states.
    enum State {Neutral, Detect, Firing, Alert};
    State currentState;
    State lastState;
 

    private void Start()
    {
        sweep = self.localRotation.eulerAngles.y;
        sweepmin = (sweep - sweepangle) + 360;
        sweepmax = sweep + sweepangle;
        SetState(State.Neutral);
        
    }


    private void Awake()
    {
        originalColor = this.GetComponent<Renderer>().material.color;

        //Identifying audio clips
        snd_gunshot = Resources.Load<AudioClip>("Audio/snd_gunshot");
        snd_fire = Resources.Load<AudioClip>("Audio/snd_fire");
        snd_sweep = Resources.Load<AudioClip>("Audio/snd_sweep");
        snd_safe = Resources.Load<AudioClip>("Audio/snd_safe");

    }

    private void Update()
    {
        switch (currentState)
        {

            //Neutral state. No targets detected. Turret should sweep back and forth between sweepmin and sweepmax. 
            case State.Neutral:
                sweep = self.localRotation.eulerAngles.y;

                if (PlayerDetect())
                {
                    this.GetComponent<AudioSource>().PlayOneShot(snd_fire);
                    SetState(State.Detect);
                }
                if (lastState == State.Alert && alertReset == true)
                {
                    if (sweep <= 180) sweepdirection = false;
                    else sweepdirection = true;
                    alertReset = false;
                }
                else if (sweepdirection == true)
                {

                    this.transform.Rotate(0, (sweepspeed * Time.deltaTime), 0);
                    if (sweep >= sweepmax && sweep < sweepmax + 10)
                    {
                        this.GetComponent<AudioSource>().PlayOneShot(snd_sweep);
                        sweepdirection = false;
                    }

                }
                else if (sweepdirection == false)
                {
                    this.transform.Rotate(0, (-sweepspeed * Time.deltaTime), 0);
                    if (sweep <= sweepmin && sweep > sweepmin - 10)
                    {
                        this.GetComponent<AudioSource>().PlayOneShot(snd_sweep);
                        sweepdirection = true;
                    }
                }
                break;

            //Detect state. Transition state between Neutral and Firing. Turret identifies players location and rotates itself towards the player.
            case State.Detect:
                detect = detectionCone.GetComponent<Detector>().detect;
                fireDetect = firingCone.GetComponent<FiringDetector>().fireDetect;
                target = detectionCone.GetComponent<Detector>().target;
                targetpos = new Vector3(target.transform.position.x, target.transform.position.y, target.transform.position.z);
                
                //If Detection is lost, enters Alert state. Else, if player enters the firing cone, enter Firing state.
                if (detect != true) SetState(State.Alert);
                else if (fireDetect == true)
                {
                    haveFired = true;
                    SetState(State.Firing);
                }

                else
                {
                    targetDir = targetpos - transform.position;
                    newDir = Vector3.RotateTowards(transform.forward, targetDir, (sweepspeed / 5) * Time.deltaTime, 0.0f);
                    transform.rotation = Quaternion.LookRotation(newDir);
                }
                break;
            //Firing state. Target tracking is now 1:1 @ infinite distance w LOS. Gun will fire in bursts. If it shoots ya, it's gonna hurt.
            case State.Firing:
                targetpos = new Vector3(target.transform.position.x, target.transform.position.y, target.transform.position.z);
                RaycastHit hit;
                Physics.Raycast(socket.transform.position, targetpos, out hit);

                if (!PlayerDetect())
                {
                    alertTime = Time.time;
                    SetState(State.Alert);
                }
                FollowTarget(target.transform);
                Shoot();
                break;
            
            //Alert State. Turret will keep looking in the direction of the last target position before LOS was broken.
            case State.Alert:
                if (Time.time > alertTime + 5.0f)
                {
                    haveFired = false;
                    alertReset = true;
                    this.GetComponent<AudioSource>().PlayOneShot(snd_safe);
                    SetState(State.Neutral);
                }

                //If target is re-detected, turret enters Detection state (if no shots were fired) or Firing state (if shots were fired).
                if (PlayerDetect())
                {
                    if (haveFired == true)
                    {
                        SetState(State.Firing);
                    }
                    else SetState(State.Detect);
                }

        
                

                break;


        }
        
    }

    //Method to swap to desired state
    void SetState(State state)
    {
        Debug.Log(this.name + " spent " + Mathf.Round(Time.time - lastChange) + " Seconds in " + currentState + ". Now changing to " + state);
        lastState = currentState;
        currentState = state;
        lastChange = Time.time;
    }

    //Monitoring the detection status of the detection cone, returning true if the cone detects a target.
    public bool PlayerDetect()
    {
        bool detect = detectionCone.GetComponent<Detector>().detect;
        return detect; 

    }

    void FollowTarget(Transform target)
    {
        this.transform.LookAt(target);
    }

    //Aw shoot
    void Shoot()
    {
        if (canShoot == true)
        {
            GameObject obj = Instantiate(cannonball, socket.position, socket.rotation);
            this.GetComponent<AudioSource>().PlayOneShot(snd_gunshot);
            obj.name = "bullet";
            shots++;
            StartCoroutine(Cooldown());
        }
    }

    //If Hit by Projectile
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "magicOrb")
        {
            int hp = other.GetComponent<MagicOrb>().hitpoint;
            GetHealth(hp);
        }
    }

    //Method for handing taking damage. 
    public void GetHealth(int hp)
    {
        if (health > 0)
        {
            health -= hp;
            Debug.Log("MG Turret Health: " + health);
            if (this.gameObject.tag == "Enemies") StartCoroutine(GetHit());
        }
        //Death sequence.
        else
        {
            smoke = Instantiate(smoke, self.position, self.rotation) as ParticleSystem;
            deathExplosion = Instantiate(deathExplosion, self.position, self.rotation) as GameObject;
            Destroy(this.gameObject);
        }
    }



    //Coroutine for visual depections of hits. Runs with GetHealth().
    public IEnumerator GetHit()
    {
        this.GetComponent<Renderer>().material.color = hitColor;
        yield return new WaitForSeconds(0.4f);
        this.GetComponent<Renderer>().material.color = originalColor;

    }

    //Coroutine for handling gun timings. 
    public IEnumerator Cooldown()
    {
        canShoot = false;

        if (shots < burst)
        {
            yield return new WaitForSeconds(action);
            canShoot = true;
        }
        else if (shots >= burst)
        {
            yield return new WaitForSeconds(cooldown);
            canShoot = true;
            shots = 0;
        }
    }
}
