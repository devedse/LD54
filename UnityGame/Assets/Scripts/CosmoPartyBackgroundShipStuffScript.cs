using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;

public class CosmoPartyBackgroundShipStuffScript : MonoBehaviour
{
    public int shipCount;
    public GameObject shipPrefab;

    public float maxSpeed;
    public float maxAcceleration;

    public BoxCollider Area;

    private List<GameObject> shipList = new List<GameObject>();
    private Dictionary<GameObject, Vector3> shipTargets = new Dictionary<GameObject, Vector3>();
    private Dictionary<GameObject, Vector3> shipSpeed = new Dictionary<GameObject, Vector3>();

    public PlayerToColorMappingScriptableObject PlayerColors;
    public PlayerImagesScriptableObject PlayerImages;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < shipCount; i++)
        {
            var ga = Instantiate(shipPrefab, transform);
            ga.transform.localPosition = GetRandomPositionOutsideArea();
            var fakePC = ga.AddComponent<PC>();
            var bb = ga.AddComponent<BeatBonker>();
            bb.chanceToTrigger = 0.2f;

            fakePC.PlayerIndex = i;

            fakePC.PlayerColor = PlayerColors.Colors[i % PlayerColors.Colors.Count];
            var thisCharacter = PlayerImages.Images[i % PlayerImages.Images.Count];
            fakePC.PlayerImage = thisCharacter.ImageIdle;
            fakePC.PlayerMad = thisCharacter.ImageSad;
            fakePC.PlayerHappy = thisCharacter.ImageWin;
            fakePC.Template = thisCharacter;

            var shipFiller = ga.GetComponent<SpaceShipFiller>();
            shipFiller.SetProps(fakePC, FaceType.Happy, true);

            shipList.Add(ga);
            shipTargets.Add(ga, GetRandomPositionInArea());
            shipSpeed.Add(ga, Vector3.zero);
        }
    }

    private Vector3 GetRandomPositionInArea()
    {
        var x = Random.Range(Area.bounds.min.x, Area.bounds.max.x);
        var y = Random.Range(Area.bounds.min.y, Area.bounds.max.y);
        var z = Random.Range(Area.bounds.min.z, Area.bounds.max.z);

        return new Vector3(x, y, z);
    }

    private Vector3 GetRandomPositionOutsideArea()
    {
        var left = Random.Range(0, 2) < 1;

        int startXOutside = 20;
        int startXInside = 10;

        var x = left ? Random.Range(Area.bounds.min.x - startXOutside, Area.bounds.min.x - startXInside) : Random.Range(Area.bounds.max.x + startXInside, Area.bounds.max.x + startXOutside);
        var y = Random.Range(Area.bounds.min.y, Area.bounds.max.y);
        var z = Random.Range(Area.bounds.min.z, Area.bounds.max.z);

        return new Vector3(x, y, z);
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var ship in shipList)
        {
            var position = ship.transform.localPosition;
            var speed = shipSpeed[ship];
            var target = shipTargets[ship];

            var distance = Vector3.Distance(position, target);
            if (distance < 0.3f)
            {
                target = GetRandomPositionInArea();
            }

            var direction = (target - position).normalized;

            //ship.transform.LookAt(target);

            // Accelerate the ship towards its target up to its desired speed
            speed = Vector3.MoveTowards(speed, direction * maxSpeed, maxAcceleration * Time.deltaTime);

           


            // Move the ship
            position += speed * Time.deltaTime;


            // Store everything in dictionaries again
            shipTargets[ship] = target;
            shipSpeed[ship] = speed;
            ship.transform.localPosition = position;

            ship.transform.LookAt(ship.transform.position + speed);
        }
    }
}
