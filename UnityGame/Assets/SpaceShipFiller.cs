using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShipFiller : MonoBehaviour
{
    public MeshRenderer FaceL;
    public MeshRenderer FaceR;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetProps(PC player)
    {
        if (player?.PlayerImage?.texture != null)
        {
            FaceL.material.mainTexture = player.PlayerImage.texture;
            FaceR.material.mainTexture = player.PlayerImage.texture;
        }
    }
}
