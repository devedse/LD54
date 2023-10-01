using NUnit.Framework.Internal;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Video;

public class NewBehaviourScript : MonoBehaviour
{
    public GameObject PrefabSpaceship;

    // Start is called before the first frame update
    void Start()
    {
        var allChildren = new List<GameObject>();
        foreach (Transform child in transform)
        {
            allChildren.Add(child.gameObject);
        }
        StartCoroutine(GoMoveObjects(allChildren));
    }

    // Update is called once per frame
    void Update()
    {

    }


    public GameObject InstantiateSpaceShipForPlayer(int player, FaceType faceType)
    {
        var spaceShip = GameObject.Instantiate(PrefabSpaceship, this.transform);

        var playerObj = MinigameManager.Instance.SignalR.GetPlayerByNumber(player);

        spaceShip.GetComponent<SpaceShipFiller>().SetProps(playerObj, faceType);

        return spaceShip;
    }


    IEnumerator GoGetOtherPlayers()
    {
        var totalPlayersRemaining = MinigameManager.Instance.SignalR.Players.Count - 3;
        float rangeLeft = -2f;
        float rangeRight = 2f;

        float xPos = -2f;

        for (int i = MinigameManager.Instance.SignalR.Players.Count - 1; i >= 3; i--)
        {
            float inBetween = (rangeRight - rangeLeft) / (totalPlayersRemaining); // Subtract by 1 to consider start and end points

            var to = new Vector3(rangeLeft + (inBetween * (i - 3)), 0, xPos);
            var from = to + new Vector3(0, 0, -3f);

            var spaceShip = InstantiateSpaceShipForPlayer(i, FaceType.Normal);
            var scaleForViewers = 0.35f;
            spaceShip.transform.localScale = new Vector3(scaleForViewers, scaleForViewers, scaleForViewers);


            StartCoroutine(GoLerpViewer(spaceShip, from, to, 2f));
            yield return new WaitForSeconds(0.5f);
        }
    }

    IEnumerator GoLerpViewer(GameObject toMove, Vector3 from, Vector3 to, float duration)
    {
        float start = Time.time;

        while (Time.time - start < duration)
        {
            var diff = Time.time - start;

            toMove.transform.localPosition = Vector3.Lerp(from, to, diff / duration);
            yield return null;
        }
    }

    IEnumerator GoMoveObjects(List<GameObject> toMove)
    {
        var originalPositions = new Dictionary<GameObject, Vector3>();

        foreach (var m in toMove)
        {
            originalPositions[m] = m.transform.localPosition;
            m.transform.localPosition += new Vector3(0, -1, 0);
        }


        foreach (var m in toMove)
        {
            float start = Time.time;


            while (Time.time - start < 0.5f)
            {
                m.transform.localPosition = Vector3.Lerp(m.transform.localPosition, originalPositions[m], Time.time - start);
                yield return null;
            }
            m.transform.localPosition = originalPositions[m];
        }

        StartCoroutine(GoGetOtherPlayers());


        var playerCount = math.min(3, MinigameManager.Instance.SignalR.Players.Count);
        for (int i = playerCount - 1; i >= 0; i--)
        {
            var spaceShip = InstantiateSpaceShipForPlayer(i, FaceType.Happy);

            if (i == 2)
            {
                spaceShip.transform.localPosition = new Vector3(5f, 0, 0);

                float start = Time.time;

                var totalDuration = 2f;

                while (Time.time - start < totalDuration)
                {
                    var diff = Time.time - start;

                    var wonk = new Vector3(Mathf.Cos(diff - 2f) * 5f, Mathf.Sin(diff - 2f) * 0.1f + 2f, Mathf.Cos(diff) * 5f);
                    var dest = new Vector3(2, 0.33f, 0);

                    spaceShip.transform.localPosition = Vector3.Lerp(wonk, dest, diff / totalDuration);
                    spaceShip.transform.LookAt(transform.position + new Vector3(0, 0.33f, 0));
                    yield return null;
                }
            }
            if (i == 1)
            {
                spaceShip.transform.localPosition = new Vector3(5f, 0, 0);

                float start = Time.time;

                var totalDuration = 3f;

                while (Time.time - start < totalDuration)
                {
                    var diff = Time.time - start;

                    var wonk = new Vector3(Mathf.Cos(diff * 10f) * 1f - 3f, Mathf.Sin(diff) * 2 + 2f, 5f);
                    var dest = new Vector3(-2, 0.66f, 0);

                    spaceShip.transform.localPosition = Vector3.Lerp(wonk, dest, diff / totalDuration);
                    spaceShip.transform.LookAt(transform.position + new Vector3(0, 0.66f, 0));
                    yield return null;
                }
            }
            if (i == 0)
            {
                var from = new Vector3(-5f, 1, -1f);
                var to = new Vector3(5f, 1, -1f);
                spaceShip.transform.LookAt(new Vector3(100f, 1f, -1f));

                float start = Time.time;
                var totalDuration = 2f;

                while (Time.time - start < totalDuration)
                {
                    var diff = Time.time - start;

                    spaceShip.transform.localPosition = Vector3.Lerp(from, to, diff / totalDuration);
                    yield return null;
                }

                start = Time.time;
                totalDuration = 3f;

                while (Time.time - start < totalDuration)
                {
                    var diff = Time.time - start;

                    var wonk = new Vector3(Mathf.Cos(diff * 5f) * 2f + 15f, Mathf.Sin(diff * 3f) * 5 + 2f, 2f);
                    var dest = new Vector3(0, 1f, 0);

                    spaceShip.transform.localPosition = Vector3.Lerp(wonk, dest, diff / totalDuration);
                    spaceShip.transform.LookAt(transform.position + new Vector3(-1, 1f, 0));
                    yield return null;
                }
            }
        }

        yield return new WaitForSeconds(2f);

        FindFirstObjectByType<GameFlow>().ClaimReward();
    }
}
