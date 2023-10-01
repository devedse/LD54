using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShipFiller : MonoBehaviour
{
    public MeshRenderer FaceL;
    public MeshRenderer FaceR;

    public MeshRenderer ShipRenderer;

    public GameObject FrontSocket;
    public GameObject SideSocket;
    public GameObject RearSocket;

    public ShipModuleScriptableObject Modules;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetModule(int socketNumber, int? moduleNumber)
    {
        var socket = socketNumber switch
        {
            0 => FrontSocket,
            1 => SideSocket,
            2 => RearSocket,
            _ => throw new System.Exception("Invalid socket number")
        }; ;
        socket.RemoveAllChildObjects();

        if (moduleNumber == null)
        {
            return;
        }
        var module = Modules.AllShipModules[moduleNumber.Value];

        var moduleInstantiated = GameObject.Instantiate(module, socket.transform);
        moduleInstantiated.transform.localPosition = Vector3.zero;
        moduleInstantiated.transform.localRotation = Quaternion.identity;
    }

    public void SetProps(PC player, FaceType faceType = FaceType.Normal)
    {
        if (player?.PlayerImage?.texture != null)
        {
            var desiredColor = MinigameManager.Instance.GetPlayerColor(player.PlayerIndex);

            var texture = faceType switch
            {
                FaceType.Normal => player.PlayerImage.texture,
                FaceType.Happy => player.PlayerHappy.texture,
                FaceType.Mad => player.PlayerMad.texture,
                _ => throw new System.Exception("Invalid face type")
            };

            FaceL.material.mainTexture = texture;
            FaceR.material.mainTexture = texture;

            bool randomModules = true;
            if (randomModules)
            {
                var totalModules = Modules.AllShipModules.Count - 1;
                SetModule(0, ((0 + player.PlayerIndex) % totalModules) + 1);
                SetModule(1, ((1 + player.PlayerIndex) % totalModules) + 1);
                SetModule(2, ((2 + player.PlayerIndex) % totalModules) + 1);
            }
            else
            {
                for (int i = 0; i < 3; i++)
                {
                    var module = player.GetModuleForSlot(i);
                    if (module == ShipModuleType.None)
                    {
                        SetModule(i, null);
                    }
                    else
                    {
                        SetModule(i, null);
                    }
                }
            }

            ShipRenderer.material.color = desiredColor;
        }
    }
}

public enum FaceType
{
    Normal,
    Happy,
    Mad
}
