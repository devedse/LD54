using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IngameScoreScreen : MonoBehaviour
{
    public GameObject EntryPrefab;

    public void Init()
    {
        foreach (var p in MinigameManager.Instance.SignalR.Players.Values.OrderBy(x => x.PlayerIndex))
        {
            var i = Instantiate(EntryPrefab, transform);
            var scoreCard = i.GetComponent<IngameScoreScreenCard>();
            scoreCard.Init(p);
            p.Cards.Add(scoreCard);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
