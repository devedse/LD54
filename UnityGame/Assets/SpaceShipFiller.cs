using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShipFiller : MonoBehaviour
{
    public MeshRenderer FaceL;
    public MeshRenderer FaceR;

    public GameObject FrontSocket;
    public GameObject SideSocket;
    public GameObject RearSocket;

    public List<GameObject> Modules;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddModule(int moduleNumber, int socketNumber)
    {
        var module = Modules[moduleNumber];

        var socket = socketNumber switch
        {
            0 => FrontSocket,
            1 => SideSocket,
            2 => RearSocket,
            _ => throw new System.Exception("Invalid socket number")
        }; ;

        socket.RemoveAllChildObjects();

        var moduleInstantiated = GameObject.Instantiate(module);
        moduleInstantiated.transform.localPosition = Vector3.zero;
        moduleInstantiated.transform.localRotation = Quaternion.identity;
    }

    public void SetProps(PC player)
    {
        if (player?.PlayerImage?.texture != null)
        {
            FaceL.material.mainTexture = player.PlayerImage.texture;
            FaceR.material.mainTexture = player.PlayerImage.texture;

            AddModule(0, 0);
            AddModule(1, 1);
            AddModule(2, 2);
        }
    }
}
