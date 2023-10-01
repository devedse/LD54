using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    public GameObject player, hp_Gun, pellet;
    public Rigidbody rb;
    private bool btn_0_pressed, btn_1_pressed, btn_2_pressed;

    public int Health = 100;
    public int CurrentHealth = 100;
    public RectTransform hud_Health;

    public int Armor = 10;
    public int ShipSpeed = 10;
    public float MaxSpeed = 10;
    public int WeaponDamage = 30;
    public float FireRate = 2;
    private float timer;

    private bool isFiring;
    public bool hasArmorUpgrade, hasHealthUpgrade, hasSpeedUpgrade, hasFireRateUpgrade, hasWeapomUpgrade;
    // Weapon upgrades:
    public bool hasChainSaw, hasBooster, hasShield;

    public PC pcplayer;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.maxAngularVelocity = MaxSpeed;
        rb.maxLinearVelocity = MaxSpeed;

        timer = FireRate;

        int childCount = transform.childCount;

        for (int i = 0; i < 3; i++)
        {
            ShipModuleType shipModule = pcplayer.GetModuleForSlot(i);

            switch (shipModule)
            {
                case ShipModuleType.None:
                    ShipSpeed += 2;
                    break;
                case ShipModuleType.Chainsaw:
                    hasChainSaw = true;
                    Armor += 5;
                    //var chain = this.GetComponentInChildren<ChainSawDingetje>();
                    //chain.OnChainsawEnter += ;

                    break;
                case ShipModuleType.Booster:
                    hasBooster = true;
                    hasFireRateUpgrade = true;
                    ShipSpeed += 5;
                    FireRate -= 0.1f;
                    break;
                case ShipModuleType.Parasolding:
                    ShipSpeed -= 3;
                    break;
                case ShipModuleType.Turbine:
                    ShipSpeed += 4;
                    break;
                case ShipModuleType.Squid:
                    ShipSpeed -= 2;
                    FireRate += 0.2f;
                    break;
                case ShipModuleType.Adelaar:
                    hasWeapomUpgrade = true;
                    FireRate -= 0.4f;
                    WeaponDamage += 15;
                    break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (btn_0_pressed) { rb.AddTorque(new Vector3(0, -ShipSpeed, 0), ForceMode.VelocityChange); }
        if (btn_1_pressed) { rb.AddForce(transform.forward * ShipSpeed, ForceMode.VelocityChange); }
        if (btn_2_pressed) { rb.AddTorque(new Vector3(0, ShipSpeed, 0), ForceMode.VelocityChange); }


        DefaultGun();
    }

    public void DefaultGun()
    {
        timer -= Time.deltaTime;

        if (!hasChainSaw)
        {
            if (timer <= 0)
            {
                timer += FireRate;

                FireWeapon();
            }

            if (isFiring)
            {
                FireWeapon();
            }
        }
    }

    public void Button0Pressed()
    {
        btn_0_pressed = true;
    }

    public void Button1Pressed()
    {
        btn_1_pressed = true;
    }

    public void Button2Pressed()
    {
        btn_2_pressed = true;
    }

    public void Button0Released()
    {
        btn_0_pressed = false;
    }

    public void Button1Released()
    {
        btn_1_pressed = false;
    }

    public void Button2Released()
    {
        btn_2_pressed = false;
    }

    void FireWeapon()
    {
        GameObject proj_Pellet = Instantiate(pellet);

        Pellet myPellet = proj_Pellet.GetComponentInParent<Pellet>();
        myPellet.damage = WeaponDamage;
        myPellet.pelletOwner = pcplayer;

        proj_Pellet.transform.position = hp_Gun.transform.position;
        proj_Pellet.transform.rotation = hp_Gun.transform.rotation;

        isFiring = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name.Contains("Pellet"))
        {
            Pellet hitPellet = collision.gameObject.transform.GetComponentInParent<Pellet>();

            MinigameManager.Instance.SignalR.GetPlayerByNumber(hitPellet.pelletOwner.PlayerIndex).ChangeScore(1);

            GetHit(hitPellet.damage);
            Destroy(collision.gameObject);
        }
    }

    private void GetHit(int damage)
    {
        if (Health <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
            Health = Health - Mathf.Max(damage - Armor, 5);

            hud_Health.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Health);
        }
    }
}
