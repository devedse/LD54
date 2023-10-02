using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiebreakerShip : MonoBehaviour
{
    public PC PC;
    public float Speed;

    public bool finished = false;
    public TiebreakerGame Game;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Speed > 0)
            Speed -= Time.deltaTime * 10;
        if (Speed < 0)
            Speed = 0;

        transform.position += Vector3.right * Speed * Time.deltaTime;
        if (!finished && transform.position.x > 8)
        {
            finished = true;
            Game.CrossedFinishLine(this);
        }
    }

    internal void Tapped()
    {
        Speed += 1f;
    }
}
