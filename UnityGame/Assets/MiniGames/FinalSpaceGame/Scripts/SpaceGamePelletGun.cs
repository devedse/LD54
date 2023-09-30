using UnityEngine;

public class PelletGun : MonoBehaviour
{
    public GameObject player, hp_Gun, pellet;
    
    private bool isFiring;
    public float timer = 2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0) {
            timer += 2;

            FireWeapon();
        }

        if (isFiring)
        {
            FireWeapon();
        }
    }

    void FireWeapon()
    {
        GameObject proj_Pellet = Instantiate(pellet);
        proj_Pellet.transform.position = hp_Gun.transform.position;
        proj_Pellet.transform.rotation = hp_Gun.transform.rotation;

        Debug.Log("Firing Weaponn");

        isFiring = false;
    }
}
