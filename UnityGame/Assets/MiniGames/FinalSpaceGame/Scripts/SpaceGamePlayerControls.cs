using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    public Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //InputController(btnLeft, btnRight, btnMiddle);
    }

    public void Button0Pressed()
    {
        rb.AddTorque(new Vector3(0, -100, 0), ForceMode.Acceleration);
    }

    public void Button1Pressed()
    {
        rb.AddForce(transform.forward * 10, ForceMode.VelocityChange);
    }

    public void Button2Pressed()
    {
        rb.AddTorque(new Vector3(0, 100, 0), ForceMode.Acceleration);
    }
}
