using System.Collections;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    Waypoint currWp;
    Vector3 dir;

    private void Awake()
    {
        StartCoroutine(UpdateDirection(5));
    }

    private IEnumerator UpdateDirection(float waitTime)
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);
            dir = Random.insideUnitSphere;
            dir.y = 0;
            dir = dir.normalized;
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(transform.position + dir);
        transform.position += dir * Time.deltaTime * 10;
        
        if (transform.position.magnitude > 100)
            dir = -transform.position.normalized;
    }
}
