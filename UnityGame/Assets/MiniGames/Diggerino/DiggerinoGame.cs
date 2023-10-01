using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiggerinoGame : MonoBehaviour
{
    public List<Color> ColorsPerStrength;

    public Color LavaColor;
    public Color DiamondColor;

    public GameObject DiggerinoTilePrefab;
    public GameObject DiggerinoPlayerPrefab;

    public DiggerinoTile[][] Map;
    public int totalDiamonds;

    public int GridSize = 16;
    public List<DiggerinoPlayer> Players = new List<DiggerinoPlayer>();

    // Start is called before the first frame update
    void Start()
    {
        Map = new DiggerinoTile[GridSize][];
        for (int x = 0; x < GridSize; x++)
        {
            Map[x] = new DiggerinoTile[GridSize];
            for (int z = 0; z < GridSize; z++)
            {
                var tile = Instantiate(DiggerinoTilePrefab);
                var dt = tile.GetComponent<DiggerinoTile>();
                tile.transform.position = new Vector3(x, 0, z);
                Map[x][z] = dt;

                if (z < 2)
                    dt.Strength = 0;
                else
                    dt.Strength = Random.Range(1, 6);

                UpdateTileColor(dt);
            }
        }


        for (int x = 0; x < GridSize; x++)
        {
            for (int i = 0; i < 1 + (MinigameManager.Instance.SignalR.Players.Count / 6); i++)
            {
                var z = Random.Range(3, GridSize);
                if (!Map[x][z].Diamond)
                {
                    Map[x][z].Diamond = true;
                    Map[x][z].Strength = 0;
                    totalDiamonds++;
                    UpdateTileColor(Map[x][z]);
                }
            }
        }

        for (int x = 0; x < GridSize; x++)
        {
            for (int i = 0; i < 1; i++)
            {
                var z = Random.Range(3, GridSize);
                if (!Map[x][z].Diamond)
                {
                    Map[x][z].Lava = true;
                }

                UpdateTileColor(Map[x][z]);
            }
        }
    }

    public void PlacePlayers()
    {
        var playerCount = MinigameManager.Instance.SignalR.Players.Values.Count;
        int spawnX = (GridSize / playerCount) / 2;
        foreach (var signalRPlayer in MinigameManager.Instance.SignalR.Players.Values)
        {
            var pi = Instantiate(DiggerinoPlayerPrefab);
            pi.transform.position = new Vector3(spawnX, 0, 0);
            var dp = pi.GetComponent<DiggerinoPlayer>();
            dp.PC = signalRPlayer;
            dp.Game = this;
            dp.PosX = spawnX;
            dp.SpawnX = spawnX;
            dp.PosZ = 0;
            dp.DirectionZ = 1;
            if (signalRPlayer.PlayerImage)
                dp.SetPlayerImage(signalRPlayer.PlayerImage.texture);
            dp.UpdatePos();
            Players.Add(dp);
            spawnX += GridSize / playerCount;
            if (spawnX >= GridSize)
                spawnX -= GridSize;
        }
    }
    public void BindPlayers()
    {
        foreach (var dp in Players)
        {
            dp.PC.OnButton0Press.AddListener(() => dp.TurnLeft());
            dp.PC.OnButton1Press.AddListener(() => dp.MoveForward());
            dp.PC.OnButton2Press.AddListener(() => dp.TurnRight());
        }
    }

    internal void Move(DiggerinoPlayer diggerinoPlayer, int directionX, int directionZ)
    {
        var newX = diggerinoPlayer.PosX + directionX;
        var newZ = diggerinoPlayer.PosZ + directionZ;
        if (newZ >= 16)
            newZ = 15;
        if (newZ < 0)
            newZ = 0;
        if (newX < 0)
            newX = 15;
        if (newX >= 16)
            newX = 0;

        var targetTile = Map[newX][newZ];
        if (targetTile.Strength > 0)
        {
            targetTile.Strength--;
            UpdateTileColor(targetTile);

        }
        else
        {
            if (targetTile.Lava)
            {
                diggerinoPlayer.PosX = diggerinoPlayer.SpawnX;
                diggerinoPlayer.PosZ = 0;
                diggerinoPlayer.UpdatePos();
                diggerinoPlayer.DirectionX = 0;
                diggerinoPlayer.DirectionZ = 1;
                diggerinoPlayer.ChangeRotation();
                targetTile.Lava = false;
                UpdateTileColor(targetTile);
                diggerinoPlayer.PC.ChangeScore(-1);
            }
            else
            {
                if (targetTile.Diamond)
                {
                    targetTile.Diamond = false;
                    UpdateTileColor(targetTile);
                    
                    diggerinoPlayer.PC.ChangeScore(3);
                    totalDiamonds--;
                    if (totalDiamonds <= 0)
                        EndGame();
                }

                diggerinoPlayer.PosX = newX;
                diggerinoPlayer.PosZ = newZ;
                diggerinoPlayer.UpdatePos();
            }
        }
    }

    private void UpdateTileColor(DiggerinoTile targetTile)
    {
        if (targetTile.Diamond)
            targetTile.UpdateColor(DiamondColor);
        else
        {
            if (targetTile.Strength > 0)
                targetTile.UpdateColor(ColorsPerStrength[targetTile.Strength]);
            else if (targetTile.Lava)
                targetTile.UpdateColor(LavaColor);
            else
                targetTile.UpdateColor(ColorsPerStrength[0]);
        }
    }

    private void EndGame()
    {
        FindFirstObjectByType<GameFlow>().EndGame();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
