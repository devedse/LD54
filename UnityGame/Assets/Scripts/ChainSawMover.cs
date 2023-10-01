using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainSawMover : MonoBehaviour
{
    public float max;
    public float speed;
    private float original;

    // Start is called before the first frame update
    void Start()
    {
        original = transform.localPosition.z;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, (Mathf.Sin(Time.time * speed) + 1) / 2 * max + original);
    }
}
