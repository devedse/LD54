using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class PlayerReadyScreen : MonoBehaviour
{
    public GameObject CardPrefab;
    public List<PlayerReadyCard> Cards;
    public UnityEvent OnEveryoneIsReady;

    // Start is called before the first frame update
    void Start()
    {
        foreach (var p in MinigameManager.Instance.SignalR.Players)
        {
            p.Value.ResetButtonBindings();
            var cardInstance = Instantiate(CardPrefab, transform);
            var card = cardInstance.GetComponent<PlayerReadyCard>();
            card.Screen = this;
            card.PlayerName.text = p.Value.PlayerName;
            card.PlayerImageI.sprite = p.Value.PlayerImage;
            card.PlayerMaskI.sprite = p.Value.PlayerImage;
            Cards.Add(card);
            p.Value.OnButton1Press.AddListener(() => card.ToggleReady());
        }
    }

    public void UnaliveMyself()
    {
        foreach (var p in MinigameManager.Instance.SignalR.Players)
        {
            p.Value.ResetButtonBindings();
        }
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    internal void RefreshReadyCheck()
    {
        if (Cards.All(x => x.IsReady))
        {
            foreach (var p in MinigameManager.Instance.SignalR.Players)
            {
                p.Value.ResetButtonBindings();
            }
            OnEveryoneIsReady.Invoke();
        }
    }
}
