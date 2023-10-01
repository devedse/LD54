using System.Collections;
using UnityEngine;

public class Squirterscript : MonoBehaviour
{
    public ParticleSystem ParticleSystem;

    float nextCall = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        SetNextCall();
    }

    void SetNextCall()
    {

        nextCall = Time.time + Random.Range(0.5f, 10f);
    }

    //public IEnumerator Squirt(int times)
    //{
    //    if (times <= 0)
    //    {
    //        yield break;
    //    }
    //    else
    //    {
    //        ParticleSystem.Emit(Random.Range(10, 40));
    //        yield return new WaitForSeconds(Random.Range(0.1f, 0.6f));
    //        yield return Squirt(times - 1);
    //    }
    //}

    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextCall)
        {
            //StartCoroutine(Squirt(Random.Range(2, 8)));
            ParticleSystem.Play();
            SetNextCall();
        }
    }
}
