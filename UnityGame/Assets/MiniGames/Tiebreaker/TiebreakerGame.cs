using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TiebreakerGame : MonoBehaviour
{
    public GameObject ShipPrefab;
    public List<TiebreakerShip> Ships;

    public int NextWinnerScore;

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

        Ships = new List<TiebreakerShip>();
        var maxScore = MinigameManager.Instance.SignalR.Players.Values.Select(x => x.Score).Max();
        foreach (var p in players.Where(x => x.Score == maxScore).OrderBy(x => x.PlayerIndex))
        {
            var shipInstance = Instantiate(ShipPrefab);
            var tbs = shipInstance.AddComponent<TiebreakerShip>();
            tbs.Game = this;
            tbs.PC = p;
            var sf = shipInstance.GetComponent<SpaceShipFiller>();
            Ships.Add(tbs);
            sf.SetProps(p, FaceType.Normal);
        }
        var shipCount = Ships.Count;
        NextWinnerScore = shipCount;
        for (int i = 0; i < shipCount; i++)
        {
            var pos = new Vector3(-6, ((9f / shipCount) * i) - 4.5f, 0);

            Ships[i].transform.position = pos;
            Ships[i].transform.eulerAngles = new Vector3(0, 90, 0);
        }
    }

    internal void CrossedFinishLine(TiebreakerShip tiebreakerShip)
    {
        tiebreakerShip.PC.ChangeScore(NextWinnerScore);
        NextWinnerScore--;
        if (NextWinnerScore == 0)
        {
            StartCoroutine(FinishGame());
        }
    }

    private IEnumerator FinishGame()
    {
        var c = FindFirstObjectByType<CountdownCanvasScript>(FindObjectsInactive.Include);
        c.OverrideText = "Finished!";
        c.gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        FindFirstObjectByType<GameFlow>().EndGame();
    }

    public void StartGame()
    {
        foreach (var ssf in Ships)
        {
            ssf.PC.OnButton0Press.AddListener(() => Boost(ssf));
            ssf.PC.OnButton1Press.AddListener(() => Boost(ssf));
            ssf.PC.OnButton2Press.AddListener(() => Boost(ssf));
        }
    }

    private void Boost(TiebreakerShip value)
    {
        value.Tapped();
    }
}
