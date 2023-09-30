using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScreen : MonoBehaviour
{
    public SignalRTest SignalR;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPess(int index)
    {
        SignalR.SendButtonPress(index, true);
    }

    public void OnRelease(int index)
    {
        SignalR.SendButtonPress(index, false);
    }
}
