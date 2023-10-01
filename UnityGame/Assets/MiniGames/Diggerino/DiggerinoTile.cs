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
    public MeshRenderer MeshOnTop;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    internal void UpdateMeshOnTop(Material mat, Color color)
    {
        if (mat)
        {
            MeshOnTop.gameObject.SetActive(true);
            MeshOnTop.material = mat;
            MeshOnTop.material.color = color;
        }
        else
        {
            MeshOnTop.gameObject.SetActive(false);
        }
    }
}
