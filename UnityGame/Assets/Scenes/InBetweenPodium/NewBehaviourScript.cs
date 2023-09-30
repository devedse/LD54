using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

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

        var playerCount = math.min(3, MinigameManager.Instance.SignalR.Players.Count);
        for (int i = playerCount; i > 0; i--)
        {
            var spaceShip = GameObject.Instantiate(PrefabSpaceship, this.transform);
            
            if (i == 3)
            {
                spaceShip.transform.localPosition = new Vector3(5f, 0, 0);

                float start = Time.time;

                var totalDuration = 5f;

                while (Time.time - start < 5f)
                {
                    var diff = Time.time - start;

                    var wonk = new Vector3(Mathf.Sin(diff - 2f) * 5f, Mathf.Cos(diff - 2f) * 0.1f + 2f, Mathf.Cos(diff) * 5f);
                    var dest = new Vector3(2, 0.165f, 0);

                    spaceShip.transform.localPosition = Vector3.Lerp(wonk, dest, diff / totalDuration) ;
                    spaceShip.transform.LookAt(transform.position + new Vector3(0, 0.165f, 0));
                    yield return null;
                }
            }
            if (i == 2)
            {
                spaceShip.transform.localPosition = new Vector3(5f, 0, 0);

                float start = Time.time;

                var totalDuration = 5f;

                while (Time.time - start < 5f)
                {
                    var diff = Time.time - start;

                    var wonk = new Vector3(Mathf.Sin(diff * 10f) * 1f - 3f, Mathf.Cos(diff) * 2 + 2f, 5f);
                    var dest = new Vector3(-2, 0.4f, 0);

                    spaceShip.transform.localPosition = Vector3.Lerp(wonk, dest, diff / totalDuration);
                    spaceShip.transform.LookAt(transform.position + new Vector3(0, 0.4f, 0));
                    yield return null;
                }
            }
        }
    }
}
