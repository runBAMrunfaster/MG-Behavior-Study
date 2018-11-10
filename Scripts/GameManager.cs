using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour 
{
    public Hero hero = null;
    //public Turret turret = null;

    public Text playerHealth = null;
    public Text magicOrbs = null;
    //public Text turretHealth = null;

    private void Update()
    {
        DisplayProperties();
    }

    void DisplayProperties()
    {
        playerHealth.text = "Player Health: " + hero.health.ToString();
        magicOrbs.text = "Magic Orbs: " + hero.magicOrbAmount.ToString();
        //turretHealth.text = "Turret Health: " + turret.health.ToString();
    }
}
