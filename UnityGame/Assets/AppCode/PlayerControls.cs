using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    public KeyCode btnLeft = KeyCode.Mouse0;
    public KeyCode btnRight = KeyCode.Mouse1;
    public KeyCode btnMiddle = KeyCode.Mouse2;

    public Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        InputController(btnLeft, btnRight, btnMiddle);
    }

    public void InputController(KeyCode btnLeft, KeyCode btnRight, KeyCode btnMiddle)
    {
        if (Input.GetKeyDown(btnLeft))
        {
            rb.AddTorque(new Vector3(0, -100, 0), ForceMode.Acceleration);
        }

        if (Input.GetKeyDown(btnRight))
        {
            rb.AddTorque(new Vector3(0, 100, 0), ForceMode.Acceleration);
        }

        if (Input.GetKeyDown(btnMiddle))
        {
            rb.AddForce(transform.forward * 10, ForceMode.VelocityChange);
        }
    }
}
