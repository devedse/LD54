using UnityEngine;

public class Pellet : MonoBehaviour
{
    public Rigidbody rb;
    public int proj_Force = 10;
    public float timer, proj_LifeTime = 3;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.AddForce(transform.forward * proj_Force, ForceMode.Force);

        timer += Time.deltaTime;

        if (timer >= proj_LifeTime)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);

        if (collision.gameObject.name.Contains("player"))
        {
            Destroy(collision.gameObject);
        }
    }
}
