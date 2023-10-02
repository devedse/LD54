using System.Collections;
using System.Linq;
using UnityEngine;

public class ArenaIntroducer : MonoBehaviour
{
    public GameObject Camera;
    public GameObject Arena;

    public GameObject RootOfLights;
    public Light DirectionalLight;

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

    private bool _lightsDisabled = false;

    IEnumerator GogoMoveLights()
    {
        var allLights = RootOfLights.GetComponentsInChildren<Light>();
        var allStartRotations = allLights.Select(light => light.transform.localRotation).ToArray();
        var startTime = Time.time;

        float inbetweenLights = Mathf.PI;
        float speedMultiplier = 1f;

        while (!_lightsDisabled)
        {
            var elapsed = Time.time - startTime;
            //Move lights around

            for (int i = 0; i < allLights.Length; i++)
            {
                var light = allLights[i];
                float rotatoRange = 12.5f;

                var lookatPos = new Vector3(Mathf.Sin((i * inbetweenLights) + elapsed * speedMultiplier) * rotatoRange, 0, Mathf.Cos((i * inbetweenLights) + elapsed * speedMultiplier) * rotatoRange);

                light.transform.LookAt(lookatPos);
            }
            yield return null;
        }


    }

    IEnumerator GogoDimLightsAndActivateNormalLight()
    {
        var allLights = RootOfLights.GetComponentsInChildren<Light>();
        var allStartRotations = allLights.Select(light => light.transform.localRotation).ToArray();
        var startTime = Time.time;

        var originalIntensity = allLights[0].intensity;

        float duration = 3f;

        while (Time.time - startTime < duration)
        {
            var elapsed = Time.time - startTime;

            for (int i = 0; i < allLights.Length; i++)
            {
                var light = allLights[i];
                light.intensity = Mathf.Lerp(originalIntensity, 0, elapsed / duration);
            }
            DirectionalLight.intensity = Mathf.Lerp(0, 1, elapsed / duration);
            yield return null;
        }

        _lightsDisabled = true;

        foreach (var light in allLights)
        {
            light.enabled = false;
        }
        DirectionalLight.intensity = 1;
    }

    IEnumerator GogoArenaIntroduce()
    {
        StartCoroutine(GogoMoveLights());

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

        StartCoroutine(GogoDimLightsAndActivateNormalLight());

        CountdownCanvasScript.StartCountdown();


    }
}
