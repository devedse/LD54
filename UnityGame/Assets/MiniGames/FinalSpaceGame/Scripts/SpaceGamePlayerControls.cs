using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    public GameObject player, hp_Gun, pellet;
    public Rigidbody rb;
    private bool btn_0_pressed, btn_1_pressed, btn_2_pressed;

    public float MaxHealth = 100;
    public float CurrentHealth = 100;
    public RectTransform hud_Health;

    private float lastFireTime;

    public PC pcplayer;

    private float _originalPositionY = 0;


    public Module[] GetAllModules()
    {
        return GetComponentsInChildren<Module>();
    }

    public IEnumerable<float> SumField(Func<ModuleScriptableObject, float> selector)
    {
        return GetAllModules().Select(t => t.ModuleModifiers).Select(selector);
    }

    public float SpeedModifier => (1 + SumField(t => t.SpeedModifier).Sum()).AtLeast(0.2f);
    public float RotationSpeedModifier => (1 + SumField(t => t.RotationSpeedModifier).Sum()).AtLeast(0.2f);
    public float DamageModifier => (1 + SumField(t => t.DamageModifier).Sum()).AtLeast(0);
    public float FireRateModifier => (1 + SumField(t => t.FireRateModifier).Sum()).AtLeast(0);
    public float BulletSpeedModifier => (1 + SumField(t => t.BulletSpeedModifier).Sum()).AtLeast(0);
    public float ArmorModifier => 0 + SumField(t => t.ArmorModifier).Sum().AtLeast(0);
    public float BulletRangeModifier => (1 + SumField(t => t.BulletRangeModifier).Sum()).AtLeast(0.1f);

    private const float DefaultDamage = 20f;



    // Start is called before the first frame update
    void Start()
    {
        pcplayer.ChangeScore(100);

        _originalPositionY = rb.position.y;

        rb = GetComponent<Rigidbody>();
        lastFireTime = Time.time;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var rotatoOngeveer1PerSecond = Mathf.PI;
        var rotatoNu = rotatoOngeveer1PerSecond * RotationSpeedModifier;

        var baseSpeed = 10f;

        if (btn_0_pressed)
        {
            rb.AddTorque(new Vector3(0, rotatoNu, 0), ForceMode.VelocityChange);
        }
        if (btn_1_pressed)
        {
            rb.AddForce(transform.forward * (SpeedModifier * baseSpeed), ForceMode.VelocityChange);
        }
        if (btn_2_pressed)
        {
            rb.AddTorque(new Vector3(0, -rotatoNu, 0), ForceMode.VelocityChange);
        }

        //Fix always upright ofzo
        rb.MoveRotation(Quaternion.Euler(0, rb.rotation.eulerAngles.y, 0));
        rb.MovePosition(new Vector3(rb.position.x, _originalPositionY, rb.position.z));

        DefaultGun();
    }

    public void DefaultGun()
    {
        var timePerShot = 1 / FireRateModifier;

        if (lastFireTime + timePerShot < Time.time)
        {
            lastFireTime = Time.time;

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

        Pellet myPellet = proj_Pellet.GetComponentInParent<Pellet>();
        myPellet.damageModifier = DamageModifier;
        myPellet.pelletOwner = pcplayer;
        myPellet.proj_LifeTime = BulletRangeModifier;
        myPellet.bulledSpeedModifier = BulletSpeedModifier;

        proj_Pellet.transform.position = hp_Gun.transform.position;
        proj_Pellet.transform.rotation = hp_Gun.transform.rotation;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name.Contains("Pellet"))
        {
            Pellet hitPellet = collision.gameObject.transform.GetComponentInParent<Pellet>();
            if (hitPellet.pelletOwner != pcplayer)
            {


                GetHit(hitPellet.damageModifier);
                Destroy(collision.gameObject);
            }

        }
    }

    private void GetHit(float damageModifier)
    {
        CurrentHealth -= Math.Max(2, (DefaultDamage * damageModifier) - (ArmorModifier * 5));

        pcplayer.ForceScore(Math.Max(0, (int)CurrentHealth), -1);


        hud_Health.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, CurrentHealth);


        if (CurrentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
