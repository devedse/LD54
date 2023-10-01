using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AwardSceneStuff : MonoBehaviour
{
    public Transform ModuleSpawnPoint;
    public GameObject ShipPrefab;
    public GameObject ShipInstance;
    public GameObject OldShipInstance;
    public Transform Spawn;
    public Transform Target;
    private SpaceShipFiller ssf;
    public PC Player;
    public GameObject RewardPrefab;
    public GameObject RewardInstance;

    public int CurrentShipOwnerIndex;

    // Start is called before the first frame update
    void Start()
    {
        ShipInstance = Instantiate(ShipPrefab);
        ShipInstance.transform.position = Spawn.position;
        ShipInstance.transform.LookAt(Target);

        ssf = ShipInstance.GetComponent<SpaceShipFiller>();
        Player = MinigameManager.Instance.SignalR.Players.Values.OrderByDescending(x => x.Score).First();
        CurrentShipOwnerIndex = Player.PlayerIndex;
        ssf.SetProps(Player, FaceType.Normal);

        RewardPrefab = MinigameManager.Instance.NextModuleReward;
        RewardInstance = Instantiate(RewardPrefab);
        RewardInstance.transform.position = ModuleSpawnPoint.position;
        RewardInstance.AddComponent<RotatorScript>().RotatoSpeed = new Vector3(0, 30, 0);

        Player.ResetButtonBindings();
        Player.OnButton0Press.AddListener(() => SelectPlayer(true));
        Player.OnButton2Press.AddListener(() => SelectPlayer(false));
        Player.OnButton1Press.AddListener(() => ChooseCurrentShip());
    }

    private void ChooseCurrentShip()
    {
        Player.ResetButtonBindings();
        Player.OnButton0Press.AddListener(() => ApplyToSlot(0));
        Player.OnButton1Press.AddListener(() => ApplyToSlot(1));
        Player.OnButton2Press.AddListener(() => ApplyToSlot(2));
    }

    public void ApplyToSlot(int slot)
    {
        var current = MinigameManager.Instance.SignalR.GetPlayerByNumber(CurrentShipOwnerIndex);
        current.SetModuleForSlot(slot, RewardPrefab.GetComponent<Module>().ModuleType);
    }

    // Update is called once per frame
    void Update()
    {
        ShipInstance.transform.position = Vector3.Lerp(ShipInstance.transform.position, Target.position, .05f);
        if (OldShipInstance)
        {
            OldShipInstance.transform.position = Vector3.Lerp(OldShipInstance.transform.position, -Spawn.position, .02f);
        }
    }

    public void SelectPlayer(bool left)
    {
        if (left)
            CurrentShipOwnerIndex--;
        if (!left)
            CurrentShipOwnerIndex++;
        if (CurrentShipOwnerIndex < 0)
            CurrentShipOwnerIndex = MinigameManager.Instance.SignalR.Players.Count - 1;
        if (CurrentShipOwnerIndex >= MinigameManager.Instance.SignalR.Players.Count)
            CurrentShipOwnerIndex = 0;

        if (OldShipInstance)
            Destroy(OldShipInstance);
        OldShipInstance = ShipInstance;

        ShipInstance = Instantiate(ShipPrefab);
        ShipInstance.transform.position = Spawn.position;
        ShipInstance.transform.LookAt(Target);
        ssf = ShipInstance.GetComponent<SpaceShipFiller>();
        ssf.SetProps(MinigameManager.Instance.SignalR.GetPlayerByNumber(CurrentShipOwnerIndex), CurrentShipOwnerIndex == Player.PlayerIndex ? FaceType.Happy : FaceType.Mad);

    }
}
