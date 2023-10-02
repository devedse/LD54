using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class FireWaterNatureGame : MonoBehaviour
{
    public GameObject ShipPrefab;

    public GameObject FireBeamPrefab;
    public GameObject WaterBeamPrefab;
    public GameObject NatureBeamPrefab;

    public GameObject FireWhirlwindPrefab;
    public GameObject WaterWhirlwindPrefab;
    public GameObject NatureWhirlwindPrefab;

    public GameObject HelperCanvas;

    public Transform Center;
    public Transform Edge;

    public Dictionary<PC, FireWaterNature> Choices;
    public Dictionary<PC, SpaceShipFiller> Ships;

    public bool TimerStarted;
    public bool ShowingResults;

    public TextMeshProUGUI CountdownTimerText;

    // Start is called before the first frame update
    void Start()
    {
        PrepGame();
    }

    // Update is called once per frame
    void Update()
    {


    }

    public void PrepGame()
    {
        var players = MinigameManager.Instance.SignalR.Players.Values;
        var playerCount = players.Count;
        var anglePerPlayer = 360f / playerCount;

        Ships = new Dictionary<PC, SpaceShipFiller>();

        foreach (var p in players.OrderBy(x => x.PlayerIndex))
        {
            var shipInstance = Instantiate(ShipPrefab);
            var sf = shipInstance.GetComponent<SpaceShipFiller>();
            Ships.Add(p, sf);
            sf.SetProps(p, FaceType.Normal);
            shipInstance.transform.position = Edge.position;
            shipInstance.transform.LookAt(Center);
            Center.Rotate(Vector3.up, anglePerPlayer);
        }

        Choices = new Dictionary<PC, FireWaterNature>();
    }

    public void Choose(PC pc, SpaceShipFiller ssf, FireWaterNature choice)
    {
        if (!Ships.ContainsKey(pc))
            return;

        if (Choices.ContainsKey(pc))
            return;

        if (ShowingResults)
            return;

        Choices.Add(pc, choice);
        StartCoroutine(Move(ssf, true));

        ChoiceMade();
    }

    private void ChoiceMade()
    {
        var totalPlayers = (float)Ships.Count;
        var chosen = Choices.Count;
        var percDone = chosen / totalPlayers;
        if (percDone >= .5f && !TimerStarted)
        {
            TimerStarted = true;
            StartCoroutine(StartCountdown());
        }
        if (Ships.Count == Choices.Count)
        {
            StartCoroutine(ShowResult());
        }
    }

    public IEnumerator StartCountdown()
    {
        CountdownTimerText.gameObject.SetActive(true);
        float timer = 10;
        while (timer > 0)
        {
            timer -= Time.deltaTime;

            CountdownTimerText.text = ((int)timer).ToString();

            yield return null;
        }
        if (!ShowingResults)
            StartCoroutine(ShowResult());
    }

    public IEnumerator ShowResult()
    {
        HelperCanvas.SetActive(false);
        CountdownTimerText.gameObject.SetActive(false);
        ShowingResults = true;

        yield return new WaitForSeconds(1);
        foreach (var c in Choices)
        {
            var spawnPos = Ships[c.Key].transform.position;
            GameObject spawned = null;
            if (c.Value == FireWaterNature.Fire)
                spawned = Instantiate(FireBeamPrefab);
            if (c.Value == FireWaterNature.Water)
                spawned = Instantiate(WaterBeamPrefab);
            if (c.Value == FireWaterNature.Nature)
                spawned = Instantiate(NatureBeamPrefab);
            if (spawned)
            {
                spawned.transform.position = spawnPos;
                spawned.transform.LookAt(Center);
            }
        }

        yield return new WaitForSeconds(1);

        var countFire = Choices.Values.Count(x => x == FireWaterNature.Fire);
        var countWater = Choices.Values.Count(x => x == FireWaterNature.Water);
        var countNature = Choices.Values.Count(x => x == FireWaterNature.Nature);

        bool spawnFire = false, spawnWater = false, spawnNature = false;
        var highest = Mathf.Max(countFire, countWater, countNature);
        var matches = 0;
        if (countFire > countWater && countFire >= countNature) { matches++; spawnFire = true; }
        if (countWater > countNature && countWater >= countFire) { matches++; spawnWater = true; }
        if (countNature > countFire && countNature >= countWater) { matches++; spawnNature = true; }

        List<PC> losers = new List<PC>();
        if (matches > 1)
        {
        }
        else
        {
            if (countFire == highest && spawnFire)
            {
                SpawnWhirlwind(FireWhirlwindPrefab);
                losers.AddRange(Choices.Where(x => x.Value == FireWaterNature.Nature).Select(x => x.Key));
                if (losers.Any())
                    foreach (var winner in Choices.Where(x => x.Value == FireWaterNature.Fire))
                        winner.Key.ChangeScore(1);
            }
            if (countWater == highest && spawnWater)
            {
                SpawnWhirlwind(WaterWhirlwindPrefab);
                losers.AddRange(Choices.Where(x => x.Value == FireWaterNature.Fire).Select(x => x.Key));
                if (losers.Any())
                    foreach (var winner in Choices.Where(x => x.Value == FireWaterNature.Water))
                        winner.Key.ChangeScore(1);
            }
            if (countNature == highest && spawnNature)
            {
                SpawnWhirlwind(NatureWhirlwindPrefab);
                losers.AddRange(Choices.Where(x => x.Value == FireWaterNature.Water).Select(x => x.Key));
                if (losers.Any())
                    foreach (var winner in Choices.Where(x => x.Value == FireWaterNature.Nature))
                        winner.Key.ChangeScore(1);
            }
        }
        losers.AddRange(Ships.Keys.Except(Choices.Keys));
        yield return new WaitForSeconds(1);

        foreach (var loser in losers)
        {
            Destroy(Ships[loser].gameObject);
            Ships.Remove(loser);
        }
        yield return new WaitForSeconds(1);

        foreach (var ship in Ships)
        {
            ship.Key.ChangeScore(2);
            StartCoroutine(Move(ship.Value, false));
        }
        yield return new WaitForSeconds(2);
        ResetGamestate();
    }

    private void SpawnWhirlwind(GameObject whirlwind)
    {
        var go = Instantiate(whirlwind);
        go.transform.position = Center.transform.position;
    }

    private void ResetGamestate()
    {
        Choices.Clear();
        TimerStarted = false;
        ShowingResults = false;
        StopAllCoroutines();

        if (Ships.Count == 1)
        {
            EndGame();
        }
        else
        {
            HelperCanvas.SetActive(true);
        }
    }

    private IEnumerator Move(SpaceShipFiller ssf, bool forward)
    {
        var current = ssf.transform.position;
        var target = current + (ssf.transform.forward * (forward ? 2 : -2));
        float timer = 0;
        while (timer <= 1)
        {
            timer += Time.deltaTime;
            ssf.transform.position = Vector3.Slerp(current, target, timer);
            yield return null;
        }
        ssf.transform.position = target;
    }

    public void StartGame()
    {
        foreach (var ssf in Ships)
        {
            ssf.Key.OnButton0Press.AddListener(() => Choose(ssf.Key, ssf.Value, FireWaterNature.Water));
            ssf.Key.OnButton1Press.AddListener(() => Choose(ssf.Key, ssf.Value, FireWaterNature.Nature));
            ssf.Key.OnButton2Press.AddListener(() => Choose(ssf.Key, ssf.Value, FireWaterNature.Fire));
        }
        HelperCanvas.SetActive(true);
    }

    public void EndGame()
    {
        FindFirstObjectByType<GameFlow>().EndGame();
    }
}

public enum FireWaterNature
{
    Fire,
    Water,
    Nature
}