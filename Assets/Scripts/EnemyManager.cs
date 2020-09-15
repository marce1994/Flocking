using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    Waypoint currWp;
    private void Awake()
    {
        currWp = FindObjectsOfType<Waypoint>().First(x => x.name == "EnemyWayPoint");
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(currWp.transform.position);
        transform.position += (currWp.transform.position-transform.position).normalized * Time.deltaTime * 10;
        if (Vector3.Distance(currWp.transform.position, transform.position) < 1)
            currWp = currWp.getNextWaypoint();
    }
}
