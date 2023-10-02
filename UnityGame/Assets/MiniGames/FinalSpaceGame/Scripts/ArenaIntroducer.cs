using System.Collections;
using UnityEngine;

public class ArenaIntroducer : MonoBehaviour
{
    public GameObject Camera;
    public GameObject Arena;

    public CountdownCanvasScript CountdownCanvasScript;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GogoArenaIntroduce());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator GogoArenaIntroduce()
    {
        var startTime = Time.time;

        var originalCameraPosition = Camera.transform.localPosition;
        var originalCameraRotation = Camera.transform.localRotation;

        //Rotate camera around arena starting at 0,35,-24
        while (true)
        {
            var elapsed = Time.time - startTime;

            float rotatoRange = 35;

            var rotatoX = Mathf.Sin(elapsed * 1f) * rotatoRange;
            var rotatoZ = Mathf.Cos(elapsed * 1f) * -rotatoRange;
            Camera.transform.localPosition = new Vector3(rotatoX, 12, rotatoZ);
            Camera.transform.LookAt(Arena.transform);
            yield return null;

            if (elapsed > 2 * Mathf.PI)
            {
                break;
            }
        }

        var curPositione = Camera.transform.localPosition;
        var curRotatione = Camera.transform.localRotation;
        startTime = Time.time;
        while (true)
        {
            var elapsed = Time.time - startTime;

            Camera.transform.localPosition = Vector3.Lerp(curPositione, originalCameraPosition, elapsed);
            Camera.transform.localRotation = Quaternion.Lerp(curRotatione, originalCameraRotation, elapsed);
            yield return null;

            if (elapsed > 1)
            {
                Camera.transform.localPosition = originalCameraPosition;
                Camera.transform.localRotation = originalCameraRotation;
                break;
            }
        }




        CountdownCanvasScript.StartCountdown();


    }
}
