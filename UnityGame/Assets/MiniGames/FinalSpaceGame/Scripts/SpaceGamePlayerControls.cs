using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    public GameObject player, hp_Gun, pellet;
    public Rigidbody rb;
    private bool btn_0_pressed, btn_1_pressed, btn_2_pressed;

    public int Health = 100;
    public int Armor = 10;
    public int ShipSpeed = 10;
    public float MaxSpeed = 10;
    public int WeaponDamage = 30;
    public float FireRate = 2;
    private float timer;

    private bool isFiring;
    public bool hasArmorUpgrade, hasHealthUpgrade, hasSpeedUpgrade, hasFireRateUpgrade;
    // Weapon upgrades:
    public bool hasSawBlade, hasRocketLauncher, hasBooster, hasShield;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.maxAngularVelocity = MaxSpeed;
        rb.maxLinearVelocity = MaxSpeed;

        timer = FireRate;

        Transform getClass = transform.Find("SpaceGameShip");
        Debug.Log(getClass);
    }

    // Update is called once per frame
    void Update()
    {
        if (btn_0_pressed) { rb.AddTorque(new Vector3(0, -ShipSpeed, 0), ForceMode.VelocityChange); }
        if (btn_1_pressed) { rb.AddForce(transform.forward * ShipSpeed, ForceMode.VelocityChange); }
        if (btn_2_pressed) { rb.AddTorque(new Vector3(0, ShipSpeed, 0), ForceMode.VelocityChange); }

        timer -= Time.deltaTime;

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
}
