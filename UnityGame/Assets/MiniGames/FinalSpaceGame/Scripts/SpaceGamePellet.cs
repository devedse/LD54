using UnityEngine;

public class Pellet : MonoBehaviour
{
    public Rigidbody rb;
    public int proj_Force = 25;
    public float timer, proj_LifeTime = 3;

    public float damageModifier;
    public PC pelletOwner;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        rb.AddForce(transform.forward * proj_Force, ForceMode.VelocityChange);

        this.transform.localScale = Vector3.one * damageModifier;
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
        if (!collision.gameObject.name.Contains("Player"))
        {
            Destroy(gameObject);
        }
    }

}
