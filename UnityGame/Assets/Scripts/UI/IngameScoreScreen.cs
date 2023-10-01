using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IngameScoreScreen : MonoBehaviour
{
    public GameObject EntryPrefab;

    public bool IsSplit;
    public GameObject SplitOne;
    public GameObject SplitTwo;

    private bool ToSplitOne = true;

    public void Init()
    {
        foreach (var p in MinigameManager.Instance.SignalR.Players.Values.OrderBy(x => x.PlayerIndex))
        {
            GameObject i;
            if (IsSplit)
            {
                if (ToSplitOne)
                {
                    i = Instantiate(EntryPrefab, SplitOne.transform);
                }
                else
                {
                    i = Instantiate(EntryPrefab, SplitTwo.transform);
                }
                ToSplitOne = !ToSplitOne;
            }
            else
            {
                i = Instantiate(EntryPrefab, transform);
            }
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
