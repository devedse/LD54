using UnityEngine;

public class Pellet : MonoBehaviour
{
    private Rigidbody rb;
    public int proj_Force = 25;
    private float timer;
    public float proj_LifeTime = 3;
    public float bulledSpeedModifier = 1;

    public float damageModifier;
    public PC pelletOwner;

    // Start is called before the first frame update
    void Start()
    {
        name = "Pellet";
        rb = GetComponent<Rigidbody>();

        rb.AddForce(transform.forward * (proj_Force * bulledSpeedModifier), ForceMode.VelocityChange);

        this.transform.localScale = Vector3.one * Mathf.Clamp(damageModifier, 0.5f, 1.5f);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= proj_LifeTime)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.name.Contains("Player") && !collision.gameObject.name.Contains("Pellet"))
        {
            Destroy(gameObject);
        }
    }

}
