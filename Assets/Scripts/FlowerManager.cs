using System.Collections;
using UnityEngine;

public class FlowerManager : MonoBehaviour
{
    public bool CanEat = true;
    private Vector3 _scale;

    private void Awake()
    {
        _scale = transform.localScale;
        transform.localScale = Vector3.zero;

        CanEat = false;
        StartCoroutine(WaitAndCanEat(UnityEngine.Random.Range(10,50)));
    }

    private IEnumerator WaitAndCanEat(float waitTime)
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);
            CanEat = true;
        }
    }

    private void Update()
    {
        if(CanEat)
            transform.localScale = Vector3.Lerp(transform.localScale, _scale, Time.deltaTime);
    }

    public bool Eat()
    {
        var prevValue = CanEat;
        
        CanEat = false;
        transform.localScale = Vector3.zero;
         
        return prevValue;
    }
}
