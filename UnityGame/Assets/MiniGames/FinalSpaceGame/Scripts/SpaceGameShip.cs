using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceGameShip : MonoBehaviour
{
    public int Health = 100;
    public int Armor = 10;
    public int ShipSpeed = 10;
    public int WeaponDamage = 30;
    public float FireRate = 2;

    public bool hasArmorUpgrade, hasHealthUpgrade, hasSpeedUpgrade, hasFireRateUpgrade;
    // Weapon upgrades:
    public bool hasSawBlade, hasRocketLauncher, hasBooster, hasShield;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
