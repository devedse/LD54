using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AwardSceneStuff : MonoBehaviour
{
    public Transform ModuleSpawnPoint;
    public GameObject ShipPrefab;
    public GameObject ShipInstance;
    public Transform Spawn;
    public Transform Target;
    private SpaceShipFiller ssf;
    public PC Player;
    public GameObject RewardPrefab;
    public GameObject RewardInstance;

    // Start is called before the first frame update
    void Start()
    {
        ShipInstance = Instantiate(ShipPrefab);
        ShipInstance.transform.position = Spawn.position;
        ShipInstance.transform.LookAt(Target);

        ssf = ShipInstance.GetComponent<SpaceShipFiller>();
        ssf.SetProps(MinigameManager.Instance.SignalR.Players.Values.OrderByDescending(x => x.Score).First(), FaceType.Normal);

        RewardPrefab = MinigameManager.Instance.NextModuleReward;
        RewardInstance = Instantiate(RewardPrefab);
        RewardInstance.transform.position = ModuleSpawnPoint.position;
        RewardInstance.AddComponent<RotatorScript>().RotatoSpeed = new Vector3(0, 30, 0);
    }

    // Update is called once per frame
    void Update()
    {
        ShipInstance.transform.position = Vector3.Lerp(ShipInstance.transform.position, Target.position, .02f);
    }

    public void ChooseUpgradeSlot(int slot)
    {

    }
}
