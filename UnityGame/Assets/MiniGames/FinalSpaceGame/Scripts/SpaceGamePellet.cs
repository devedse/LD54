using UnityEngine;

public class Pellet : MonoBehaviour
{
    public Rigidbody rb;
    public int proj_Force = 3;
    public float timer, proj_LifeTime = 3;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        rb.AddForce(transform.forward * proj_Force, ForceMode.VelocityChange);
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
        Destroy(gameObject);

        if (collision.gameObject.name.Contains("Player"))
        {
            Destroy(collision.gameObject);
        }
    }
}
