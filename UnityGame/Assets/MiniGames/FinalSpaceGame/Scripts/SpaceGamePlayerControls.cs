using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    public Rigidbody rb;
    private bool btn_0_pressed, btn_1_pressed, btn_2_pressed;

    private int shipSpeed;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.maxAngularVelocity = 20;
        rb.maxLinearVelocity = 20;
    }

    // Update is called once per frame
    void Update()
    {
        if (btn_0_pressed) { rb.AddTorque(new Vector3(0, -100, 0), ForceMode.Acceleration); }
        if (btn_1_pressed) { rb.AddForce(transform.forward * shipSpeed, ForceMode.VelocityChange); }
        if (btn_2_pressed) { rb.AddTorque(new Vector3(0, 100, 0), ForceMode.Acceleration); }
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
}
