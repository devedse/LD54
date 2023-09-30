using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiggerinoTile : MonoBehaviour
{
    public int Strength;
    public bool Diamond;
    public bool Lava;
    public MeshRenderer Mesh;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    internal void UpdateColor(Color color)
    {
        Mesh.material.color = color;
    }
}
