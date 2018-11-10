using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    public Color hitColor = Color.white;

    public int health = 100;
    public float moveSpeed = 5.0f;
    public float rotateSpeed = 100.0f;
    public GameObject magicOrb = null;
    public Transform socket = null;
    public int magicOrbAmount = 20;

    private Color originalColor = Color.white;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void Awake()
    {
        originalColor = this.GetComponent<Renderer>().material.color;
    }

    private void Update()
    {
        Move();
        Shoot();
        if (Input.GetKeyDown("escape"))
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    private void Move()
    {
        float translation = Input.GetAxis("Vertical") * moveSpeed;
        float strafe = Input.GetAxis("Horizontal") * moveSpeed;
        translation *= Time.deltaTime;
        strafe *= Time.deltaTime;

        transform.Translate(strafe, 0, translation);
        
        /*
        float vmove = Input.GetAxis("Vertical") * moveSpeed;
        float hmove = Input.GetAxis("Horizontal") * moveSpeed;
        float rotation = Input.GetAxis("Mouse X") * rotateSpeed;
        float pitch = Input.GetAxis("Mouse Y") * rotateSpeed;
        this.transform.Translate(0, 0, vmove * Time.deltaTime);
        this.transform.Translate(hmove * Time.deltaTime, 0, 0);
        this.transform.Rotate(0, rotation * Time.deltaTime, 0);
        this.transform.Rotate(pitch * Time.deltaTime, 0, 0);
        */
    }

    void Shoot()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (magicOrbAmount > 0)
            {
                magicOrbAmount--;
                GameObject obj = Instantiate(magicOrb, socket.position, socket.rotation) as GameObject;
                obj.name = "magicOrb";
            }

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "projectile")
        {
            int hp = other.GetComponent<Bullet>().hitpoint; //Need way to call correct damage value! Maybe a new method/Class to store a damage table?
            GetHealth(hp);
        }
    }


    void GetHealth(int hp)
    {
        if (health > 0)
        {
            health -= hp;
            Debug.Log("Hero Hit! Health: " + health);
            StartCoroutine(GetHit());
        }
        else
        {
            Debug.Log("Game Over Man!");
        }
    }

    IEnumerator GetHit()
    {
        this.GetComponent<Renderer>().material.color = hitColor;
        yield return new WaitForSeconds(0.4f);
        this.GetComponent<Renderer>().material.color = originalColor;

    }

}
