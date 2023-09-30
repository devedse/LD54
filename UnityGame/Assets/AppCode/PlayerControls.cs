using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    public KeyCode btnLeft = KeyCode.Mouse0;
    public KeyCode btnRight = KeyCode.Mouse1;
    public KeyCode btnMiddle = KeyCode.Mouse2;

    // Start is called before the first frame update
    void Start()
    {
        
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
            var rotation = this.transform.localEulerAngles;
            var rotationQuat = Quaternion.Euler(new Vector3(rotation.x, rotation.y - 10, rotation.z));
            this.transform.localRotation = rotationQuat;
        }

        if (Input.GetKeyDown(btnRight))
        {
            Quaternion rotation = this.transform.localRotation;
            rotation = Quaternion.Euler(new Vector3(rotation.x, rotation.y + 10, rotation.z));
            this.transform.localRotation = rotation;
        }

        if (Input.GetKeyDown(btnMiddle))
        {
            
            Vector3 position = this.transform.localPosition;
            position += transform.forward * 5;
            this.transform.localPosition = position;
        }
    }
}
