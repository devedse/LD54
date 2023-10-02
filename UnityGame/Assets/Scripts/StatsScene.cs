using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StatsScene : MonoBehaviour
{
    public GameObject StatsPrefab;

    public List<GameObject> Trophies = new List<GameObject>();


    public void Start()
    {
        var players = MinigameManager.Instance.SignalR.Players.Values.ToList();

        Trophies.Add(CreateTrophyCard(x => x.Stats.TiebreakerWins, players, "Tiebreaker Boss", new Color(234/255f,157/255f,43/255f), new Vector3(-8, 3.5f, 0)));
        Trophies.Add(CreateTrophyCard(x => x.Stats.AmountOfTimesOnPodium, players, "Most Podium Visits", new Color(197 / 255f, 79 / 255f, 201 / 255f), new Vector3(-4, 3.75f, 0)));
        Trophies.Add(CreateTrophyCard(x => x.Stats.BigfiteWins, players, "Grand Prize Winner", new Color(255 / 255f, 52 / 255f, 101 / 255f), new Vector3(0, 4f, 0)));
        Trophies.Add(CreateTrophyCard(x => x.Stats.MinigameWins, players, "Minigame Master", new Color(108 / 255f, 118 / 255f, 203 / 255f), new Vector3(4, 3.75f, 0)));
        Trophies.Add(CreateTrophyCard(x => x.Stats.LastPaces, players, "Participation Award", new Color(246 / 255f, 196 / 255f, 197 / 255f), new Vector3(8, 3.5f, 0)));

    }

    private void Update()
    {
        foreach (var g in Trophies)
        {
            g.transform.GetChild(0).localPosition = new Vector3(0, Mathf.Sin(Time.time + g.transform.position.x) * .2f, 0);
        }
    }

    public GameObject CreateTrophyCard(Func<PC, float> selector, IEnumerable<PC> players, string title, Color color, Vector3 Pos)
    {
        var parent = new GameObject();
        var podiums = GetTopFor(selector, players).ToList();
        var value = podiums.Max(selector);
        if (value == 0)
        {
            var asdf = new GameObject();
            asdf.transform.parent = parent.transform;
            return parent;
        }

        var cardObj = Instantiate(StatsPrefab);
        var trophyCard = cardObj.GetComponent<TrophyCard>();
        trophyCard.ShowPlayers(podiums, podiums.Max(selector), title, color);
        cardObj.transform.parent = parent.transform;
        parent.transform.position = Pos;
        return parent;
    }

    public IEnumerable<PC> GetTopFor(Func<PC, float> selector, IEnumerable<PC> players)
    {
        var max = players.Max(selector);
        return players.Where(x => selector(x) == max);
    }
}
