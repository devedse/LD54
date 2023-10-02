using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
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


    public TextMeshProUGUI LeftButton;
    public TextMeshProUGUI MidButton;
    public TextMeshProUGUI RightButton;
    public TextMeshProUGUI HelpText;
    public TextMeshProUGUI StatsText;

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

        MidButton.text = $"Add {RewardInstance.GetComponent<Module>().DisplayName} to the ship of {Player.PlayerName}";
        HelpText.text = $"Congratulations, {Player.PlayerName}! You have won the right to place module {RewardInstance.GetComponent<Module>().DisplayName} on a ship of your choice! This module is {(MinigameManager.Instance.AllModules.Positives.Any(x => x.GetComponent<Module>().ModuleType == RewardInstance.GetComponent<Module>().ModuleType) ? "helpful" : "harmful")} to a ship!";
        StatsText.text = RewardInstance.GetComponent<Module>().ToStatsString(true);
    }

    private void ChooseCurrentShip()
    {
        Player.ResetButtonBindings();
        Player.OnButton0Press.AddListener(() => ApplyToSlot(0));
        Player.OnButton1Press.AddListener(() => ApplyToSlot(1));
        Player.OnButton2Press.AddListener(() => ApplyToSlot(2));

        HelpText.text = $"Please choose a slot to place the {RewardInstance.GetComponent<Module>().DisplayName} on!";
        var leftMod = ssf.FrontSocket.GetComponent<Module>();
        var midMod = ssf.FrontSocket.GetComponent<Module>();
        var rightMod = ssf.FrontSocket.GetComponent<Module>();

        if (leftMod)
            LeftButton.text = $"Replace {leftMod.DisplayName}";
        else
            LeftButton.text = $"Place on front";

        if (midMod)
            MidButton.text = $"Replace {midMod.DisplayName}";
        else
            MidButton.text = $"Place in middle";

        if (rightMod)
            RightButton.text = $"Replace {rightMod.DisplayName}";
        else
            RightButton.text = $"Place on rear";
    }

    public void ApplyToSlot(int slot)
    {
        var current = MinigameManager.Instance.SignalR.GetPlayerByNumber(CurrentShipOwnerIndex);
        current.SetModuleForSlot(slot, RewardPrefab.GetComponent<Module>().ModuleType);
        Player.ResetButtonBindings();
        StartCoroutine(SwapAndEnd(slot));
    }

    private IEnumerator SwapAndEnd(int slot)
    {
        LeftButton.transform.parent.gameObject.SetActive(false);
        MidButton.transform.parent.gameObject.SetActive(false);
        RightButton.transform.parent.gameObject.SetActive(false);
        HelpText.gameObject.SetActive(false);

        var targetSocket = ssf.GetSocket(slot).transform;
        var targetPos = targetSocket.position;
        var socketStartPos = targetSocket.position;
        var socketEndPos = socketStartPos + new Vector3(0, -4, 0);
        var startPos = RewardInstance.transform.position;
        var startRot = RewardInstance.transform.rotation;
        float timer = 0;
        while (timer < 1)
        {
            timer += Time.deltaTime;
            RewardInstance.transform.position = Vector3.Lerp(startPos, targetPos, timer);
            RewardInstance.transform.rotation = Quaternion.Lerp(startRot, targetSocket.rotation, timer);
            targetSocket.transform.position = Vector3.Lerp(socketStartPos, socketEndPos, timer);
            yield return null;
        }

        Destroy(ShipInstance);
        Destroy(RewardInstance);
        ShipInstance = Instantiate(ShipPrefab);
        ShipInstance.transform.position = Target.position;
        ShipInstance.transform.LookAt(-Spawn.position);
        ssf = ShipInstance.GetComponent<SpaceShipFiller>();
        ssf.SetProps(MinigameManager.Instance.SignalR.GetPlayerByNumber(CurrentShipOwnerIndex), CurrentShipOwnerIndex == Player.PlayerIndex ? FaceType.Happy : FaceType.Mad);

        StartCoroutine(AnimateSeat());
        yield return new WaitForSeconds(5);
        MinigameManager.Instance.StartNextGame();
    }

    public IEnumerator AnimateSeat()
    {
        float timer = 0;
        while (timer < 5)
        {
            timer += Time.deltaTime;

            float bla = timer - (int)timer;
            if (bla < .5f)
                ssf.SetProps(MinigameManager.Instance.SignalR.GetPlayerByNumber(CurrentShipOwnerIndex), CurrentShipOwnerIndex == Player.PlayerIndex ? FaceType.Happy : FaceType.Mad);
            else
                ssf.SetProps(MinigameManager.Instance.SignalR.GetPlayerByNumber(CurrentShipOwnerIndex), FaceType.Normal);

            yield return null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (ShipInstance)
        {
            ShipInstance.transform.position = Vector3.Lerp(ShipInstance.transform.position, Target.position, .05f);
        }
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

        MidButton.text = $"Add {RewardInstance.GetComponent<Module>().DisplayName} to the ship of {MinigameManager.Instance.SignalR.GetPlayerByNumber(CurrentShipOwnerIndex).PlayerName}";

    }
}
