using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiggerinoPlayer : MonoBehaviour
{
    public MeshRenderer PlayerMesh;
    public Transform Arrow;

    public int DirectionX;
    public int DirectionZ;

    public DiggerinoGame Game;
    public int PosX;
    public int PosZ;
    public int SpawnX;
    internal PC PC;

    public void SetPlayerImage(Texture tex)
    {
        PlayerMesh.material.mainTexture = tex;
    }

    public void ChangeRotation()
    {
        Arrow.localEulerAngles = new Vector3(0, DirectionX == 1 ? 90 : DirectionX == -1 ? -90 : DirectionZ == -1 ? 180 : 0, 0);
    }

    public void TurnLeft()
    {
        if (DirectionX == -1)
        {
            DirectionX = 0;
            DirectionZ = -1;
        }
        else if (DirectionX == 1)
        {
            DirectionX = 0;
            DirectionZ = 1;
        }
        else if (DirectionZ == -1)
        {
            DirectionZ = 0;
            DirectionX = 1;
        }
        else if (DirectionZ == 1)
        {
            DirectionZ = 0;
            DirectionX = -1;
        }

        ChangeRotation();
    }

    public void TurnRight()
    {
        if (DirectionX == -1)
        {
            DirectionX = 0;
            DirectionZ = 1;
        }
        else if (DirectionX == 1)
        {
            DirectionX = 0;
            DirectionZ = -1;
        }
        else if (DirectionZ == -1)
        {
            DirectionZ = 0;
            DirectionX = -1;
        }
        else if (DirectionZ == 1)
        {
            DirectionZ = 0;
            DirectionX = 1;
        }
        ChangeRotation();
    }

    public void MoveForward()
    {
        Game.Move(this, DirectionX, DirectionZ);
    }

    internal void UpdatePos()
    {
        transform.position = new Vector3(PosX, 0, PosZ);
    }
}
