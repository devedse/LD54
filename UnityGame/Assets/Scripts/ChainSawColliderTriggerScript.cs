using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ChainSawDingetje : MonoBehaviour
{
    public UnityEvent<Collider> OnChainsawEnter;
    public UnityEvent<Collider> OnChainsawLeave;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        OnChainsawEnter.Invoke(other);
    }

    private void OnTriggerExit(Collider other)
    {
        OnChainsawLeave.Invoke(other);
    }
}
