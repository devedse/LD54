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

    public List<GameObject> SocketAttachments;

    public int Armor = 10;
    public int ShipSpeed = 10;
    public float MaxSpeed = 10;
    public int WeaponDamage = 30;
    public float FireRate = 2;
    private float timer;

    private bool isFiring;
    public bool hasArmorUpgrade, hasHealthUpgrade, hasSpeedUpgrade, hasFireRateUpgrade;
    // Weapon upgrades:
    public bool hasChainSaw, hasRocketLauncher, hasBooster, hasShield;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.maxAngularVelocity = MaxSpeed;
        rb.maxLinearVelocity = MaxSpeed;

        timer = FireRate;

        int childCount = transform.childCount;
        
        for (int i = 0; i < transform.childCount; i++)
        {
            //Debug.Log("Found " + transform.GetChild(i).name);

            if (transform.GetChild(i).name.Equals("Hardpoints")) 
            {
                int subChild = transform.GetChild(i).childCount;

                for(int j = 0; j < subChild; j++)
                {
                    //Debug.Log("Found subchild " + transform.GetChild(i).GetChild(j).name);

                    if (transform.GetChild(i).GetChild(j).name == "ChainSaw") {

                        Transform parent = transform.GetChild(i).GetChild(j);
                        parent.gameObject.SetActive(false);

                        //foreach (Transform child in parent) { child.gameObject.SetActive(false); }
                    }
                    if (transform.GetChild(i).GetChild(j).name == "Booster") { 
                        transform.GetChild(i).GetChild(j).gameObject.SetActive(false);
                    }
                    //if (transform.GetChild(i).GetChild(j).name == "Booster") { transform.GetChild(i).GetChild(j).gameObject.SetActive(false); }
                }


            }

            if (transform.GetChild(i).name.Equals("HealthBar"))
            {
                //hud_Health = transform.GetChild(i).GetChild(0);
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (btn_0_pressed) { rb.AddTorque(new Vector3(0, -ShipSpeed, 0), ForceMode.VelocityChange); }
        if (btn_1_pressed) { rb.AddForce(transform.forward * ShipSpeed, ForceMode.VelocityChange); }
        if (btn_2_pressed) { rb.AddTorque(new Vector3(0, ShipSpeed, 0), ForceMode.VelocityChange); }

        timer -= Time.deltaTime;

        if (!hasChainSaw && !hasRocketLauncher)
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
        proj_Pellet.transform.position = hp_Gun.transform.position;
        proj_Pellet.transform.rotation = hp_Gun.transform.rotation;

        isFiring = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name.Contains("Pellet"))
        {
            Destroy(collision.gameObject);

            GetHit(30);
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
            Health -= damage + Armor;
            hud_Health.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Health);
        }
    }
}
